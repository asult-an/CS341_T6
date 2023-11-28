using CookNook.Model;
using System.Diagnostics;
using CookNook.Services;

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
        loadRecipes();
    }
    public Feed()
    {
        InitializeComponent();
        recipeLogic = new RecipeLogic(new RecipeDatabase());
        user = UserViewModel.Instance.AppUser;
        loadRecipes();
    }
    
    public Feed(IRecipeLogic recipeLogic)
    {
        this.recipeLogic = recipeLogic;
    }

    private void loadRecipes()
    {
        recipesCollectionView.ItemsSource = recipeLogic.FeedRecipes();
    }

    public async void UserProfileClicked(object sender, EventArgs e)
    {
        Debug.WriteLine(user.Username);
        await Navigation.PushAsync(new Profile(user));
    }
    public async void SearchClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SearchPage());
    }

    public async void OnItemTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is Recipe recipe)
        {
            // Navigate to the RecipePopUpPage with the selected recipe
            var popup = new RecipePopUpView(recipe);
            await Navigation.PushModalAsync(popup);
        }
    }
}