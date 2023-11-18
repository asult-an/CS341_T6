using CookNook.Model;
namespace CookNook;

public partial class TabView : TabbedPage
{
    private User user;
	public TabView()
	{
		InitializeComponent();
	}
    public TabView(User inUser)
    {
        InitializeComponent();
        user = inUser;
    }

    private async void PageClicked(object sender, EventArgs e)
    {
        var currentPage = CurrentPage as NavigationPage; if (currentPage?.CurrentPage is AddRecipePage addRecipePage) 
        {
            addRecipePage.PageUser = user;
        }
    }
}