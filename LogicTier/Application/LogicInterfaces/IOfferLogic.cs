﻿using Shared.DTOs;
using Shared.Models;

namespace Application.LogicInterfaces;

/// <summary>
/// Logic interface for the farm
/// </summary>
public interface IOfferLogic
{
    Task<Offer> CreateAsync(OfferCreationDto dto); //OfferCreation dto
    Task<IEnumerable<Offer>> GetAsync(SearchOfferParameterDto dto);
    Task<IEnumerable<Offer>> GetByFarmNameAsync(string farmName);
    Task DisableAsync(int id);
    Task<IEnumerable<Offer>> GetRecommendedAsync(string username);
}