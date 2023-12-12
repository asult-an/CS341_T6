using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CookNook.Model;

namespace CookNook
{

    /// <summary>
    /// This class manages the operations involved with storing the user's dietary preferences in JSON.
    /// This storage consists of preferences *only* for the user who last logged in, and is created once they log in.
    /// 
    /// Should update on home screen and log in
    /// </summary>
    public class DietPreferenceStorage
    {
        private readonly string filePath;

        public DietPreferenceStorage()
        {
            // Define the file path for storing preferences. 
            // This should be in a user-specific directory to ensure privacy and separation of data.
            filePath = Path.Combine(Environment.GetFolderPath(
                                    Environment.SpecialFolder.LocalApplicationData), "user_preferences.json");
            
            // create if not found
            if (!File.Exists(filePath))
            {
                var file = File.Create(filePath);
                // TODO: check if user is logged in
                // if they are, retreive their preferences from Db

                // if not, then default to a new DietPreference Object
                DietPreference myPreference = new DietPreference();

                SavePreferencesAsync(myPreference).Wait();
            }
            // create the file if it doens't exist
        }

        // TODO: method for resolving multiple preferences, similar to above but accepting a List of prefernces

        /// <summary>
        /// Saves the dietary preferences to a local file.
        /// </summary>
        /// <param name="preferences">The preferences to save</param>
        public async Task SavePreferencesAsync(DietPreference preferences)
        {
            var json = JsonConvert.SerializeObject(preferences);
            await File.WriteAllTextAsync(filePath, json);
        }

        /// <summary>
        /// Loads the dietary preferences from the local file.
        /// </summary>
        /// <returns>The loaded preferences</returns>
        public async Task<DietPreference> LoadPreferencesAsync()
        {
            if (!File.Exists(filePath))
            {
                // return a new instance if the file doesn't exist.
                return new DietPreference(); 
            }

            var json = await File.ReadAllTextAsync(filePath);
            return JsonConvert.DeserializeObject<DietPreference>(json);
        }

    }
}
