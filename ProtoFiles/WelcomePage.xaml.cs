namespace ProtoFiles;

public partial class WelcomePage : ContentPage
{
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
        await Navigation.PushAsync(new Feed());
    }

}
