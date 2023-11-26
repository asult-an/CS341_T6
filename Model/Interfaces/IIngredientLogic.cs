namespace CookNook.Model.Interfaces
{
    public interface IIngredientLogic
    {
        /// <summary>
        /// Inserts a new ingredient into the database, as long as an
        /// existing ingredient isn't already present 
        /// </summary>
        /// <remarks>For <b>creating an ingredient from AddRecipeIngredientsPage</b>.
        /// </remarks>
        /// <param name="recipeId">UNCHANGED: </param>
        /// <param name="name">the name of the ingredient</param>
        /// <param name="unit">Nullable unit field, in case ingredient is unitless</param>
        /// <returns>IngredientError</returns>
        //IngredientAdditionError CreateIngredient(Int64 ingredientId, string name, string? unit);
        //public IngredientAdditionError CreateIngredientForRecipe(Int64 recipeId, string name, string? unit, string quantity);

        /// <summary>
        /// Creates an ingredient in the database, not attached to any recipe.
        /// This type of creation is suitable from ingredient management
        /// </summary>
        /// <param name="name"></param>
        /// <returns>IngredientAdditionError</returns>

        IngredientAdditionError CreateIngredient(string name);

        /// <summary>
        /// Update an ingredient 
        /// </summary>
        /// <param name="ingredient"></param>
        /// <returns></returns>
        IngredientUpdateError UpdateIngredient(Ingredient ingredient);

        /// <summary>
        /// Deletes an ingredient from the database, if it exists.
        /// If an error occurs, the error should be propogated up so the UI
        /// can smoothly inform the user.
        /// </summary>
        /// <param name="ingredientId"></param>
        /// <returns></returns>
        IngredientDeleteError DeleteIngredient(Int64 ingredientId);


        /// <summary>
        /// Fetch a particular ingredient by its Id.
        /// </summary>
        /// <param name="ingredientId"></param>
        /// <returns>An ingredient object</returns>
        Ingredient GetIngredientById(Int64 ingredientId);


        /// <summary>
        /// Fetches an ingredient by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>An ingredient, otherwise null/error</returns>
        Ingredient GetIngredientByName(string name);

        //List<Ingredient> GetIngredientRange(Int64[] ingredientIds);

        /// <summary>
        /// Given a recipeId, queries all the ingredients in the database
        /// that are associated with that particular recipe. 
        /// If an error occurs, the error should be propogated up so the UI
        /// can smoothly inform the user.
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        List<Ingredient> GetIngredientsInRecipe(Int64 recipeId);


        /// <summary>
        /// Fetches all ingredients from the database
        /// </summary>
        /// <returns>A list of Ingredient type</returns>
        List<Ingredient> GetAllIngredients();

        ///// <summary>
        ///// Checks for an existing Ingredient before attempting to add it.
        ///// If the ingredient is found, then it will return that Ingredient.
        ///// If it's not found, and the data is valid, then we add it to the db.
        ///// </summary>
        ///// <remarks>
        ///// Note that the dynamic return type allows for IngredientAdditionErrors
        ///// in the event that the data is invalid
        ///// </remarks>
        ///// <param name="name"></param>
        ///// <param name="quantity"></param>
        ///// <param name="unit"></param>
        ///// <returns>Either the Ingredient (existing/inserted) or a IngredientAdditionError</returns>
        //dynamic GetOrCreateIngredient(string name, string quantity, string? unit);

    }
}
