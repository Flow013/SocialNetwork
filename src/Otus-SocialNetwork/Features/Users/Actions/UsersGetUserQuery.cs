using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Otus_SocialNetwork.Features.Users.Services;
using OtusSocialNetwork.Database;
using OtusSocialNetwork.DataClasses.Dtos;

namespace Otus_SocialNetwork.Features.Users.Actions
{
    public static class UsersGetUserQuery
    {
        public class UsersGetUserQueryRequest : IRequest<Result<UserDto>>
        {
            [FromRoute(Name = "id")]
            public Guid Id { get; set; }
        }


        public class Handler : IRequestHandler<UsersGetUserQueryRequest, Result<UserDto>>
        {
            private readonly IDatabaseContext _db;
            private readonly IMapper _mapper;

            public Handler(IDatabaseContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<Result<UserDto>> Handle(UsersGetUserQueryRequest request, CancellationToken cancellationToken)
            {
                var dbRes = await _db.GetUserAsync(request.Id.ToString());
                if (!dbRes.isSuccess)
                {
                    return Result.Failure<UserDto>(dbRes.msg);
                }

                return _mapper.Map<UserDto>(dbRes.user);
            }
        }
    }
}