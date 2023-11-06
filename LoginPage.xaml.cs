using CookNook.Model;

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
		if(result == UserAuthenticationError.InvalidUsername)
		{
			DisplayAlert("Error", "Invalid Username", "Retry");
		}
		else if(result == UserAuthenticationError.InvalidPassword)
		{
            DisplayAlert("Error", "Invalid Password", "Retry");
        }
		else
		{
            var nextPage = new Feed();
            await Navigation.PushAsync(nextPage);
        }
	}
}
