using System.Security.Authentication;
using System.Threading.Tasks;
using MongoDB.Driver;
using SocialAuth.Models;

namespace SocialAuth.Services
{
    public class LastFmService
    {
        private string _dbName = "UsersDb";
        private string _collectionName = "Artists";

        #region Properties

        private IMongoCollection<Artist> _lastFmArtistsCollection;

        public IMongoCollection<Artist> ArtistsCollection
        {
            get
            {
                if (_lastFmArtistsCollection == null)
                {
                    var mongoUrl = new MongoUrl(ApiKeys.DbConnectionString);

                    // APIKeys.Connection string is found in the portal under the "Connection String" blade
                    MongoClientSettings settings = MongoClientSettings.FromUrl(
                      mongoUrl
                    );

                    settings.SslSettings.CheckCertificateRevocation = false;
                    settings.SslSettings.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                    settings.VerifySslCertificate = false;
                    settings.AllowInsecureTls = true;

                    settings.ConnectTimeout = System.TimeSpan.FromSeconds(15);

                    // Initialize the client
                    var mongoClient = new MongoClient(settings);

                    // This will create or get the database
                    var db = mongoClient.GetDatabase(_dbName);

                    // This will create or get the collection
                    var collectionSettings = new MongoCollectionSettings { ReadPreference = ReadPreference.Nearest };
                    _lastFmArtistsCollection = db.GetCollection<Artist>(_collectionName, collectionSettings);
                }
                return _lastFmArtistsCollection;
            }
        }

        #endregion Properties

        public Artist GetArtistByArtistId(string artistId)
        {
            var lastfmArtist = ArtistsCollection
                .Find(f => f.ArtistId.Equals(artistId))
                .FirstOrDefault();

            return lastfmArtist;
        }

        public void CreateArtist(Artist lastfmArtist)
        {
            var existingArtist = GetArtistByArtistId(lastfmArtist.ArtistId);
            if (existingArtist == null)
            {
                ArtistsCollection.InsertOne(lastfmArtist);
            }
            else
            {
                UpdateArtist(lastfmArtist);
            }
        }

        public void UpdateArtist(Artist lastfmArtist)
        {
            ArtistsCollection.ReplaceOne(t => t.ArtistId.Equals(lastfmArtist.ArtistId), lastfmArtist);
        }
    }
}