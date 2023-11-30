using CookNook.Model;
using System.Diagnostics;

namespace CookNook;

public partial class TabView : TabbedPage
{
    private User user;
	public TabView()
	{
		InitializeComponent();
        this.CurrentPageChanged += OnCurrentPageChanged;
    }
    public TabView(User inUser)
    {
        InitializeComponent();
        user = inUser;
        this.CurrentPageChanged += OnCurrentPageChanged;
    }
    private void OnCurrentPageChanged(object sender, EventArgs e)
    {
        // Check if the current page is a NavigationPage
        if (CurrentPage is NavigationPage navigationPage)
        {
            if (navigationPage == Children[2]) 
            {
                var child = ((NavigationPage)Children[2]).CurrentPage;
                if (child is Cookbook cookbook)
                {
                    // It's an instance of Cookbook, perform the cast and method call
                    cookbook.LoadRecipes(user.Id);
                    Debug.WriteLine("SUCCESS");
                }
                Debug.WriteLine(((NavigationPage)Children[2]).CurrentPage.GetType());

                //((Cookbook)Children[2]).LoadRecipes(user.Id);
            }
            // Trigger your method when the NavigationPage is selected
            
        }
    }

    //private async void PageClicked(object sender, EventArgs e)
    //{
    //    var currentPage = CurrentPage as NavigationPage; if (currentPage?.CurrentPage is AddRecipePage addRecipePage) 
    //    {
    //        addRecipePage.PageUser = user;
    //    }
    //}
}