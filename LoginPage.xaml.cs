using CookNook.Model;
using CookNook.Services;
using System.Diagnostics;

namespace CookNook;

public partial class LoginPage : ContentPage
{

    private UserLogic userLogic = new UserLogic(new UserDatabase(), new RecipeLogic(new RecipeDatabase(), new IngredientLogic(new IngredientDatabase())));
    public LoginPage()
	{ 
		InitializeComponent();
	}

	public async void LoginClicked(object sender, EventArgs e)
	{
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
			User user = userLogic.GetUserByUsername(Username.Text);
			if (user == null)
			{

				DisplayAlert("Error", "No user found", "Close");
				Debug.WriteLine("DB Retreival Failed");
			}
			UserViewModel.Instance.AppUser = user;
			
            var nextPage = new TabView(user);

            await Navigation.PushAsync(nextPage);
        }
	}

	public async void BackClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}

