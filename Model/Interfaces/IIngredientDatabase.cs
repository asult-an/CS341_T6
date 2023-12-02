using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model.Interfaces
{
    public interface IIngredientDatabase
    {
        /// <summary>
        /// returns all ingredients in the database
        /// 
        /// </summary>
        /// <returns></returns>
        List<Ingredient> GetAllIngredients();

        /// <summary>
        /// Allows for a subset of ingredients to be selected
        /// e.g all recipe_ids in a cookbook page
        /// </summary>
        /// <param name="ingredientIds">Array of ingredientIds</param>
        /// <returns>list of ingredients per the ids</returns>
        //List<Ingredient> GetIngredientRange(Int64[] ingredientIds);

        /// <summary>
        /// Fetch a particular ingredient by its name.
        /// </summary>
        /// <param name="name">Ingredient name.</param>
        /// <returns>Ingredient object if found, null otherwise.</returns>
        Ingredient GetIngredientByName(string name);

 
        /// <summary>
        /// Fetch a particular ingredient by its id.
        /// </summary>
        /// <param name="id">Ingredient id.</param>
        /// <returns>Ingredient object if found, null otherwise.</returns>
        Ingredient GetIngredientById(Int64 id);

        /// <summary>
        /// Fetch all ingredients that belong to a particular recipe
        /// </summary>
        /// <param name="recipeId">recipe id</param>
        /// <returns>List of Ingredient object if found, null otherwise</returns>
        ObservableCollection<Ingredient> GetIngredientsFromRecipe(Int64 recipeId);
        
        
        /// <summary>
        /// Associates an ingredient to a particular recipe by creating a new row 
        /// in the recipe_ingredients table. 
        /// </summary>
        /// <param name="recipeId">Recipe's Id</param>
        /// <param name="ingredientId">Ingredient's Id</param>
        /// <returns></returns>
        IngredientAdditionError AddIngredientToRecipe(Int64 recipeId, Int64 ingredientId);
        
        /// <summary>
        /// Updates an ingredient by its id
        /// </summary>
        /// <param name="ingredientId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        IngredientUpdateError UpdateIngredient(Int64 ingredientId, string name);
        //IngredientUpdateError UpdateIngredient(Int64 ingredientId, string name, string quantity, string? unit);


        /// <summary>
        /// Inserts a new ingredient into the database
        /// </summary>
        /// <param name="ingredient">the new ingredient to be added</param>
        /// <returns></returns>
        IngredientAdditionError CreateIngredient(string name);
        
        /// <summary>
        /// Similar to CreateIngredient, except in cases where a duplicate ingredient is 
        /// inserted, the existing ingredient is instead returned.
        /// 
        /// WARNING: since this features a `dynamic` type,
        /// the resulting object will need to be type-checked so that we can 
        /// handle the operation's result appropriately.
        /// </summary>
        /// <param name="ingredient"></param>
        /// <returns>Either returns an IngredientAdditionError, or the Ingredient</returns>
        dynamic GetOrCreateIngredient(Ingredient ingredient);


        /// <summary>
        /// Remove an ingredient by its ingredientId
        /// </summary>
        /// <param name="ingredientId"></param>
        /// <returns></returns>
        IngredientDeleteError RemoveIngredient(Int64 ingredientId);

    }
}
