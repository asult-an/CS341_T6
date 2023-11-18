using CookNook.Model;
namespace CookNook;

public partial class UserSettings : ContentPage
{
    private User user;
	public UserSettings()
	{
		InitializeComponent();
	}

    public UserSettings(User inUser)
    {
        InitializeComponent();
        user = inUser;
    }

    public async void UserAccountSettingsClicked(object sender, EventArgs e)
	{
		AccountSettings accountSettings = new AccountSettings();
		await Navigation.PushAsync(accountSettings);
	}

    public async void UserPreferencesClicked(object sender, EventArgs e)
    {
        //AccountSettings accountSettings = new AccountSettings();
        //await Navigation.PushAsync(accountSettings);
    }
}