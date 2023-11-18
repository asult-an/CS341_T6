using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookNook.Model;

namespace CookNook;

public partial class DietaryRestrictionsPage : ContentPage
{
    private User user;
    public DietaryRestrictionsPage(User inUser)
    {
        InitializeComponent();
        user = inUser;
    }

    public async void BackClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }

    public async void FinishedAddClick(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TabView());
    }
}