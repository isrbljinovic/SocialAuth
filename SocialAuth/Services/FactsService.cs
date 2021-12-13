using MongoDB.Driver;
using SocialAuth.Models;

namespace SocialAuth.Services
{
    public class FactsService
    {
        private string _dbName = "UsersDb";
        private string _collection = "Facts";

        private IMongoCollection<Fact> _factsCollection;

        private IMongoCollection<Fact> FactsCollection
        {
            get
            {
                try
                {
                    if (_factsCollection is null)
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
                        _factsCollection = db.GetCollection<Fact>(_collection);
                    }

                    return _factsCollection;
                }
                catch (System.Exception e)
                {
                    throw;
                }
            }
        }

        //public UserProfile GetUserProfileByProfileId(string profileId)
        //{
        //    var userProfile = FactsCollection
        //        .Find(f => f.ProfileId.Equals(profileId))
        //        .FirstOrDefault();

        //    return userProfile;
        //}

        public void CreateFact(Fact fact)
        {
            try
            {
                FactsCollection.InsertOne(fact);
            }
            catch (System.Exception ec)
            {
                return;
            }
        }

        public long CountFactsLessThan100()
        {
            var number = FactsCollection.Count(t => t.Length <= 100);

            return number;
        }
    }
}