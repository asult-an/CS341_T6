using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook;

public partial class DietaryRestrictionsPage : ContentPage
{
    public DietaryRestrictionsPage()
    {
        InitializeComponent();
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