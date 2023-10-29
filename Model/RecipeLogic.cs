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
       public RecipeAdditionError CreateRecipe(int inId, string inName, string inDescription, int inAuthor,
            String inIngredients,String inIngredientsQty,
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

           
            AddRecipe(newRecipe);
            return RecipeAdditionError.NoError;
        }


        public RecipeAdditionError AddRecipe(Recipe recipe)
        {
            if (FindRecipe(recipe.ID) != null)
                return RecipeAdditionError.DuplicateId;

            try
            {
                return recipeDatabase.InsertRecipe(recipe);
            }
            catch (Exception ex)
            {
                // returning a generic error.
                return RecipeAdditionError.DBAdditionError;
            }
        }

        public RecipeEditError EditRecipe(Recipe recipe)
        {
            if (FindRecipe(recipe.ID) == null)
                return RecipeEditError.RecipeNotFound;

            try
            {
                return recipeDatabase.EditRecipe(recipe);
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
                return recipeDatabase.DeleteRecipe(recipe.ID);
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
               
                return recipeDatabase.SelectRecipeByID(id);
            }
            catch
            {
                return null;
            }
        }
    }
}

