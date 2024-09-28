using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Otus_SocialNetwork.DataClasses.Internals;
using Otus_SocialNetwork.Features.Users.Services;
using OtusSocialNetwork.Database;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Otus_SocialNetwork.Features.Users.Actions;

public class UsersLoginCommand
{
    public class UsersLoginRequest : IRequest<Result<UsersLoginResponse>>
    {
        [FromBody]
        public UsersLoginCommandBody Body { get; set; }

        public class UsersLoginCommandBody
        {

            [Required]
            public string Id { get; set; }

            [Required]
            public string Password { get; set; }
        }
    }

    public class UsersLoginResponse
    {
        public UsersLoginResponse(string token)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public string Token { get; set; }
    }

    public class Handler : IRequestHandler<UsersLoginRequest, Result<UsersLoginResponse>>
    {
        private readonly IDatabaseContext _db;
        private readonly IPasswordService _pass;
        private readonly JWTSettings _jwtSettings;

        public Handler(IDatabaseContext db, IPasswordService pass, IOptions<JWTSettings> jwtSettings)
        {
            _db = db;
            _pass = pass;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<Result<UsersLoginResponse>> Handle(UsersLoginRequest request, CancellationToken cancellationToken)
        {
            var login = await _db.GetLoginAsync(request.Body.Id);
            if (login.isSuccess)
            {
                var isPasswordOk = _pass.VerifyHashedPassword(login.account.Password, request.Body.Password);
                if (isPasswordOk)
                {
                    var jwt = await GenerateJWToken(login.account.Id);
                    var token = new JwtSecurityTokenHandler().WriteToken(jwt);
                    return new UsersLoginResponse(token);
                }
            }
            else
            {
                return Result.Failure<UsersLoginResponse>(login.msg);
            }
            return Result.Failure<UsersLoginResponse>("No login");

        }

        private async Task<JwtSecurityToken> GenerateJWToken(string userId)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}
