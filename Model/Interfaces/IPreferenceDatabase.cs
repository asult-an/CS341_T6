using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model.Interfaces
{
    /// <summary>
    /// Handles database operations involved with user's dietary preferences.
    /// Tables involved:
    /// preference_recipes: Links preferences to recipes
    /// user_settings: Defines user preferences, including dietary restrictions
    /// </summary>
    public interface IPreferenceDatabase
    {
        /// <summary>
        /// Used to grab all of the latest settings from both ingredients and recipes
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<DietPreference>> GetPreferencesForUserAsync(long userId);
        
        /// <summary>
        /// Overwrites all preferences for a user in bulk: this way PreferenceProvider can 
        /// check against the locally stored settings to trim out data from the proposed 
        /// query that would otherwise be redundant.
        /// </summary>
        /// <param name="userId">the user who's updating their preferences</param>
        /// <param name="preferences"></param>
        /// <returns></returns>
        public bool UpdatePreferencesFor(long userId, List<DietPreference> preferences);


        /// <summary>
        /// Retrieves the ingredients affected by a particular user's preference
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="prefId"></param>
        /// <returns></returns>
        // public List<DietAffectedIngredient> GetPrefAffectedIngredients(long userId, long prefId);


        /// <summary>
        /// Retrieves the recipes affected by a particular user's preference
        /// </summary>
        /// <param name="userId">id of which user's preferences we want</param>
        /// <returns></returns>
        // public List<DietAffectedRecipe> GetGetPrefAffectedRecipes(long userId);

        /// <summary>
        /// Retrieves all ingredients that are affected by the user's preferences
        /// </summary>
        /// <param name="userId">id of which user's preferences we want</param>
        /// <returns></returns>
        public List<DietAffectedIngredient> GetAffectedIngredients(long userId);


        /// <summary>
        /// Retrieves all recipes that are affected by the user's preferences
        /// </summary>
        /// <param name="userId">id of which user's preferences we want</param>
        /// <returns></returns>
        public List<DietAffectedRecipe> GetAffectedRecipes(long userId, long prefId);


        /// <summary>
        /// Wipes all of the stored preferences for a user in the user_settings table
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteAllPreferencesFor(long userId);
    }
}
