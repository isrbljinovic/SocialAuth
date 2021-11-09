using System.ComponentModel;
using SocialAuth.ViewModels;
using Xamarin.Forms;

namespace SocialAuth.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}