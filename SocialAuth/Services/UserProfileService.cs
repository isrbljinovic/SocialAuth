using System.Security.Authentication;
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
                if (_usersCollection is null)
                {
                    var mongoUrl = new MongoUrl(ApiKeys.DbConnectionString);

                    // APIKeys.Connection string is found in the portal under the "Connection String" blade
                    MongoClientSettings settings = MongoClientSettings.FromUrl(
                      mongoUrl
                    );

                    settings.SslSettings =
                        new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

                    settings.RetryWrites = false;

                    // Initialize the client
                    var mongoClient = new MongoClient(settings);

                    // This will create or get the database
                    var db = mongoClient.GetDatabase(_dbName);

                    // This will create or get the collection
                    var collectionSettings = new MongoCollectionSettings { ReadPreference = ReadPreference.Nearest };
                    _usersCollection = db.GetCollection<UserProfile>(_collection, collectionSettings);
                }

                return _usersCollection;
            }
        }

        public async Task<UserProfile> GetUserProfileByProfileId(string profileId)
        {
            var userProfile = await UsersCollection
                .Find(f => f.ProfileId.Equals(profileId))
                .FirstOrDefaultAsync();

            return userProfile;
        }

        public async Task CreateUserProfile(UserProfile userProfile)
        {
            var existingUserProfile = await GetUserProfileByProfileId(userProfile.ProfileId);
            if (existingUserProfile == null)
            {
                await UsersCollection.InsertOneAsync(userProfile);
            }
            else
            {
                await UpdateUserProfile(userProfile);
            }
        }

        public async Task UpdateUserProfile(UserProfile userProfile)
        {
            await UsersCollection.ReplaceOneAsync(t => t.ProfileId.Equals(userProfile.ProfileId), userProfile);
        }
    }
}