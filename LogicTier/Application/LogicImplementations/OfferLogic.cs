﻿using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Shared.DTOs;
using Shared.Models;
using Offer = Shared.Models.Offer;

namespace Application.LogicImplementations;

/// <summary>
/// Offer logic that implements from <see cref="IOfferDao"/> that is responsible for checking and validating data
/// </summary>
public class OfferLogic : IOfferLogic
{
    private IOfferDao offerDao;
    private IImageDao imageDao;
    private IFarmDao farmDao;
    private IUserDao userDao;
    private ICartDao cartDao;

    /// <summary>
    /// Initializing the OfferLogic with the given IOfferDao
    /// </summary>
    /// <param name="offerDao"></param>
    public OfferLogic(IOfferDao offerDao, IImageDao imageDao, IFarmDao farmDao, IUserDao userDao, ICartDao cartDao)
    {
        this.offerDao = offerDao;
        this.imageDao = imageDao;
        this.farmDao = farmDao;
        this.userDao = userDao;
        this.cartDao = cartDao;
    }
    
    /// <summary>
    /// Create asynchronously a Offer using the OfferCreationDto object. Call the CreateAsync method in gRPC client 
    /// </summary>
    /// <param name="dto">The object holding all the offer information</param>
    /// <returns>The created Offer object</returns>
    public async Task<Offer> CreateAsync(OfferCreationDto dto)
    {
        await ValidateData(dto);

        Offer offerToSend = new Offer
        {
            FarmName = dto.FarmName,
            Name = dto.Name,
            Quantity = dto.Quantity,
            Unit = dto.Unit,
            Price = dto.Price,
            CollectionOption = dto.CollectionOption,
            Description = dto.Description,
            Image = new Image()
            {
                RelativeUrl = ""
            },
        };

        var created = await offerDao.CreateAsync(offerToSend);
        
        return created;
    }

    /// <summary>
    /// Getting asynchronously all the offers through the IOfferDao
    /// </summary>
    /// <returns>A Collection of Offers</returns>
    public async Task<IEnumerable<Offer>> GetAsync(SearchOfferParameterDto dto)
    {
        //get all offers
        var results = await offerDao.GetAsync();

        if (!string.IsNullOrEmpty(dto.NameContains))
        {
            results = results.Where(offer => offer.Name.Contains(dto.NameContains, StringComparison.OrdinalIgnoreCase));
        }

        if (dto.Delivery != null && dto.Delivery != false)
        {
            results = results.Where(offer => (offer.CollectionOption & CollectionOption.Delivery) != 0);
        }

        if (dto.PickUp != null && dto.PickUp != false)
        {
            results = results.Where(offer => (offer.CollectionOption & CollectionOption.PickUp) != 0);
        }

        if (dto.PickYO != null && dto.PickYO != false)
        {
            results = results.Where(offer => (offer.CollectionOption & CollectionOption.PickYourOwn) != 0);
        }

        //1.need to check customer's address (get it by username which will be send from the view)
        //2.then get the farm's address by the farm name in offer
        //3.remove the offers where the distance is bigger than the distance specified in the dto
        if (dto.Distance != null && dto.Distance != 0)
        {
            List<Offer> offersToRemove = await GetResultsInCorrectDistance(results, dto.Username, dto.Distance);
            results = results.Except(offersToRemove);
        }

        return results;
    }

    public async Task<IEnumerable<Offer>> GetByFarmNameAsync(string farmName)
    {
        var results = await offerDao.GetByFarmNameAsync(farmName);
        return results;
    }

    /// <summary>
    /// Will disable the offer with the given id
    /// </summary>
    public async Task DisableAsync(int id)
    {
        //check if offer exists??? -- 
        try
        {
            Offer offerToDelete = await offerDao.GetOfferByIdAsync(id);
            await offerDao.DisableAsync(id);

            await cartDao.DeleteAllByOfferIdAsync(id);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public async Task<IEnumerable<Offer>> GetRecommendedAsync(string username)
    {
        int[] distances = { 10, 25, 50, 70, 100 };
        var filter = new SearchOfferParameterDto();

        IEnumerable<Offer> offers = new List<Offer>();

        foreach (int distance in distances)
        {
            filter.Distance = distance;
            
            offers = (await GetAsync(filter)).ToList();
            if (offers.Count() >= 20)
                break;
        }

        return offers.Take(20);
    }


    private async Task<List<Offer>> GetResultsInCorrectDistance(IEnumerable<Offer> results, string? dtoUsername,
        int? dtoDistance)
    {
        if (dtoDistance is null)
            throw new ArgumentNullException(nameof(dtoDistance));


        List<Offer> offersToRemove = new List<Offer>();
        if (!string.IsNullOrEmpty(dtoUsername))
        {
            Customer? customer = await userDao.GetCustomer(dtoUsername);
            if (customer is null)
                throw new Exception("Customer not found");

            foreach (var offer in results)
            {
                Farm farm = await farmDao.GetFarmByNameAsync(offer.FarmName);
                
                double distance = customer.Address - farm.Address;

                if (dtoDistance <= distance)
                {
                    offersToRemove.Add(offer);
                }
            }
        }
        return offersToRemove;
    }



    /// <summary>
    /// Validating the data when creating an offer
    /// </summary>
    /// <param name="dto">The offer to be created</param>
    /// <exception cref="Exception"></exception>
    private async Task ValidateData(OfferCreationDto dto)
    {
        
        if (dto.Name.Length > 100)
        {
            throw new Exception("Offer name is too long!");
        }

        if (dto.Quantity <= 0)
        {
            throw new Exception("Quantity must be bigger than 0!");
        }

        if (dto.Price <= 0)
        {
            throw new Exception("Price must be bigger than 0!");
        }
        
        // get farm by name from dao and check if it exists
        var farm = await farmDao.GetByName(dto.FarmName);
        
        if (farm == null)
        {
            throw new Exception($"Farm {dto.FarmName} does not exist!");
        }
        
    }
}