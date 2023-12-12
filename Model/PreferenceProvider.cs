using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookNook.Model.Interfaces;
using Newtonsoft.Json;

namespace CookNook.Model
{
    
    /// <summary>
    /// PreferenceProvider is a service that interacts with DietPreferenceStorage to yield tuples 
    /// of user and preference ids (Since 'user_preference' has cols ['user_id','pref_id'])
    /// </summary>
    class PreferenceProvider : IPreferenceProvider
    {
        /// <summary>
        /// the class directly responsible for the JSON storage
        /// </summary>
        private readonly DietPreferenceStorage localStorage;

        public PreferenceProvider()
        {
            localStorage = new DietPreferenceStorage();
        }


        /// <summary>
        /// Appends a singular new preference to the json file
        /// </summary>
        /// <param name="preference"></param>
        /// <returns></returns>
        public bool WritePreferenceJSON(DietPreference preference)
        {
            try
            {
                var preferences = Task.Run(async () => await localStorage.LoadPreferencesAsync()).Result;

                // append new data
                preferences.Append(preference); 

                // save the updated collection
                Task.Run(async () => await localStorage.SavePreferencesAsync(preferences)).Wait();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"[PreferenceProvider] Error while writing JSON: {e}");
                return false;
            }
        }

        public bool OverwritePreferencesJSON(List<DietPreference> preferences)
        {
            try
            {
                foreach (var preference in preferences)
                {
                    WritePreferenceJSON(preference);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<string> ConvertPrefToJSON(DietPreference preference)
        {
            try
            {
                var json = JsonConvert.SerializeObject(preference);
                // cast the json to a string
                return new List<string> { json };
            }
            catch (Exception e)
            {
                Debug.WriteLine($"[PreferenceProvider] Error during Pref -> JSON conv: {e}");
                throw;
            }
        }

        public async Task<List<DietPreference>> UpdateLocalSettingsAsync()
        {
            var userId = UserViewModel.Instance.AppUser.Id;
            // fetch the latest settings through the database service
            var latest = MauiProgram.ServiceProvider.GetService<IPreferenceDatabase>()
                                    .GetPreferencesForUserAsync(userId);
            
            OverwritePreferencesJSON(latest.Result);
            return await localStorage.LoadPreferencesAsync();
        }

        /// <summary>
        /// Translates the contents of user_preferences.json into a List of DietPreference objects
        /// and returns that list.
        /// </summary>
        /// <returns></returns>
        public async Task<List<DietPreference>> GetLocalPreferencesAsync()
        {
            return await localStorage.LoadPreferencesAsync();
        }

        //public Recipe ResolveRecipeFromPreference(DietAffectedRecipe recipe)
        //{
        //    MauiProgram.ServiceProvider.GetService<I>
        //    return TODO_IMPLEMENT_ME;
        //}
    }
}
