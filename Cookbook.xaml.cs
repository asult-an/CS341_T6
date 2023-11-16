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
      
        //LoadRecipes();
    }

    public Cookbook(IRecipeLogic recipeLogic)
    {
        InitializeComponent();
        this.recipeLogic = recipeLogic;

        LoadRecipes();
    }
   
    // TODO: Add comments.  What does this do?
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