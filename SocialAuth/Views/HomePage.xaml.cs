using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}