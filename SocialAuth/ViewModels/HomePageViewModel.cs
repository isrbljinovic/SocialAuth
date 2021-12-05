using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using IF.Lastfm.Core.Api;
using Newtonsoft.Json;
using SocialAuth.Models;
using SocialAuth.Services;
using Xamarin.Forms;

namespace SocialAuth.ViewModels
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        public ICommand OnGetArtistInfoCommand { get; set; }
        public ICommand OnGetTrafficInfoCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private LastFmService _lastFmService = new LastFmService();
        private MapQuestService _mapQuestService = new MapQuestService();

        private void HandlePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region UserInfo

        private string _userFullName;

        public string UserFullName
        {
            get { return _userFullName; }
            set { _userFullName = value; HandlePropertyChanged(); }
        }

        private string _userPicture;

        public string UserPicture
        {
            get { return _userPicture; }
            set { _userPicture = value; HandlePropertyChanged(); }
        }

        #endregion UserInfo

        #region ArtistInfo

        private string _artistName;

        public string ArtistName
        {
            get { return _artistName; }
            set { _artistName = value; HandlePropertyChanged(); }
        }

        private string _artistBio;

        public string ArtistBio
        {
            get { return _artistBio; }
            set { _artistBio = value; HandlePropertyChanged(); }
        }

        private string _artistPicture;

        public string ArtistPicture
        {
            get { return _artistPicture; }
            set { _artistPicture = value; HandlePropertyChanged(); }
        }

        #endregion ArtistInfo

        #region TrafficIncidents

        private List<string> _trafficIncidents;

        public List<string> TrafficIncidents
        {
            get { return _trafficIncidents; }
            set { _trafficIncidents = value; HandlePropertyChanged(); }
        }

        #endregion TrafficIncidents

        public HomePageViewModel(UserProfile userProfile)
        {
            UserFullName = userProfile.Name;
            UserPicture = userProfile.Picture;
            TrafficIncidents = new List<string>();
            OnGetArtistInfoCommand = new Command(async () => await GetArtistData());
            OnGetTrafficInfoCommand = new Command(() => GetTrafficInfo());
        }

        public async Task GetArtistData()
        {
            var client = new LastfmClient(ApiKeys.LastfmApiKey, ApiKeys.LastfmApiSecret);
            var artistInfo = (await client.Artist.GetInfoAsync(ArtistName)).Content;

            var artist = new Artist(artistInfo.Mbid, artistInfo.Name, artistInfo.Bio.Summary, artistInfo.MainImage.Medium.AbsoluteUri);

            //store to DB
            try
            {
                _lastFmService.CreateArtist(artist);
            }
            catch (Exception e)
            {
            }

            ArtistName = artistInfo.Name;
            ArtistBio = artistInfo.Bio.Summary;
            ArtistPicture = artistInfo.MainImage.Small.AbsoluteUri.ToString();
        }

        private void GetTrafficInfo()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = $"https://www.mapquestapi.com/traffic/v2/incidents?key=NwAiRaW52GEDYI6PjMAdlo0dwIEudifi&boundingBox=29.95,-105.25,39.52,-104.71&filters=construction,incidents";
                    var response = httpClient.GetStringAsync(new Uri(url)).Result;

                    Traffic traffic = JsonConvert.DeserializeObject<Traffic>(response);

                    TrafficIncidents.Clear();
                    List<string> novi = new List<string>();

                    foreach (var item in traffic.incidents)
                    {
                        var incident = new TrafficIncident(item.id, item.shortDesc, item.fullDesc);

                        try
                        {
                            _mapQuestService.CreateTrafficIncident(incident);
                        }
                        catch (Exception e)
                        {
                        }
                        novi.Add(item.shortDesc);
                    }

                    TrafficIncidents = novi;
                    HandlePropertyChanged("TrafficIncidents");
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}