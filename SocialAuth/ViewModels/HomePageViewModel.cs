using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SocialAuth.Models;

namespace SocialAuth.ViewModels
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public HomePageViewModel(UserProfile userProfile)
        {
        }
    }
}