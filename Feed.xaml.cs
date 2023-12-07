using System.Collections.ObjectModel;
using CookNook.Model;
using System.Diagnostics;
using CookNook.Services;

namespace CookNook;

public partial class Feed : ContentPage
{
    private ObservableCollection<Recipe> recipes;

    public ObservableCollection<Recipe> Recipes
    {
        get { return recipes; }
        set { recipes = value; }
    }

    private IRecipeLogic recipeLogic;
    private User user;
    public Feed(User inUser)
    {
        InitializeComponent();
        //recipeLogic = new RecipeLogic(new RecipeDatabase(), new IngredientLogic(new IngredientDatabase()));
        recipeLogic = MauiProgram.ServiceProvider.GetService<IRecipeLogic>();
        user = inUser;
        PopulateFeedWithRecipes();
    }

    public Feed()
    {
        InitializeComponent();
        recipeLogic = MauiProgram.ServiceProvider.GetService<IRecipeLogic>();
        user = UserViewModel.Instance.AppUser;
        PopulateFeedWithRecipes();
    }
    
    public Feed(IRecipeLogic recipeLogic)
    {
        this.recipeLogic = recipeLogic;
    }

    private void PopulateFeedWithRecipes()
    {
        Recipes = recipeLogic.FeedRecipes();
        // making sure they have proper data
        RecipesCollectionView.ItemsSource = recipes;
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
            var popup = new RecipePopUpView(recipe, user);
            await Navigation.PushModalAsync(popup);
        }
    }
}