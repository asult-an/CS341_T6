namespace CookNook;
using CookNook.Model;
using System.ComponentModel;
using System.Diagnostics;

public partial class RecipeDetailedView : ContentPage
{
	private Recipe recipe;
    private User user;
    private UserLogic userLogic;

    public RecipeDetailedView(Recipe inRecipe, User user)
	{
		InitializeComponent();
		recipe = inRecipe;
        this.user = user;
        BindingContext = recipe;
        
        userLogic = new();
        AuthorName.BindingContext = userLogic.GetUserById(recipe.AuthorID);

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
        await Navigation.PopAsync();
    }


}