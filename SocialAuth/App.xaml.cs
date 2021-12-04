using System;
using SocialAuth.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialAuth
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();//MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}