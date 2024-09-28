using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Otus_SocialNetwork.Database.Entities;
using Otus_SocialNetwork.Features.Users.Services;
using OtusSocialNetwork.Database;
using System.ComponentModel.DataAnnotations;
using static Otus_SocialNetwork.Features.Users.Actions.AuthRegisterCommand.AuthRegisterRequest;

namespace Otus_SocialNetwork.Features.Users.Actions
{
    public static class AuthRegisterCommand
    {
        public class AuthRegisterRequest : IRequest<Result<AuthRegisterResponse>>
        {
            [FromBody]
            public AuthRegisterCommandBody Body { get; set; }

            public class AuthRegisterCommandBody
            {
                [Required]
                public string First_name { get; set; }

                [Required]
                public string Second_name { get; set; }

                [Required]
                public int Age { get; set; }

                [Required]
                public string Sex { get; set; }

                [Required]
                public string Biography { get; set; }

                [Required]
                public string City { get; set; }

                [Required]
                [MinLength(6)]
                public string Password { get; set; }
            }
        }

        public class AuthRegisterResponse
        {
            public AuthRegisterResponse(string userId)
            {
                UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            }

            public string UserId { get; set; }
        }

        public class AuthRegisterCommandProfile : Profile
        {
            public AuthRegisterCommandProfile()
            {
                CreateMap<AuthRegisterCommandBody, UserEntity>()
                    .ForMember(e => e.Id, opt => opt.MapFrom(r => Guid.NewGuid().ToString()));
            }
        }

        public class Handler : IRequestHandler<AuthRegisterRequest, Result<AuthRegisterResponse>>
        {
            private readonly IDatabaseContext _db;
            private readonly IMapper _mapper;
            private readonly IPasswordService _pass;

            public Handler(IDatabaseContext db, IMapper mapper, IPasswordService pass)
            {
                _db = db;
                _mapper = mapper;
                _pass = pass;
            }

            public async Task<Result<AuthRegisterResponse>> Handle(AuthRegisterRequest request, CancellationToken cancellationToken)
            {
                var user = _mapper.Map<UserEntity>(request.Body);

                var password = _pass.HashPassword(request.Body.Password);

                var dbRes = await _db.RegisterAsync(user, password);

                if (!dbRes.isSuccess)
                {
                    return Result.Failure<AuthRegisterResponse>(dbRes.msg);
                }

                return new AuthRegisterResponse(dbRes.userId);
            } 
        }
    }
}