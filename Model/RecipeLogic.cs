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

       
       public RecipeAdditionError CreateRecipe(int inId, string inName, string inDescription, int inAuthor,
            ObservableCollection<string> inIngredients, ObservableCollection<String> inIngredientsQty,
            int inCooktime, string inCourse, int inRating, int inServings, string inImage,
            ObservableCollection<string> inTags, ObservableCollection<string> inFollowers)
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

        public Recipe FindRecipe(int id)
        {
            try
            {
                return recipeDatabase.SelectRecipe(id);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
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
                Debug.WriteLine(ex);
                return null;
            }
        }

        public int GetSmallestAvailableId()
        {
            var allIds = recipeDatabase.GetAllRecipeIds(); 
            allIds.Sort();

            int smallestAvailableId = 0;
            foreach (var id in allIds)
            {
                if (id == smallestAvailableId)
                {
                    smallestAvailableId++; // Increment to the next ID if the current one is taken
                }
                else
                {
                    // break when we find unused ID.
                    break;
                }
            }

            return smallestAvailableId; 
        }


    }
}

