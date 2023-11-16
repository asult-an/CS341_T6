using CookNook.Model;
using System.Collections.ObjectModel;

namespace CookNook;

public partial class Cookbook : ContentPage
{

    private RecipeLogic recipeLogic;

    public Cookbook()
    {
        InitializeComponent();
        recipeLogic = new RecipeLogic(new RecipeDatabase());
      
        LoadRecipes();
    }

   
    private void LoadRecipes()
    {
        List<Recipe> recipes = recipeLogic.SelectAllRecipes();
        //recipesCollectionView.ItemsSource = recipes;
     }
    

    public async void UserProfileClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Profile());

    }
}