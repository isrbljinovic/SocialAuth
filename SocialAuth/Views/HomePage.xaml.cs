using System;
using SocialAuth.Models;
using SocialAuth.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialAuth.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage(UserProfile userProfile)
        {
            InitializeComponent();
            BindingContext = new HomePageViewModel(userProfile);
        }

        private async void Entry_Completed(object sender, EventArgs e)
        {
            await ((HomePageViewModel)BindingContext).GetArtistData();
        }
    }
}