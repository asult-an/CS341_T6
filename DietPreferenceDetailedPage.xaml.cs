using System.ComponentModel;
using CookNook.Model;
using CookNook.Model.Interfaces;

namespace CookNook;

public partial class DietPreferenceDetailedPage : ContentPage, INotifyPropertyChanged
{
 
    private DietPreference dietPreference;
    private IPreferenceProvider preferenceProvider = MauiProgram.ServiceProvider.GetService<IPreferenceProvider>();


    public DietPreferenceDetailedPage(DietPreference dietPreference)
    {
        InitializeComponent();
        this.dietPreference = dietPreference ?? throw new ArgumentNullException(nameof(dietPreference));
        BindingContext = this.dietPreference;
    }

    /// <summary>
    /// Savs the sures's ppreferneces
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // Implement the logic to save the changes to the DietPreference
        var user = UserViewModel.Instance.AppUser;
        user.DietaryPreferences.Add(dietPreference);
        
        // write all the user's preferences
        preferenceProvider.OverwritePreferencesJSON(user.DietaryPreferences);
        preferenceProvider.UpdateLocalSettingsAsync();

        await DisplayAlert("Info", "Diet preference saved successfully.", "OK");
        // TODO: tell the DietaryRestriction page to get new collections
        await Navigation.PopAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        // don't do anything more than just navigating away from the page
        await Navigation.PopAsync();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}