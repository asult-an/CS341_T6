using CookNook.Model;

namespace CookNook;

public partial class SignUpPage : ContentPage
{
	private UserLogic userLogic = new UserLogic();
	public SignUpPage()
	{
		InitializeComponent();
	}

	public async void SignUpClicked (object sender, EventArgs e)
	{
		UserAdditionError result = userLogic.RegisterNewUser(Username.Text, UserEmail.Text, Password.Text, ConfirmPassword.Text);
		if(result != UserAdditionError.NoError)
		{
			DisplayAlert("Error", "Registration Error", "Okay");
		}
        //change to login page instead of pushing?
        var nextPage = new LoginPage();
		await Navigation.PushAsync(nextPage);

    }

}
