﻿namespace Otus_SocialNetwork.Features.Users.Services;

public interface IPasswordService
{
    string HashPassword(string password);

    bool VerifyHashedPassword(string hashedPassword, string providedPassword);
}
