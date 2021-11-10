using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using SocialAuth.Models;
using SocialAuth.Views;
using Xamarin.Forms;

namespace SocialAuth.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand OnLoginWithFacebookCommand { get; set; }

        IFacebookClient _facebookService = CrossFacebookClient.Current;
        public LoginViewModel()
        {
            OnLoginWithFacebookCommand = new Command(async () => await LoginFacebookAsync());
        }

        async Task LoginFacebookAsync()
        {
            try
            {

                if (_facebookService.IsLoggedIn)
                {
                    _facebookService.Logout();
                }

                EventHandler<FBEventArgs<string>> userDataDelegate = null;

                userDataDelegate = async (object sender, FBEventArgs<string> e) =>
                {
                    if (e == null) return;

                    switch (e.Status)
                    {
                        case FacebookActionStatus.Completed:
                            var facebookProfile = await Task.Run(() => JsonConvert.DeserializeObject<FacebookProfile>(e.Data));
                            var socialLoginData = new NetworkAuthData
                            {
                                Email = facebookProfile.Email,
                                Name = $"{facebookProfile.FirstName} {facebookProfile.LastName}",
                                Id = facebookProfile.UserId
                            };
                            await App.Current.MainPage.Navigation.PushModalAsync(new AboutPage());
                            break;
                        case FacebookActionStatus.Canceled:
                            break;
                    }

                    _facebookService.OnUserData -= userDataDelegate;
                };

                _facebookService.OnUserData += userDataDelegate;

                string[] fbRequestFields = { "email", "first_name", "gender", "last_name" };
                string[] fbPermisions = { "email" };
                await _facebookService.RequestUserDataAsync(fbRequestFields, fbPermisions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
