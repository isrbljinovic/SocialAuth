using System;
using System.Collections.Generic;
using SocialAuth.ViewModels;
using SocialAuth.Views;
using Xamarin.Forms;

namespace SocialAuth
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
