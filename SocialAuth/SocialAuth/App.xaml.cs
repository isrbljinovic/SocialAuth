using System;
using SocialAuth.Services;
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

            DependencyService.Register<MockDataStore>();
            MainPage = new LoginPage();//new AppShell();
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
