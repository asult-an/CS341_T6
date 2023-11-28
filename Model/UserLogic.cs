using CookNook.Services;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookNook.Model.Interfaces;

//namespace CookNook.Services
namespace CookNook.Model
{

    public class UserLogic
    {
        Random random = new Random();

        // place for the injected datbase instance to load into
        private readonly IUserDatabase userDatabase;

        // TODO: check to see if we can justify keeping this, or if userDatabase can be utilized
        //private UserDatabase userDatabase = new UserDatabase();

        // since users can interact with recipies, inject RecipeLogic
        // may not use it now, but by doing this we can send recipe data to users
        // e.g notify user of followed recipe...?
        private readonly IRecipeLogic recipeLogic;

        // Unlike Lab3, actually implement Dependency Injection from DB to BL

        /// <summary>
        /// TEMPORARY USE ONLY:
        /// RecipeLogic needed to handle Recipe
        /// Creates a new UserService object with an instance of the database loaded
        /// via DI.  
        /// </summary>
        /// <param name="userDatabase"></param>
        [Obsolete]
        public UserLogic(IUserDatabase userDatabase)
        {
            this.userDatabase = userDatabase;
        }
        public UserLogic() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDatabase"></param>
        /// <param name="recipeLogic"></param>
        public UserLogic(IUserDatabase userDatabase, IRecipeLogic recipeLogic)
        {
            this.userDatabase = userDatabase;
            this.recipeLogic = recipeLogic;
        }
        public UserAuthenticationError AuthenticateUser(string username, string password)
        {
           return userDatabase.AuthenticateUser(username, password);
        }

        public UserAdditionError RegisterNewUser(string username, string email, string password, string confirmPassword)
        {
            //TODO: add email confirmation
            if(!ValidateSignupInput(username, email, password, confirmPassword))
            {
                return UserAdditionError.InvalidPassword;
            }
            // Note:  we want the database to come up with Id, since we're wasting computation if the 
            // insert is going to fail
            User newUser = new User((Int64)random.NextInt64(5000), username, email, password);
            try
            {
                UserAdditionError result = userDatabase.InsertUser(newUser);
                if (result != UserAdditionError.NoError) 
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return UserAdditionError.DBAdditionError;
            }
            return UserAdditionError.NoError;
        }
        private bool ValidateSignupInput(string username, string email, string password, string confirmPassword)
        {

            if(password != confirmPassword)
            {
                return false;
            }
            //TODO: Add profanity filter
            //TODO: add special character invalidator
            return true;
        }

        /// <summary>
        /// Attempt to follow a recipe if not already following.  If
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        public UserSelectionError FollowRecipe(Int64 userId, Int64 recipeId)
        {
            /* if recipeLogic injected:
             * check if recipeID is valid
             */

            // check if the recipe is already followed
            return userDatabase.IsFollowingRecipeById(userId, recipeId);

        }

        public List<User> GetUsersByID(List<Int64> userIDs)
        {
            return userDatabase.GetUsersById(userIDs);
        }

        public User GetUserByEmail(string email)
        {
            // verify that the email matches the format of an email address


            return userDatabase.GetUserByEmail(email);
        }

        public User GetUserById(Int64 id)
        {

            return userDatabase.GetUserById(id);
        }

        public User GetUserByUsername(string username)
        {
            
            return userDatabase.GetUserByUsername(username);
        }

        public UserAdditionError InsertUser(User inUser)
        {
            return userDatabase.InsertUser(inUser);
        }

        //public UserEditError UpdateUserInfo(Int64 id, string username, string password, string imgPath)
        //{
        //    return userDatabase.UpdateUserInfo(id, username, password, imgPath);

        //}

        //public UserEditError UpdateFollowedUser(string followedId, string followerId)
        //{

        //}

        //public UserSelectionError GetFollowedUsers(int userId)
        //{

        //}

        public UserDeletionError DeleteUser(User inUser)
        {
            return userDatabase.DeleteUser(inUser);
        }
    }
}
