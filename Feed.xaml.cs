using CookNook.Model;

namespace CookNook;

public partial class Feed : ContentPage
{
    private RecipeLogic recipeLogic;
    public Feed()
	{
		InitializeComponent();
		recipeLogic = new RecipeLogic();
	}

    private async void ShowAllRecipesClicked(object sender, EventArgs e)
    {
        var allRecipes = recipeLogic.SelectAllRecipes();

        // As of now, just getting names of recipes into string
        var recipesText = string.Join(", ", allRecipes.Select(r => r.Name));

        // then diplaying the string
        await DisplayAlert("All Recipes", recipesText, "OK");
    }

    public async void ProfileClicked(object sender, EventArgs e)
    {
        Profile profilePage = new Profile();
        await Navigation.PushAsync(profilePage);
    }

}