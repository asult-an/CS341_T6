using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CookNook.Model;

namespace CookNook
{
    public class DietPreferenceStorage
    {
        private readonly string filePath;

        public DietPreferenceStorage()
        {
            // Define the file path for storing preferences. 
            // This should be in a user-specific directory to ensure privacy and separation of data.
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "user_preferences.json");
        }

        /// <summary>
        /// Saves the dietary preferences to a local file.
        /// </summary>
        /// <param name="preferences">The preferences to save.</param>
        public async Task SavePreferencesAsync(DietPreference preferences)
        {
            var json = JsonConvert.SerializeObject(preferences);
            await File.WriteAllTextAsync(filePath, json);
        }

        /// <summary>
        /// Loads the dietary preferences from the local file.
        /// </summary>
        /// <returns>The loaded preferences.</returns>
        public async Task<DietPreference> LoadPreferencesAsync()
        {
            if (!File.Exists(filePath))
            {
                return new DietPreference(); // Return a new instance if the file doesn't exist.
            }

            var json = await File.ReadAllTextAsync(filePath);
            return JsonConvert.DeserializeObject<DietPreference>(json);
        }

        // Additional methods for managing the local storage can be added as needed.
    }
}
