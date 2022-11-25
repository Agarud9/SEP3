package mango.sep3.databaseaccess.DAOInterfaces;

import mango.sep3.databaseaccess.Shared.User;
import mango.sep3.databaseaccess.Shared.UserAuth;

/**
 * Data access object accessing user related data.
 */
public interface UserDaoInterface {
    /**
     * Registers a user.
     * @param userAuth The user to register.
     * @return The registered user.
     */
    User registerUser(UserAuth userAuth);

    /**
     * Checks for user in the database. If Exists -> Logs the user in
     * @param userAuth The user to login.
     * @return The logged-in user.
     */
    User loginUser(UserAuth userAuth);

    /**
     * Gets a user by username.
     * @param username The username of the user.
     * @return The user.
     */
    User getUserByUsername(String username);

    UserAuth getUserAuthByUsername(String username);


}
