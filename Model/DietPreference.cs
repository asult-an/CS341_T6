using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    
     
    /// <summary>
    /// The database has to use junction tables to maintain any user's preferences that affect 
    /// more than a single Recipe or Ingredient, from possibly multiple tables in worst case scenarios.
    /// The DietPreference model help facilitate aggregation of cases where multiple ingredients/recipes are 
    /// tied to a single DietPreference's ID (diet_id).   
    /// 
    /// Dietary Preferences will have a title that the user enters for easy identification.  This alond with 
    /// the affected ingredients and their types 
    /// </summary>
    public class DietPreference
    {
        // Question: we might be able to be using <T> since recipes AND ingredients can be affected...?

        // storing this much data might get ugly, so let's only store the affected entity's Id 
        private List<DietAffectedRecipe> affectedRecipes;
        public List<DietAffectedRecipe> AffectedRecipes
        {
            get { return affectedRecipes; }
            private set => AffectedRecipes = value;
        }
        
        
        private List<DietAffectedIngredient> affectedIngredients;


        /// <summary>
        /// Ingredients that are affected are stored in this list, and can be 
        /// retrieve
        /// </summary>
        public List<DietAffectedIngredient> AffectedIngredients
        {
            get { return affectedIngredients; }
            private set => AffectedIngredients = value;
        }

        private long dietPrefId;

        /// <summary>
        /// the primary key for a dietary preference
        /// </summary>
        public long DietPrefId
        {
            get { return dietPrefId; }
            set { dietPrefId = value; }
        }

        private string title;

        /// <summary>
        /// The identifying name of the title
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private long ownerId;

        /// <summary>
        /// The UserID belonging to the owner
        /// </summary>
        public long OwnerId
        {
            get { return ownerId; }
            set { ownerId = value; }
        }


        /// <summary>
        /// Checks against the stored ingredient Ids to see what kind of 
        /// preference exists (if any) on a given ingredient
        /// </summary>
        /// <param name="ingredientId"></param>
        /// <returns></returns>
        public bool GetPreferenceOfIngredient(long ingredientId)
        {
            // check if the ingredient is listed in those affected   
            if (AffectedIngredients.Any(i => i.IngredientId == ingredientId))
            {
                var targetEntry = AffectedIngredients.FirstOrDefault(i => i.IngredientId == ingredientId);
                // the true/false is stored in the second tuple value
                return targetEntry.IsPreferred;
            }
            // otherwise, the ingredient wasn't found
            Debug.WriteLine("[TBD] The selected ingredient could not be found!");

            return false;
        }

        
        /// <summary>
        /// Checks against the stored ingredient Ids to see what kind of 
        /// preference exists (if any) on a given ingredient
        /// </summary>
        /// <param name="recipeId">the id of the recipe to check</param>
        /// <returns></returns>
        public bool GetPreferenceOfRecipe(long recipeId)
        {
            // check if the recipe is listed in those affected   
            if (AffectedRecipes.Any(r => r.RecipeId == recipeId))
            {
                var targetEntry = AffectedRecipes.FirstOrDefault(r => r.RecipeId == recipeId);
                
                // the true/false is stored in the second tuple value
                return targetEntry.IsPreferred;
            }
            // otherwise, the recipe wasn't found
            Debug.WriteLine("[TBD] The selected recipe could not be found!");
            return false;
        }

        /**
         * To keep track of whether or not an affected Ingredient or Recipe is *positively* or *negatively*
         * associated... we could either use a Tuple, or just make it a property... hm....
         */
        /// <summary>
        /// Creates a new DietPreference object, from two Collections of Tuples 
        /// detailing the affected entities.
        /// 
        /// This constructor is specially used for when the Ids from the Recipe and Ingredients 
        /// have been isolated so that any other data is dropped
        /// </summary>
        /// <param name="affectedRecipes"></param>
        /// <param name="affectedIngredients">collection of </param>        
        public DietPreference(
            List<DietAffectedRecipe> affectedRecipes,
            List<DietAffectedIngredient> affectedIngredients)
        {
            this.affectedRecipes = affectedRecipes;
            this.affectedIngredients = affectedIngredients;
        }

        /// <summary>
        /// Initializes a new empty DietPreference model, ready to have
        /// ingredients and recipes added to it
        /// </summary>
        public DietPreference() { 
            // initialize as empty lists, without data to reference
            affectedRecipes = new List<DietAffectedRecipe>();
            affectedIngredients = new List<DietAffectedIngredient>();
            this.Title = "";
        }


        /// <summary>
        /// This constructor is more prepped for everywhere else in the app. 
        /// Accepts collections of Recipe and Ingredient models, then reads 
        /// all of the ids of the affected entities before storing them in 
        /// the appropriate field
        /// </summary>
        /// <param name="title"></param>
        /// <param name="recipes"></param>
        /// <param name="ingredient"></param>
        public DietPreference(string title, List<Recipe> recipes, List<Ingredient> ingredient)
        {
            // filter the recipeId off of recipes

            // filter the ingredientId off of ingredients

            // by default, all of the preferences can be false, since
            // this constructor is likely only used when creating a new preference
            // that will immediately be modified by the user 
            
        }

        /// <summary>
        /// Standard constructor for a DietPreference, accepting the AffectedFoo model 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="recipes"></param>
        /// <param name="ingredients"></param>
        public DietPreference(string title, List<DietAffectedRecipe> recipes, List<DietAffectedIngredient> ingredients)
        {
            this.title = title;
            this.affectedRecipes = recipes;
            this.affectedIngredients = ingredients;
        }

        /// <summary>
        /// Attempts to remove the selected ingredient from a provided preference's AffectedIngredients
        /// </summary>
        /// <param name="ingredientId">Ingredient's ID</param>
        /// <param name="preference">the preference we're manipulating</param>
        /// <returns></returns>
        public bool RemoveAssociatedIngredient(long ingredientId, DietPreference preference)
        {
            DietAffectedIngredient targetEntry = null;

            // if the ingredient is present in AffectedRecipes...
            if (preference.AffectedIngredients.Any(i => i.IngredientId == ingredientId))
            {
                targetEntry = preference.AffectedIngredients.FirstOrDefault(i => i.IngredientId == ingredientId);
            }

            if (targetEntry == null)
            {
                Debug.WriteLine("[DietPreference] The selected ingredient could not be found!");
                return false;
            }

            // remove it, returning the resulting bool
            return preference.AffectedIngredients.Remove(targetEntry);
        }

        /// <summary>
        /// Attempts to remove a given recipe from a provided preference's AffectedRecipes
        /// </summary>
        /// <param name="recipeId">the id of the recipe to remove</param>
        /// <param name="preference">the preference we're manipulating</param>
        /// <returns></returns>
        public bool RemoveAssociatedRecipe(long recipeId, DietPreference preference)
        {
            DietAffectedRecipe targetEntry = null;

            if (preference.AffectedRecipes.Any(i => (i.RecipeId == recipeId)))
                targetEntry = preference.AffectedRecipes.FirstOrDefault(i => i.RecipeId == recipeId);

            if (targetEntry == null)
            {
                Debug.WriteLine("[DietPreference] The selected recipe could not be found on the preference!");
                // we could additionally log the preference?
                return false;
            }

            return preference.AffectedRecipes.Remove(targetEntry);
        }

    }
}
