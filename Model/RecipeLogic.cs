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
        private RecipeDatabase recipeDatabase;

        public RecipeLogic()
        {
            recipeDatabase = new RecipeDatabase();
        }

       // this method may be redundant
       public RecipeAdditionError CreateRecipe(int inId, string inName, string inDescription, int inAuthorID,
            string inIngredients, string inIngredientsQty,
            int inCooktime, string inCourse, int inRating, int inServings, string inImage,
            string inTags, string inFollowers)
        {
            if (string.IsNullOrEmpty(inName))
                return RecipeAdditionError.InvalidName;

            // Validate description is not too long
            if (inDescription.Split(' ').Length > 150)
                return RecipeAdditionError.InvalidDescription;

            
            

            // If all validations pass, construct the Recipe object
            Recipe newRecipe = new Recipe(inId, inName, inDescription, inAuthorID, inIngredients,
                inIngredientsQty, inCooktime, inCourse, inRating, inServings, inImage, inTags, inFollowers);

           
            return AddRecipe(newRecipe);
        }


        public RecipeAdditionError AddRecipe(Recipe recipe)
        {
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
                return null;
            }
        }

        public ObservableCollection<Recipe> SelectAllRecipes() 
        {
            try
            {
                List<int> allRecipeIds = recipeDatabase.GetAllRecipeIds();
                return recipeDatabase.SelectAllRecipes(allRecipeIds);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                string ingredients = "";
                string ingredientsQty = "";
                string tags = "";
                string followers = "";
                Recipe failRecipe = new Recipe(56, "The First Recipe!", "This is the first recipe inserted into the CookNook database!", 0, ingredients, ingredientsQty, 60, "Dinner", 50, 6, "image_ref", tags, followers);
                ObservableCollection<Recipe> testList = new ObservableCollection<Recipe>();
                testList.Add(failRecipe);
                return testList;
            }
        }
    }
}

