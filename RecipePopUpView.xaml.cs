using System;
using CookNook.Model;
namespace CookNook;

public partial class RecipePopUpView : ContentPage
{
    public RecipePopUpView(Recipe recipe)
    {
        InitializeComponent();
        BindingContext = recipe;
    }

    private async void CloseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
