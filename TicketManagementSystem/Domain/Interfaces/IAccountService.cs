﻿using Domain.DTOs.Requests;
using Domain.DTOs.Responses;

namespace Domain.Interfaces;

/// <summary>
/// Represents an interface for the account service.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Verifies the user with the specified email and password.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response with a string value.</returns>
    Task<BaseResponse<string>> VerifyUser(string email, string password);

    /// <summary>
    /// Registers a new user with the specified request.
    /// </summary>
    /// <param name="request">The request containing the user registration information.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response.</returns>
    Task<BaseResponse> RegisterUser(RegisterUserRequest request);
}