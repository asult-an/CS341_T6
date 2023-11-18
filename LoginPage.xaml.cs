using CookNook.Model;
using System.Diagnostics;

namespace CookNook;

public partial class LoginPage : ContentPage
{

    private UserLogic userLogic = new UserLogic();
    public LoginPage()
	{
		InitializeComponent();
	}

	public async void LoginClicked(object sender, EventArgs e)
	{
		UserAuthenticationError result = userLogic.AuthenticateUser(Username.Text, Password.Text);
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
			//PASS USER INTO FEED
            var nextPage = new TabView(user);
            await Navigation.PushAsync(nextPage);
        }
	}

	public async void BackClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}

