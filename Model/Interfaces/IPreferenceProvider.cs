﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model.Interfaces
{
    /// <summary>
    /// Handles the provisions of locally-stored user settings to the app so that the 
    /// Database side has as little to do with it as possible.
    /// </summary>
    public interface IPreferenceProvider
    {
        /// <summary>
        /// Appends a new preference to the local storage of preferences
        /// </summary>
        /// <param name="preference"></param>
        /// <returns></returns>
        public bool WritePreferenceJSON(DietPreference preference);


        public bool OverwritePreferencesJSON(List<DietPreference> preferences);

        /// <summary>
        /// Takes a preference and converts it into a JSON string
        /// </summary>
        /// <param name="preference"></param>
        /// <returns></returns>
        public List<string> ConvertPrefToJSON(DietPreference preference);


        /// <summary>
        /// Checks the local settings and compares them to a fresh call to the database for 
        /// user's preferences.  If there's any mismatches, then we'll save over the existing
        /// json with a call to WritePreferences
        /// </summary>
        /// <returns></returns>
        public Task<List<DietPreference>> UpdateLocalSettingsAsync();

        /// <summary>
        /// Reads the contents of the locally stored settings and builds them back into 
        /// a DietPreference collection, then returns that collection.
        /// </summary>
        /// <returns></returns>
        public Task<List<DietPreference>> GetLocalPreferencesAsync();
    }
}