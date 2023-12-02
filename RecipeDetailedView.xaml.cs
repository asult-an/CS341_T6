using CookNook.Model.Interfaces;

namespace CookNook;
using CookNook.Model;
using CookNook.Services;
using System.ComponentModel;
using System.Diagnostics;

public partial class RecipeDetailedView : ContentPage
{
    private Recipe recipe;
    private User user;

    public User User
    {
        get { return user; }
        set { user = value; }
    }

    public Recipe Recipe
    {
        get { return recipe; }
        set { recipe = value; }
    }


    private IUserLogic userLogic;

    public RecipeDetailedView(Recipe inRecipe, User user)
	{
		InitializeComponent();
        try
        {

		recipe = inRecipe;
        //BindingContext = this;
        BindingContext = recipe;
        Debug.WriteLine($"Viewing recipe {inRecipe.ID} by {recipe.AuthorID}");
        //userLogic = new UserLogic(new Model.Services.UserDatabase(), new RecipeLogic(new RecipeDatabase(), new IngredientLogic(new IngredientDatabase())));
        userLogic = MauiProgram.ServiceProvider.GetService<IUserLogic>();
        User = userLogic.GetUserById(recipe.AuthorID);

        AuthorName.BindingContext = User;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[RecipeDetailedView] (ERROR!) {ex.InnerException.Message}");
        }

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