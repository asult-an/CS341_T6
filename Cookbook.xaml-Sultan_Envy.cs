using CookNook.Model;
using System.Collections.ObjectModel;

namespace CookNook;

public partial class Cookbook : ContentPage
{
    private IRecipeLogic recipeLogic;

    public Cookbook()
    {
        InitializeComponent();
        recipeLogic = new RecipeLogic(new RecipeDatabase());
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

