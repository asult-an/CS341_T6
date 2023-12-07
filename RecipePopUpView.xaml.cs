using System;
using CookNook.Model;
namespace CookNook;



public partial class RecipePopUpView : ContentPage
{
    private Recipe recipe;
    private User user;

    public RecipePopUpView(Recipe inRecipe, User user)
    {
        InitializeComponent();
        recipe = inRecipe;
        this.user = user;
        BindingContext = recipe;
    }

    private async void CloseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
    private async void FullRecipeButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
        await Navigation.PushModalAsync(new RecipeDetailedView(recipe, user));
    }
}
