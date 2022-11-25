﻿using Shared.DTOs;

namespace Application.LogicInterfaces;

public interface IFarmLogic
{ 
    /// <summary>
    /// Create asynchronously a Farm using the FarmCreationDto object and return it as a Task
    /// </summary>
    /// <param name="dto">The object holding all the farm information</param>
    /// <returns>The created Farm object</returns>
    Task<Farm> CreateAsync(FarmCreationDto dto);

    Task<Farm> GetFarmByNameAsync(string farmName);
}