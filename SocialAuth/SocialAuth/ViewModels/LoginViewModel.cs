﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using SocialAuth.Models;
using SocialAuth.Services;
using SocialAuth.Views;
using Xamarin.Forms;

namespace SocialAuth.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand OnLoginWithFacebookCommand { get; set; }

        private IFacebookClient _facebookService = CrossFacebookClient.Current;
        private UserProfileService _userProfileService = new UserProfileService();

        public LoginViewModel()
        {
            OnLoginWithFacebookCommand = new Command(async () => await LoginFacebookAsync());
        }

        private async Task LoginFacebookAsync()
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
                            var socialLoginData = new UserProfile(
                                facebookProfile.UserId,
                                $"{facebookProfile.FirstName} {facebookProfile.LastName}",
                                facebookProfile.Email,
                                facebookProfile.Picture.Data.Url);

                            //Insert user into DB
                            await _userProfileService.CreateUserProfile(socialLoginData);

                            var userProfileFromDb = await _userProfileService.GetUserProfileByProfileId(socialLoginData.ProfileId);

                            //Success - wellcome in
                            await App.Current.MainPage.Navigation.PushModalAsync(new HomePage());
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