package mango.sep3.databaseaccess.DAOImplementations;

import mango.sep3.databaseaccess.DAOInterfaces.OfferDaoInterface;
import org.springframework.stereotype.Repository;

import java.util.Collection;

import mango.sep3.databaseaccess.DAOInterfaces.OfferDaoInterface;
import mango.sep3.databaseaccess.Repositories.OfferRepository;
import mango.sep3.databaseaccess.Shared.Offer;
import org.springframework.stereotype.Repository;

import java.util.Collection;

@Repository
public class OfferDAO implements OfferDaoInterface {
  private OfferRepository offerRepository;

  public OfferDAO(OfferRepository offerRepository) {
    this.offerRepository = offerRepository;
  }



  @Override
  public mango.sep3.databaseaccess.Shared.Offer CreateOffer(Offer offer) {
    return offerRepository.save(offer);
  }

  @Override
  public Collection<Offer> GetOffers() {
    return offerRepository.findAll();
  }

  @Override public Offer getOfferById(int id)
  {
    return offerRepository.getReferenceById(id);
  }
}