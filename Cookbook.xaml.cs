using CookNook.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CookNook.Services;
using System.Collections.Specialized;

namespace CookNook;

public partial class Cookbook : ContentPage, INotifyCollectionChanged
{
    private User user;
    private IRecipeLogic recipeLogic;

    public event NotifyCollectionChangedEventHandler CollectionChanged;

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

    //TODO: modify load recipes to get servings, rating, and ingredients. 
    public void LoadRecipes(long userID)
    {
        recipesCollectionView.ItemsSource = recipeLogic.CookBookRecipes(userID);
    }
    public async void OnItemTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is Recipe recipe)
        {
            // Navigate to the RecipeDetailPage with the selected recipe
            var page = new RecipeDetailedView(recipe, user);
            await Navigation.PushModalAsync(page);
        }
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

    // Other methods
}

