using System;
using CookNook.Model;
namespace CookNook;



public partial class RecipePopUpView : ContentPage
{
    private Recipe recipe;
    public RecipePopUpView(Recipe inRecipe)
    {
        InitializeComponent();
        recipe = inRecipe;
        BindingContext = recipe;
    }

    private async void CloseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
    private async void FullRecipeButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
        await Navigation.PushModalAsync(new RecipeDetailedView(recipe));
    }
}
