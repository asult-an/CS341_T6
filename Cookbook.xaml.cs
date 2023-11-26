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
        recipeLogic = new RecipeLogic(new RecipeDatabase());
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
    public async void RecipeClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        Recipe recipe = (Recipe)button.BindingContext;
        
        Debug.WriteLine(recipe.CookTime);
        await Navigation.PushAsync(new RecipeDetailedView(recipe));
    }

    // Other methods
}

