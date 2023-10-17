namespace ProtoFiles;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new WelcomePage();
		//MainPage = new LoginPage();
		//MainPage = new SignUpPage();
	}
}

