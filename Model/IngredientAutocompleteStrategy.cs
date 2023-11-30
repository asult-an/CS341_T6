using CookNook.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    public class IngredientAutocompleteStrategy : IAutocompleteStrategy<Ingredient>
    {
        // private readonly IIngredientLogic ingredientLogic;
        private IEnumerable<Ingredient> cachedIngredients;
        private ObservableCollection<Ingredient> suggestedIngredients;

        public ObservableCollection<Ingredient> SuggestedIngredients
        {
            get { return suggestedIngredients; }
            set { SuggestedIngredients = value; }
        }


        public IngredientAutocompleteStrategy(IEnumerable<Ingredient> ingredients)
        {
            // this.ingredientLogic = ingredientLogic;
            this.cachedIngredients = ingredients;
        }

        /// <summary>
        /// Provides a list of suggested results to hasten typed input, using the most suitable 
        /// algorithm based on the size of the input string supplied by the user
        /// </summary>
        /// <param name="input">input string to query the recipes for</param>
        /// <returns></returns>
        public async Task<IEnumerable<Ingredient>> GetSuggestionsAsync(string input)
        {
            // store latest data without any database calls here

            // to make future revisions easier, just use a switch statement for algorithm selection
            switch (input.Length)
            {
                // if the input is small enough to do a surface-level search
                case var n when n <= 3:
                    return await GetSuggestionsByFirstThreeLetters(input);
                    break;
                // if input is too long for most basic search, but not long enough for a more intensive one
                case var n when (n > 3 && n <= 10):
                    return await GetSuggestionsByModestString(input);
                    break;
                // if input is long enough to warrant a more intensive search
                default:
                    return await GetSuggestionsByString(input);
            }
        }


        /// <summary>
        /// When the input is small enough, we can just do a surface-level search: this function
        /// performs a minimally intensive search that's sufficient to return an ideal list
        /// of expected recipes matching the input
        /// </summary>
        /// <param name="input">search query</param>
        /// <returns>Task-wrapped IEnumerable of the suggestions</returns>
        private async Task<List<Ingredient>> GetSuggestionsByFirstThreeLetters(string input)
        {
            // TODO: We'll probably want a Tuple of the <name, Id> for the DataTemplate to use
            var results = new List<Ingredient>();

            // TODO: evaluate efficiency
            
            // perform the search against the collection
            // return a task that's getting the resulting suggestions from the cachedIngredients
            return await Task.FromResult(cachedIngredients.Where(i => i.Name.StartsWith(input)).ToList<Ingredient>());
        }

        /// <summary>
        /// Next step up from the surface-level search, this function utilizes a more efficient sorting
        /// algorithm to return a more accurate list of expected recipes matching the input
        /// </summary>
        /// <returns>Task-wrapped IEnumerable of the suggestions</returns>
        private async Task<IEnumerable<Ingredient>> GetSuggestionsByModestString(string input)
        {
            var results = new List<Ingredient>();

            // until we come up with a better algorithm, just call the standard search autocompleteStrategy
            return await GetSuggestionsByString(input);
        }


        /// <summary>
        /// Standard search algorithm for a string input, when we have little to no benefit by using simpler searches
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<IEnumerable<Ingredient>> GetSuggestionsByString(string input)
        {
            var results = new List<Ingredient>();

            // TODO: evaluate efficiency
            return await Task.FromResult(cachedIngredients
                .Where(i => i.Name.Contains(input)).ToList<Ingredient>());
        }
    }
}
