using CookNook.Model;
using CookNook.Services;
using System.Diagnostics;
using CookNook.XMLHelpers;
using CookNook.Model.Interfaces;
using System.ComponentModel;

namespace CookNook;

public partial class Profile : ContentPage, INotifyPropertyChanged
{
    private IRecipeLogic recipeLogic;
    private IUserLogic userLogic;
    private User user;
    public User AppUser { get { return user; } set { user = value; } }

    public event PropertyChangedEventHandler PropertyChanged;
    private ImageSource userImage;
    public ImageSource UserImage
    {
        get => userImage;
        set
        {
            if (userImage != value)
            {
                userImage = value;
                OnPropertyChanged(nameof(UserImage));
            }
        }
    }


    public Profile()
	{
        recipeLogic = new RecipeLogic(new RecipeDatabase(), new IngredientLogic(new IngredientDatabase()));
        user = UserViewModel.Instance.AppUser;
        userLogic = MauiProgram.ServiceProvider.GetService<IUserLogic>();
        InitializeComponent();
        BindingContext = this;
        LoadProfilePic(user);
        LoadRecipes(user.Id);
    }
    public Profile(User inUser)
    {
        recipeLogic = new RecipeLogic(new RecipeDatabase(), new IngredientLogic(new IngredientDatabase()));
        userLogic = MauiProgram.ServiceProvider.GetService<IUserLogic>();
        InitializeComponent();
        user = inUser;
        BindingContext = this;
        LoadProfilePic(user);
        LoadRecipes(user.Id);
    }
    public async void SettingsClicked(object sender, EventArgs e)
	{
        Debug.WriteLine(user.Username);
        UserSettings userSettingsPage = new UserSettings();
        await Navigation.PushAsync(userSettingsPage);

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

    private void LoadRecipes(long userID)
    {
        userRecipesCollectionView.ItemsSource = recipeLogic.CookBookRecipes(userID);
    }

    private void LoadProfilePic(User user)
    {
        byte[] userPic = userLogic.GetProfilePic(user);
        if (userPic != null)
        {
            var imageConverter = new ByteToImageConverter();
            UserImage = (ImageSource)imageConverter.Convert(userPic, null, null, null);
        }
    }
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}