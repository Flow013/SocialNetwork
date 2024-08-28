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

public static class AuthLoginCommand
{
    public class AuthLoginRequest : IRequest<Result<AuthLoginResponse>>
    {
        [FromBody]
        public AuthLoginCommandBody Body { get; set; }

        public class AuthLoginCommandBody
        {

            [Required]
            public string Id { get; set; }

            [Required]
            public string Password { get; set; }
        }
    }

    public class AuthLoginResponse
    {
        public AuthLoginResponse(string token)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public string Token { get; set; }
    }

    public class Handler : IRequestHandler<AuthLoginRequest, Result<AuthLoginResponse>>
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

        public async Task<Result<AuthLoginResponse>> Handle(AuthLoginRequest request, CancellationToken cancellationToken)
        {
            var login = await _db.GetLoginAsync(request.Body.Id);
            if (login.isSuccess)
            {
                var isPasswordOk = _pass.VerifyHashedPassword(login.account.Password, request.Body.Password);
                if (isPasswordOk)
                {
                    var jwt = await GenerateJWToken(login.account.Id);
                    var token = new JwtSecurityTokenHandler().WriteToken(jwt);
                    return new AuthLoginResponse(token);
                }
            }
            else
            {
                return Result.Failure<AuthLoginResponse>(login.msg);
            }
            return Result.Failure<AuthLoginResponse>("No login");

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
