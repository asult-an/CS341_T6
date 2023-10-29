using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
       public RecipeAdditionError CreateRecipe(int inId, string inName, string inDescription, string inAuthor,
            ObservableCollection<string> inIngredients, ObservableCollection<string> inIngredientsQty,
            int inCooktime, string inCourse, int inRating, int inServings, string inImage,
            String inTags, String inFollowers)
        {
            if (string.IsNullOrEmpty(inName))
                return RecipeAdditionError.InvalidName;

            // Validate description is not too long
            if (inDescription.Split(' ').Length > 150)
                return RecipeAdditionError.InvalidDescription;

            
            

            // If all validations pass, construct the Recipe object
            Recipe newRecipe = new Recipe(inId, inName, inDescription, inAuthor, inIngredients,
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
<<<<<<< HEAD
                recipeDatabase.DeleteRecipe(recipe.ID);
                return RecipeDeletionError.NoError;
=======
                return recipeDatabase.DeleteRecipe(recipe.ID);
>>>>>>> feature/UserDatabase
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
<<<<<<< HEAD
               
                return recipeDatabase.SelectRecipeByID(id);
=======
                // will have to pass id once database method is implemented
                return recipeDatabase.SelectRecipe(id);
>>>>>>> feature/UserDatabase
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
                //return a empty list if an exception for now
                return new ObservableCollection<Recipe>();
            }
        }
    }
}

