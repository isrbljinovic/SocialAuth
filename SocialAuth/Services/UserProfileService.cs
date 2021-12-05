using System.Threading.Tasks;
using MongoDB.Driver;
using SocialAuth.Models;

namespace SocialAuth.Services
{
    public class UserProfileService
    {
        private string _dbName = "UsersDb";
        private string _collection = "UserProfiles";

        private IMongoCollection<UserProfile> _usersCollection;

        private IMongoCollection<UserProfile> UsersCollection
        {
            get
            {
                try
                {
                    if (_usersCollection is null)
                    {
                        //var mongoUrl = new MongoUrl(ApiKeys.DbConnectionString);

                        // APIKeys.Connection string is found in the portal under the "Connection String" blade
                        MongoClientSettings settings = MongoClientSettings.FromConnectionString(
                          ApiKeys.DbConnectionString
                        );

                        settings.SslSettings.CheckCertificateRevocation = false;
                        settings.SslSettings.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                        settings.VerifySslCertificate = false;
                        settings.AllowInsecureTls = true;

                        settings.ConnectTimeout = System.TimeSpan.FromSeconds(15);
                        settings.RetryWrites = false;

                        // Initialize the client
                        var mongoClient = new MongoClient(settings);

                        // This will create or get the database
                        var db = mongoClient.GetDatabase(_dbName);

                        // This will create or get the collection
                        var collectionSettings = new MongoCollectionSettings { ReadPreference = ReadPreference.Nearest };
                        _usersCollection = db.GetCollection<UserProfile>(_collection);
                    }

                    return _usersCollection;
                }
                catch (System.Exception e)
                {
                    throw;
                }
            }
        }

        public UserProfile GetUserProfileByProfileId(string profileId)
        {
            var userProfile = UsersCollection
                .Find(f => f.ProfileId.Equals(profileId))
                .FirstOrDefault();

            return userProfile;
        }

        public void CreateUserProfile(UserProfile userProfile)
        {
            try
            {
                var existingUserProfile = GetUserProfileByProfileId(userProfile.ProfileId);
                if (existingUserProfile == null)
                {
                    UsersCollection.InsertOne(userProfile);
                }
                else
                {
                    UpdateUserProfile(userProfile);
                }
            }
            catch (System.Exception ec)
            {
                return;
            }
        }

        public void UpdateUserProfile(UserProfile userProfile)
        {
            UsersCollection.ReplaceOne(t => t.ProfileId.Equals(userProfile.ProfileId), userProfile);
        }
    }
}