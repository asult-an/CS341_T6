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
        public List<DietPreference> GetPreferencesFor(long userId);


        /// <summary>
        /// Overwrites all preferences for a user in bulk: this way PreferenceProvider can 
        /// check against the locally stored settings to trim out data from the proposed 
        /// query that would otherwise be redundant.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="preferences"></param>
        /// <returns></returns>
        public bool UpdatePreferencesFor(long userId, List<DietPreference> preferences);

        /// <summary>
        /// Wipes all of the stored preferences for a user in the user_settings table
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeletePreferencesFor(long userId);
    }
}
