namespace CookNook;
using CookNook.Model;
using CookNook.Services;
using System.ComponentModel;
using System.Diagnostics;

public partial class RecipeDetailedView : ContentPage
{
	private Recipe recipe;
    private UserLogic userLogic;

    public RecipeDetailedView(Recipe inRecipe, User user)
	{
		InitializeComponent();
		recipe = inRecipe;
        BindingContext = recipe;
        Debug.WriteLine(recipe.AuthorID);
        userLogic = new UserLogic(new Model.Services.UserDatabase(), new RecipeLogic(new RecipeDatabase(), new IngredientLogic(new IngredientDatabase())));
        User author = userLogic.GetUserById(recipe.AuthorID);
        
        AuthorName.BindingContext = author;
        

        //try
        //{
        //    //Failing because FollowerIds is set to Null still?
        //    numFollowers = recipe.FollowerIds.Length;
        //}
        //catch (Exception ex)
        //{
        //    Debug.WriteLine("IN NUMFOLLOWERS");
        //    Debug.WriteLine(ex.Message);
        //}

    }
    private async void CloseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }


}