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
        private RecipeDatabase recipeDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecipeLogic"/> class, with the database service injected
        /// </summary>
        /// <param name="recipeDatabase">The service for recipe-based database interactions</param>
        public RecipeLogic(RecipeDatabase recipeDatabase)
        {
            this.recipeDatabase = recipeDatabase;
        }

       // this method may be redundant
       public RecipeAdditionError CreateRecipe(string inName, string inDescription, int inAuthorId,
            string inIngredients, string inIngredientsQty,
            int inCooktime, string inCourse, int inRating, int inServings, string inImage,
            string inTags, string inFollowers)
        {



            // validate name
            if (string.IsNullOrEmpty(inName) || inName.Length > MAX_RECIPE_NAME_LENGTH)
                return RecipeAdditionError.InvalidName;

            // Validate description
            if (inDescription.Split(' ').Length > MAX_RECIPE_DESCRIPTION_LENGTH)
                return RecipeAdditionError.InvalidDescription;




            // Q: how do we resolve Id back from the Database (since it's returning a RecipeError) if it's automatically generated
            // If all validations pass, construct the Recipe object
            Recipe newRecipe = new Recipe
            {
                AuthorID = inAuthorId,
                CookTime = inCooktime,
                Course = CourseType.Parse(inCourse),
                Description = inDescription,
                Followers = inFollowers,
                Image = inImage,
            };
           
            return AddRecipe(newRecipe);
        }


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
            return RecipeAdditionError.NoError;
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

        public Recipe FindRecipe(int id)
        {
            try
            {
                return recipeDatabase.SelectRecipe(id);

            }
            catch
            {
                Console.WriteLine("Error finding recipe");
                return null;
            }
        }

        public List<Recipe>? SelectAllRecipes() 
        {
            try
            {
                List<int> allRecipeIds = recipeDatabase.GetAllRecipeIds();
                return recipeDatabase.SelectAllRecipes(allRecipeIds);
            }
            catch (Exception ex)
            {
                throw new Exception("Error selecting all recipes", ex);

                // why are we adding a reicpe in a catch block
                //Debug.WriteLine(ex.Message);
                //string ingredients = "";
                //string ingredientsQty = "";
                //string tags = "";
                //string followers = "";
                //Recipe failRecipe = new Recipe(56, "The First Recipe!","This is the first recipe inserted into the CookNook database!", 0, ingredients, ingredientsQty, 60, "Dinner", 50, 6, "image_ref", tags, followers);
                //List<Recipe> testList = new List<Recipe>();
                //testList.Add(failRecipe);
                //return testList;
            }
        }
    }
}

