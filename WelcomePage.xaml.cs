using CookNook.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
        await Navigation.PushAsync(new SignUpPage());
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
