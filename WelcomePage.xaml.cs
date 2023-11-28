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
        await Navigation.PushAsync(new LoginPage());
    }

    public async void SignUpClicked(object sender, EventArgs e)
    {
        // since WelcomePage doesn't use UserLogic, but SignUpPage needs it...
        IUserLogic userLogic;

        // pull it from the Service
        userLogic = MauiProgram.ServiceProvider.GetService<IUserLogic>();
        await Navigation.PushAsync(new SignUpPage(userLogic));
    }

    public async void SkipClicked(object sender, EventArgs e)
    {
        
        try
        {
            await Navigation.PushAsync(new Feed());
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
}
