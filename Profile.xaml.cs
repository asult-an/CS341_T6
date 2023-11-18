using CookNook.Model;
using System.Diagnostics;

namespace CookNook;

public partial class Profile : ContentPage
{
	private User user;
    public User AppUser { get { return user; } set { user = value; } }
    
	public Profile()
	{
		InitializeComponent();
        BindingContext = this;
    }
    public Profile(User inUser)
    {
        InitializeComponent();
        user = inUser;
        BindingContext = this;
    }
    public async void SettingsClicked(object sender, EventArgs e)
	{
        Debug.WriteLine(user.Username);
        UserSettings userSettingsPage = new UserSettings();
        await Navigation.PushAsync(userSettingsPage);

    }
    public async void SearchClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SearchPage());
    }
}