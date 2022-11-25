package mango.sep3.databaseaccess.Repositories;

import mango.sep3.databaseaccess.Shared.OrderOffer;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Collection;
import java.util.List;

@Repository
public interface OrderOfferRepository extends JpaRepository<OrderOffer, Integer>
{
  Collection<OrderOffer> findAllByUsername(String username);
}
