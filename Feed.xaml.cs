using CookNook.Model;
using System.Diagnostics;

namespace CookNook;

public partial class Feed : ContentPage
{
    private IRecipeLogic recipeLogic;
    private User user;
    public Feed(User inUser)
    {
        InitializeComponent();
        recipeLogic = new RecipeLogic(new RecipeDatabase());
        user = inUser;
    }
    public Feed()
    {
        InitializeComponent();
        recipeLogic = new RecipeLogic(new RecipeDatabase());
        user = UserViewModel.Instance.AppUser;
    }
    
    public Feed(IRecipeLogic recipeLogic)
    {
        this.recipeLogic = recipeLogic;
    }

    //private async void ShowAllRecipesClicked(object sender, EventArgs e)
    //{
    //    var allRecipes = recipeLogic.SelectAllRecipes();

    //    // As of now, just getting names of recipes into string
    //    var recipesText = string.Join(", ", allRecipes.Select(r => r.Name));

    //    // then diplaying the string
    //    await DisplayAlert("All Recipes", recipesText, "OK");
    //}

    public async void UserProfileClicked(object sender, EventArgs e)
    {
        Debug.WriteLine(user.Username);
        await Navigation.PushAsync(new Profile(user));
    }
    public async void SearchClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SearchPage());
    }
}