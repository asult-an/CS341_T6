namespace CookNook;

public partial class UserSettings : ContentPage
{
	public UserSettings()
	{
		InitializeComponent();
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