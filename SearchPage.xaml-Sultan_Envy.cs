using CookNook.Model;

namespace CookNook;

public partial class SearchPage : ContentPage
{

	private UserLogic userLogic = new UserLogic();
    private User user;
	public SearchPage()
	{
		InitializeComponent();
	}

    public SearchPage(User inUser)
    {
        InitializeComponent();
        user = inUser;
    }
    public async void SearchClicked(object sender, EventArgs e)
	{
		//Search method
	}

    public async void UserProfileClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Profile());
    }

}
