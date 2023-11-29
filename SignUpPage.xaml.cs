using CookNook.Model;
using CookNook.Model.Interfaces;

namespace CookNook;

public partial class SignUpPage : ContentPage
{

	private readonly IUserLogic userLogic;

	public SignUpPage(IUserLogic userLogic)
	{
		this.userLogic = userLogic;
		InitializeComponent();
	}

	public async void SignUpClicked (object sender, EventArgs e)
	{
		UserAdditionError result = userLogic.TryRegisterNewUser(Username.Text, UserEmail.Text, Password.Text, ConfirmPassword.Text);
		if(result != UserAdditionError.NoError)
		{
			await DisplayAlert("Error", "Registration Error", "Okay");
		}
		else
		{
			// fetch the service through DI
			IUserLogic userLogic = MauiProgram.ServiceProvider.GetService<IUserLogic>();

            var nextPage = new LoginPage(userLogic);
            await Navigation.PushAsync(nextPage);
        }
        //change to login page instead of pushing?

    }

    public async void BackClicked(object sender, EventArgs e)
    {

        await Navigation.PopAsync();
    }

}
