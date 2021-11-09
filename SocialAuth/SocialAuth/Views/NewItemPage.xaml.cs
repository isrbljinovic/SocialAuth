using System;
using System.Collections.Generic;
using System.ComponentModel;
using SocialAuth.Models;
using SocialAuth.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialAuth.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}