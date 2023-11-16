using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    internal class RecipeLogic : IRecipeLogic
    {
        const int MAX_RECIPE_NAME_LENGTH = 50;
        const int MAX_RECIPE_DESCRIPTION_LENGTH = 150;

        private IRecipeDatabase recipeDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecipeLogic"/> class, with the database service injected
        /// </summary>
        /// <param name="recipeDatabase">The service for recipe-based database interactions</param>
        public RecipeLogic(IRecipeDatabase recipeDatabase)
        {
            this.recipeDatabase = recipeDatabase;
        }


       // this method may be redundant
       public RecipeAdditionError CreateRecipe(string inName, string inDescription, int inAuthorId,
           string inIngredients, string inIngredientsQty,
           int inCooktime, string inCourse, int inRating, int inServings, string inImage,
           string inTags, string inFollowers)

       {

            //TODO: ingredients will become Ingredient[] instead of a spring 


            // validate name
            if (string.IsNullOrEmpty(inName) || inName.Length > MAX_RECIPE_NAME_LENGTH)
                return RecipeAdditionError.InvalidName;

            // Validate description
            if (inDescription.Split(' ').Length > MAX_RECIPE_DESCRIPTION_LENGTH)
                return RecipeAdditionError.InvalidDescription;

            // Q: how do we resolve Id back from the Database (since it's returning a RecipeError) if it's automatically generated
            // A: The returned object from the query may have something to answer this, otherwise we could do a SELECT FROM RECIPES WHERE...
            
            // If all validations pass, construct the Recipe object
            Recipe newRecipe = new Recipe
            {
                AuthorID = inAuthorId,
                CookTime = inCooktime,
                Course = CourseType.Parse(inCourse),
                Description = inDescription,
                // new recipe will have no followers
                //FollowerIds = recipeDatabase.GetIngredientsByRecipe(),
                
                Image = Encoding.ASCII.GetBytes(inImage),
            }; 

            return AddRecipe(newRecipe);
        }


       public RecipeAdditionError CreateRecipe(int inId, string inName, string inDescription, int inAuthorID, string inIngredients,
           string inIngredientsQty, int inCooktime, string inCourse, int inRating, int inServings, string inImage,
           string inTags, string inFollowers)
       {
           return RecipeAdditionError.NoError;
       }

       /// <summary>
        /// Adds a new recipe to the database, with no followers.
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns>Appropriated error if the add failed</returns>
       public RecipeAdditionError AddRecipe(Recipe recipe)
        {
            // check for duplicate Id
            if (FindRecipe(recipe.ID) != null)
                return RecipeAdditionError.DuplicateId;

            try
            {
                recipeDatabase.InsertRecipe(recipe);
                return RecipeAdditionError.NoError;
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.ToString());
                // This is where adding a recipe is failing, its catching some exception 
                // in the database, and I'm not sure what the issue is.

                return RecipeAdditionError.DBAdditionError;
            }
        }

        public RecipeEditError EditRecipe(Recipe recipe)
        {
            if (FindRecipe(recipe.ID) == null)
                return RecipeEditError.RecipeNotFound;

            try
            {
                recipeDatabase.EditRecipe(recipe);
                return RecipeEditError.NoError;
            }
            catch (Exception ex)
            { 
                return RecipeEditError.DBEditError;
            }
        }

        public RecipeDeletionError DeleteRecipe(Recipe recipe)
        {
            try
            {

                recipeDatabase.DeleteRecipe(recipe.ID);
                return RecipeDeletionError.NoError;

            }
            catch (Exception ex)
            {
                return RecipeDeletionError.DBDeletionError;
            }
        }

        public List<Recipe> SelectRecipeByCooktime(int cooktime)
        {
            // if cooktime is invalid...

            return recipeDatabase.SelectRecipeByCooktime(cooktime);
        }

        public List<Recipe> SelectRecipes(List<int> recipeList)
        {
            return recipeDatabase.SelectRecipes(recipeList);
        }

        public Recipe FindRecipe(int id)
        {
            try
            {
                return recipeDatabase.SelectRecipe(id);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error finding recipe: " + ex);
                return null;
            }
        }

        public List<int> GetFollowerIds(int recipeID)
        {
            return recipeDatabase.GetRecipeFollowerIds(recipeID);
            // throw new NotImplementedException();

        }

        public List<Ingredient> GetIngredientsByRecipe(int recipeID)
        {
            return recipeDatabase.GetIngredientsByRecipe(recipeID);
            // throw new NotImplementedException();
        }

        public List<Ingredient> GetAllIngredients()
        {
            return recipeDatabase.GetAllIngredients();
        }

        public Ingredient GetOrCreateIngredient(string ingredientName)
        {
            // if the ingredient already exists...
            return recipeDatabase.GetOrCreateIngredient(ingredientName);
        }

        public List<Tag> GetTagsForRecipe(int recipeID)
        {
            // TODO: might need to return error rather than propogate the Tag up
            // if recipeID is not valid:
            return recipeDatabase.GetTagsForRecipe(recipeID);
            // throw new NotImplementedException();
        }

        public List<Recipe>? SelectAllRecipes() 
        {
            try
            {
                // get by user...?

                //List<int> allRecipeIds = recipeDatabase.GetAllRecipeIds();
                //return recipeDatabase.SelectRecipes(allRecipeIds);
                return recipeDatabase.SelectAllRecipes();
            }
            catch (Exception ex)
            {
                throw new Exception("Error selecting all recipes", ex);
            }
        }

    }
}

