using System.Collections.ObjectModel;
using CookNook.Model;
using System.Diagnostics;
using CookNook.Services;
using System.Collections.Specialized;
using CookNook.Model.Interfaces;
using CookNook.XMLHelpers;
using System.ComponentModel;

namespace CookNook;

public partial class Feed : ContentPage, INotifyCollectionChanged, INotifyPropertyChanged
{
    private ObservableCollection<Recipe> recipes;

    public event NotifyCollectionChangedEventHandler CollectionChanged;
    
    public ObservableCollection<Recipe> Recipes
    {
        get { return recipes; }
        set { recipes = value; }
    }

    private IRecipeLogic recipeLogic;
    private IUserLogic userLogic;
    private User user;
    private ImageSource userImageSource;
    public ImageSource UserImageSource
    {
        get { return userImageSource; }
        set
        {
            if (userImageSource != value)
            {
                userImageSource = value;
                OnPropertyChanged(nameof(UserImageSource));
            }
        }
    }
    public Feed(User inUser)
    {
        InitializeComponent();
        BindingContext = this;
        //recipeLogic = new RecipeLogic(new RecipeDatabase(), new IngredientLogic(new IngredientDatabase()));
        recipeLogic = MauiProgram.ServiceProvider.GetService<IRecipeLogic>();
        userLogic = MauiProgram.ServiceProvider.GetService<IUserLogic>();
        user = inUser;
        GetProfilePic();
        PopulateRandomRecipes();
    }

    public Feed()
    {
        InitializeComponent();
        BindingContext = this;
        recipeLogic = MauiProgram.ServiceProvider.GetService<IRecipeLogic>();
        userLogic = MauiProgram.ServiceProvider.GetService<IUserLogic>();
        user = UserViewModel.Instance.AppUser;
        GetProfilePic();
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

    


    private void GetProfilePic()
    {
        byte[] userPic = userLogic.GetProfilePic(user);
        if (userPic != null)
        {
            var imageConverter = new ByteToImageConverter();
            UserImageSource = (ImageSource)imageConverter.Convert(userPic, null, null, null);
        }
    }

    


}