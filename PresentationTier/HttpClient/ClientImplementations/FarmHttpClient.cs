﻿using System.Net.Http.Json;
using System.Text.Json;
using HttpClient.ClientInterfaces;
using Shared.DTOs;
using Shared.Models;

namespace HttpClient.ClientImplementations;

public class FarmHttpClient : IFarmService
{
    private readonly System.Net.Http.HttpClient client;

    public FarmHttpClient(System.Net.Http.HttpClient client)
    {
        this.client = client;
    } 
    
    
    
    public async Task CreateAsync(FarmCreationDto dto)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/farm", dto);
        string content = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    public async Task<ICollection<FarmIcon>> GetAllIconsAsync()
    {
        var response = await client.GetAsync("/farm/icons");
        var content = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var deserialized = JsonSerializer.Deserialize<ICollection<FarmIcon>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        return deserialized ?? new List<FarmIcon>(); 
    }
}