using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using OtusSocialNetwork.Database;
using OtusSocialNetwork.DataClasses.Dtos;

namespace Otus_SocialNetwork.Features.Users.Actions
{
    public static class UsersSearchQuery
    {
        public class UsersSearchQueryRequest : IRequest<Result<UserDto[]>>
        {
            [FromQuery(Name = "first_name")]
            [ValidateNever]
            public string FirstName { get; set; } = string.Empty;

            [FromQuery(Name = "last_name")]
            [ValidateNever]
            public string LastName { get; set; } = string.Empty;

        }


        public class Handler : IRequestHandler<UsersSearchQueryRequest, Result<UserDto[]>>
        {
            private readonly IDatabaseContext _db;
            private readonly IMapper _mapper;

            public Handler(IDatabaseContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<Result<UserDto[]>> Handle(UsersSearchQueryRequest request, CancellationToken cancellationToken)
            {
                var dbRes = await _db.SearchUserAsync(request.FirstName, request.LastName);

                if (!dbRes.isSuccess)
                {
                    return Result.Failure<UserDto[]>(dbRes.msg);
                }

                return _mapper.Map<UserDto[]>(dbRes.users);
            }
        }
    }
}