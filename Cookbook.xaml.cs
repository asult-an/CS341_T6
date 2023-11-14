using CookNook.Model;
using System.Collections.ObjectModel;

namespace CookNook;

public partial class Cookbook : ContentPage
{
    private RecipeLogic recipeLogic;

    public Cookbook()
    {
        InitializeComponent();
        recipeLogic = new RecipeLogic();
       // DeleteRecipe();
        LoadRecipes();
    }

    private void DeleteRecipe()
    {
        Recipe todelete = recipeLogic.FindRecipe(3);
       // recipeLogic.DeleteRecipe(todelete);
    }
    private void LoadRecipes()
    {
        ObservableCollection<Recipe> recipes = recipeLogic.SelectAllRecipes();
        recipesCollectionView.ItemsSource = recipes;
    }
}