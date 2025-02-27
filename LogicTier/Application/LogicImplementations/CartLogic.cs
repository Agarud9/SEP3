﻿using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Shared.DTOs;
using Shared.Models;

namespace Application.LogicImplementations;

public class CartLogic : ICartLogic
{
    private ICartDao cartDao;
    private IAuthDao authDao;
    private IOfferDao offerDao;

    public CartLogic(ICartDao cartDao, IAuthDao authDao, IOfferDao offerDao)
    {
        this.cartDao = cartDao;
        this.authDao = authDao;
        this.offerDao = offerDao;
    } 
        
    public async Task AddToCartAsync(CartOfferDto dto)
    {
        
        if (dto.Quantity <= 0)
        {
            throw new Exception("The quantity should be bigger than 0");
        }

        if (dto.Quantity >= 100)
        {
            throw new Exception("The quantity has to be smaller than 250");
        }

        /*  if (!dto.CollectOption.ToLower().Equals("Delivery".ToLower()) ||
              !dto.CollectOption.ToLower().Equals("Pick Up".ToLower()) ||
              !dto.CollectOption.ToLower().Equals("Pick Your Own".ToLower()))
          {
              throw new Exception("Invalid collection option for the cart offer");
          }
          */
        var offer = await offerDao.GetOfferByIdAsync(dto.OfferId);

        var cartOffer = new CartOffer
        {
            Offer = offer,
            Quantity = dto.Quantity,
            UserName = dto.Username,
            CollectionOption = dto.CollectionOption
        };
        
        await cartDao.AddToCartAsync(cartOffer);
    }
    

    public async Task<IEnumerable<CartOffer>> GetAllCartItemsAsync(string username)
    {
        if (authDao.GetUserAsync(username).Equals(null))
        {
            throw new Exception("Invalid Username. This user does not exist");
        }

        IEnumerable<CartOffer> cartOffers = await cartDao.GetAllCartItemsAsync(username);
        return cartOffers;
    }

    public async Task DeleteAllCartOffersAsync(string username)
    {
        if (authDao.GetUserAsync(username).Equals(null))
        {
            throw new Exception("Invalid Username. This user does not exist");
        }

        await cartDao.DeleteAllCartOffersAsync(username);
    }

    public async Task DeleteCartOfferAsync(int id)
    {
        CartOffer? existing = await cartDao.GetByIdAsync(id);

        if (existing == null)
        {
            throw new Exception($"The cart item with {id} does now exist");
        }

        await cartDao.DeleteCartOfferAsync(id);
    }

    public async Task<CartOffer> GetCartOfferById(int id)
    {
        CartOffer? existing = await cartDao.GetByIdAsync(id);

        if (existing == null)
        {
            throw new Exception($"The cart item with {id} does now exist");
        }

        return existing;
    }

    public async Task UpdateAsync(UpdateCartOfferDto dto)
    {
        await cartDao.UpdateAsync(dto);
    }
}