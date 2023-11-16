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
			await DisplayAlert("Error", "Registration Error", "Okay");
		}
		else
		{
            var nextPage = new LoginPage();
            await Navigation.PushAsync(nextPage);
        }
        //change to login page instead of pushing?

    }

    public async void BackClicked(object sender, EventArgs e)
    {

        await Navigation.PopAsync();
    }

}
