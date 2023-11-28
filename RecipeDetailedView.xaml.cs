namespace CookNook;
using CookNook.Model;
using System.ComponentModel;
using System.Diagnostics;

public partial class RecipeDetailedView : ContentPage
{
	private Recipe recipe;


    public RecipeDetailedView(Recipe inRecipe)
	{
		InitializeComponent();
		recipe = inRecipe;
        BindingContext = recipe;
        
		
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