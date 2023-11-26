namespace CookNook.Model.Interfaces;

/// <summary>
/// Database service to handle the association of recipes and lists (called "cookbook pages") 
/// so that the other services don't have to carry the weight
/// </summary>
public interface ICookbookPageDatabase
{
    // TODO: implement CRUD operations for cookbooks themselves

    /// <summary>
    /// Adds a recipe to a cookbook page
    /// </summary>
    /// <param name="recipeID"></param>
    /// <param name="cookbookPageID"></param>
    /// <returns></returns>
    public CookbookPageAdditionError AddRecipeToCookbookPage(Int64 recipeID, Int64 cookbookPageID);
    
    /// <summary>
    /// Removes a recipe from a cookbook page, though we call it an update 
    /// </summary>
    /// <param name="recipeID"></param>
    /// <param name="cookbookPageID"></param>
    /// <returns></returns>
    public CookbookPageDeletionError RemoveRecipeFromCookbookPage(Int64 recipeID, Int64 cookbookPageID);
    
    // TODO: move to CookbookPageLogic

    
    ///<summary>
    /// Gets all recipes on a given cookbook page
    /// </summary>
    /// <param name="cookbookPageID"></param>
    /// <returns></returns>
    public List<long> GetRecipeIdsForCookbookPage(Int64 cookbookPageID);
    
    /// <summary>
    /// Gets all cookbook pages for a given user
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public List<CookbookPageModel> GetCookbookPagesForUser(Int64 userID);


       
    /// <summary>
    /// Fetches a single cookbook page by its id
    /// </summary>
    /// <param name="cookbookPageID"></param>
    /// <returns></returns>
    public CookbookPageModel GetCookbookPageByID(Int64 cookbookPageID);


    /// <summary>
    /// To fetch a page by name, the author's Id must be supplied for the operation to succeed
    /// </summary>
    /// <param name="userId">id of the list's author</param>
    /// <param name="name">page title of the list</param>
    /// <returns></returns>
    public CookbookPageModel GetCookbbookPageByName(long userId, string name);
}