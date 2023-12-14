using CookNook.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CookNook.Model.Interfaces;

namespace CookNook;

public partial class WelcomePage : ContentPage
{
    //TEST DATABASE INSERTS
    
	public WelcomePage()
	{
		InitializeComponent();
        
	}

	public async void LoginClicked(object sender, EventArgs e)
    {
        IUserLogic userLogic = MauiProgram.ServiceProvider.GetService<IUserLogic>();
        await Navigation.PushAsync(new LoginPage(userLogic));
    }

    public async void SignUpClicked(object sender, EventArgs e)
    {
        // since WelcomePage doesn't use UserLogic, but SignUpPage needs it...
        IUserLogic userLogic = MauiProgram.ServiceProvider.GetService<IUserLogic>();
        await Navigation.PushAsync(new SignUpPage(userLogic));
    }

    public void SkipClicked(object sender, EventArgs e)
    {
        try
        {
            //TODO: Clear or reset user-related information
            //we need to create this function to erase every user logic if there was one already
            //ClearUserInformation();

            // Navigate to the Feed page
            Navigation.PushModalAsync(new Feed());
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
}
