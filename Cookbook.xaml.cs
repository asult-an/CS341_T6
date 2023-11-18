using CookNook.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
        LoadRecipes();
        
    }

    // Constructor with dependency injection
    public Cookbook(IRecipeLogic recipeLogic)
    {
        InitializeComponent();
        this.recipeLogic = recipeLogic;
        LoadRecipes();
    }

    private void LoadRecipes()
    {
        recipesCollectionView.ItemsSource = recipeLogic.CookBookRecipes();
    }

    // Other methods
}

