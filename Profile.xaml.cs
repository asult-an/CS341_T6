namespace CookNook;

public partial class Profile : ContentPage
{

	public Profile()
	{
		InitializeComponent();
	}
	public async void SettingsClicked(object sender, EventArgs e)
	{
		UserSettings userSettingsPage = new UserSettings();
        await Navigation.PushAsync(userSettingsPage);

    }
}