﻿using Application.LogicInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Models;
using WebAPI.Controllers.Base;
using WebAPI.Utils;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class FarmController : LocallyController
{
    private readonly IFarmLogic farmLogic;
    private readonly IReviewLogic reviewLogic;

    public FarmController(IFarmLogic farmLogic, IReviewLogic reviewLogic)
    {
        this.farmLogic = farmLogic;
        this.reviewLogic = reviewLogic;
    }
    
    /// <summary>
    /// The method asynchronously creates a Farm object. If the object is correctly
    /// created them it is returned, and if not the exception is caught and a status code
    /// is sent to the user. 
    /// </summary>
    /// <param name="dto">A FarmCreationDto containing all information Required to create a Farm object</param>
    /// <returns>Returns the farm object or a status code to indicate an error</returns>
    [HttpPost, Authorize(Roles = "farmer")]
    public async Task<IActionResult> CreateAsync(FarmCreationDto dto)
    {
        try
        {
            Shared.Models.Farm created = await farmLogic.CreateAsync(dto);
            return Created($"/farms/{created.Name}", created);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }


    /// <summary>
    /// Gets all the available farm icons
    /// </summary>
    /// <returns></returns>
    [HttpGet("icons")]
    public IActionResult GetAllIcons()
    {
        return Ok(farmLogic.GetAllIcons());
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAsync([FromQuery]string farmName)
    {
        try
        {
            var farm = await farmLogic.GetFarmByNameAsync(farmName);
            return Created($"/Farms", farm);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
        

    [HttpGet("me")]
    [Authorize(Roles = "farmer")]
    public async Task<IActionResult> GetAllFarmsByFarmer()
    {
        try
        {
            var created = await farmLogic.GetAllFarmsByFarmer(LoggedInUsername!);
            return Created($"/farms", created);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    
    [HttpPatch]
    public async Task<ActionResult> UpdateAsync([FromBody]FarmUpdateDto dto)
    {
        string username = LoggedInUsername;
        try
        {
            
            await farmLogic.UpdateFarmAsync(dto);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("all")]
    [Authorize]
    public async Task<IActionResult> GetAllFarms()
    {
        try
        {
            var created = await farmLogic.GetAllAsync();
            return Created($"/farms", created);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("{nameContains}")]
    [Authorize]
    public async Task<IActionResult> GetAllFarmsByName([FromRoute]string nameContains)
    {
        try
        {
            var created = await farmLogic.GetAllByNameAsync(nameContains);
            return Created($"/farms", created);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("{name:regex([[\\w\\W]]+)}/reviews")]
    [Authorize(Roles = "customer")]
    public async Task<IActionResult> PostReview(string name, ReviewCreationDto dto)
    {
        try
        {
            var review = await reviewLogic.CreateReview(name, LoggedInUsername!, dto);
            return Ok(review);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("{name:regex([[\\w\\W]]+)}/reviews")]
    public async Task<IActionResult> GetReviews()
    {
        return Ok();
    }
    
    [HttpPatch("{name:regex([[\\w\\W]]+)}/reviews/{id}")]
    public async Task<IActionResult> UpdateReview()
    {
        return Ok();
    }
}