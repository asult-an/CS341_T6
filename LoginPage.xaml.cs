using CookNook.Model;
using CookNook.Services;
using System.Diagnostics;
using CookNook.Model.Interfaces;


namespace CookNook;

public partial class LoginPage : ContentPage
{
	//private UserLogic userLogic = new UserLogic(new UserDatabase(), new RecipeLogic(new RecipeDatabase(), new IngredientLogic(new IngredientDatabase())));
	private readonly IUserLogic userLogic;
    public LoginPage(IUserLogic userLogic)
	{ 
		this.userLogic = userLogic;
		InitializeComponent();
	}

	public async void LoginClicked(object sender, EventArgs e)
	{
        activityIndicator.IsVisible = true;
        activityIndicator.IsRunning = true;
        await Task.Delay(1);
        UserAuthenticationError result;
        
		try
		{
            Debug.WriteLine(Username.Text, Password.Text);
            result = userLogic.AuthenticateUser(Username.Text, Password.Text);
        }
        catch (Exception ex)
		{
			Debug.WriteLine(ex.Message);
			result = UserAuthenticationError.InvalidUsername;
		}
        
        //TODO: Change invalid username/password alert to be identical.
        if (result == UserAuthenticationError.InvalidUsername)
		{
			DisplayAlert("Error", "Invalid Username", "Retry");
		}
		else if(result == UserAuthenticationError.InvalidPassword)
		{
            DisplayAlert("Error", "Invalid Password", "Retry");
        }
		else
		{
			try
			{
				
                User user = userLogic.GetUserByUsername(Username.Text);

                UserViewModel.Instance.AppUser = user;

                var nextPage = new TabView(user);
				MainThread.BeginInvokeOnMainThread(() =>
				{
					activityIndicator.IsVisible = false;
					activityIndicator.IsRunning = false;
				});

                await Navigation.PushAsync(nextPage);
            }
			catch (Exception ex)
			{
                DisplayAlert("Error", "No user found", "Close");
                Debug.WriteLine("DB Retrieval Failed");
                Debug.WriteLine(ex.Message);
			}
			
			
        }
        MainThread.BeginInvokeOnMainThread(() =>
        {
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;
        });
    }

	public async void BackClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}

