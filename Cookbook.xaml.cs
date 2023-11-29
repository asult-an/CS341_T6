using CookNook.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CookNook.Services;

namespace CookNook;

public partial class Cookbook : ContentPage
{
    private User user;
    private IRecipeLogic recipeLogic;
    public User AppUser { get { return user; } set { user = value; } }
    public string PageTitle { get { return user.Username + "'s Cookbook"; } }
    public Cookbook()
    {
        InitializeComponent();
        recipeLogic = new RecipeLogic(new RecipeDatabase(), new IngredientLogic(new IngredientDatabase()));
        user = UserViewModel.Instance.AppUser;
        BindingContext = this;
        LoadRecipes(user.Id);
        
    }

    // Constructor with dependency injection (UNUSED)
    public Cookbook(IRecipeLogic recipeLogic)
    {
        InitializeComponent();
        this.recipeLogic = recipeLogic;
        LoadRecipes(user.Id);
    }

    private void LoadRecipes(long userID)
    {
        recipesCollectionView.ItemsSource = recipeLogic.CookBookRecipes(userID);
    }
    public async void OnItemTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is Recipe recipe)
        {
            // Navigate to the RecipeDetailPage with the selected recipe
            var page = new RecipeDetailedView(recipe, user);
            await Navigation.PushAsync(page);
        }
    }

    // Other methods
}

