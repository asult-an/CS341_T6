using CookNook.Model;
namespace CookNook;

public partial class Profile : ContentPage
{
    private User user;
	public Profile()
	{
		InitializeComponent();
	}

    public Profile(User inUser)
    {
        InitializeComponent();
        user = inUser;
    }
    public async void SettingsClicked(object sender, EventArgs e)
	{
		UserSettings userSettingsPage = new UserSettings(user);
        await Navigation.PushAsync(userSettingsPage);

    }
    public async void SearchClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SearchPage(user));
    }
}