using System.Collections.ObjectModel;
using CookNook.Model;
using System.Diagnostics;
using CookNook.Services;
using System.Collections.Specialized;

namespace CookNook;

public partial class Feed : ContentPage, INotifyCollectionChanged
{
    private ObservableCollection<Recipe> recipes;

    public event NotifyCollectionChangedEventHandler CollectionChanged;
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
        PopulateRandomRecipes();
    }

    public Feed()
    {
        InitializeComponent();
        recipeLogic = MauiProgram.ServiceProvider.GetService<IRecipeLogic>();
        user = UserViewModel.Instance.AppUser;
        PopulateRandomRecipes();
    }
    
    public Feed(IRecipeLogic recipeLogic)
    {
        this.recipeLogic = recipeLogic;
    }

    private void PopulateRandomRecipes()
    {
        Recipes = recipeLogic.GetRandomFeedRecipes();
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
    public async void RandomRecipesClicked(object sender, EventArgs e)
    {
        recipes = recipeLogic.GetRandomFeedRecipes();
        RecipesCollectionView.ItemsSource = recipes;
    }
    public async void BestRecipesClicked(object sender, EventArgs e)
    {
        recipes = recipeLogic.GetBestFeedRecipes();
        RecipesCollectionView.ItemsSource = recipes;
    }
}