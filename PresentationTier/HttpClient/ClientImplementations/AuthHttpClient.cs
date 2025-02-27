﻿using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using HttpClient.ClientInterfaces;
using HttpClient.Utils;
using Shared.DTOs;
using Shared.Models;

namespace HttpClient.ClientImplementations;

/// <summary>
/// Http implementation of the <see cref="IAuthService"/> interface.
/// </summary>
public class AuthHttpClient : IAuthService
{
    // accept httpclient from constructor to field
    private System.Net.Http.HttpClient Client => _access.HttpClient;
    public static string? Jwt { get; private set; } = "";
    
    public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; } = null!;

    private readonly ApiAccess _access;
    public AuthHttpClient(ApiAccess apiAccess)
    {
        _access = apiAccess;
    }
    
    /// <inheritdoc/>
    public async Task<User> LoginAsync(string username, string password)
    {
        LoginDto dto = new LoginDto()
        {
            Username = username,
            Password = password
        };

        HttpResponseMessage response = await Client.PostAsJsonAsync("/auth/login", dto);
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
        
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        if(loginResponse == null)
            throw new Exception("Cannot read user from response");

        
        await _access.LoginAsync(loginResponse.Token);
        
        Jwt = loginResponse.Token;
        ClaimsPrincipal claimsPrincipal = AuthUtils.CreateClaimsPrincipal(Jwt);
        OnAuthStateChanged.Invoke(claimsPrincipal);
        
        //was returning Login response but isnt User better?
        return loginResponse.User;
    }

    /// <inheritdoc/>
    public async Task<User> RegisterAsync(string firstName, string lastName, string username, string password, bool isFarmer, string city, string street, string zip, string phoneNumber)
    {
        RegisterDto dto = new RegisterDto()
        {
            Username = username,
            Password = password,
            IsFarmer = isFarmer,
            FirstName = firstName,
            LastName = lastName,
            City = city,
            Street = street,
            ZIP = zip,
            PhoneNumber = "+45" + phoneNumber
        };

        HttpResponseMessage response = await Client.PostAsJsonAsync("/auth/register", dto);
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
        
        var user = await response.Content.ReadFromJsonAsync<User>();
        if(user == null)
            throw new Exception("Cannot read user from response");
        
        return user;
    }

    public Task<ClaimsPrincipal> GetAuthAsync()
    {
        ClaimsPrincipal principal = AuthUtils.CreateClaimsPrincipal(Jwt);
        return Task.FromResult(principal);  
    }


    // Below methods stolen from https://github.com/SteveSandersonMS/presentation-2019-06-NDCOslo/blob/master/demos/MissionControl/MissionControl.Client/Util/ServiceExtensions.cs
    

    
}