using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using MongoDB.Driver;
using SocialAuth.Models;

namespace SocialAuth.Services
{
    public class MapQuestService
    {
        private string _dbName = "UsersDb";
        private string _collectionName = "TrafficIncidents";

        #region Properties

        private IMongoCollection<TrafficIncident> _trafficIncidentsCollection;

        public IMongoCollection<TrafficIncident> TrafficIncidentsCollection
        {
            get
            {
                if (_trafficIncidentsCollection == null)
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
                    _trafficIncidentsCollection = db.GetCollection<TrafficIncident>(_collectionName, collectionSettings);
                }
                return _trafficIncidentsCollection;
            }
        }

        #endregion Properties

        public TrafficIncident GetTrafficIncidentByIncidentId(string incidentId)
        {
            var trafficIncident = TrafficIncidentsCollection
                .Find(f => f.IncidentId.Equals(incidentId))
                .FirstOrDefault();

            return trafficIncident;
        }

        public void CreateTrafficIncident(TrafficIncident trafficIncident)
        {
            var existingTrafficIncident = GetTrafficIncidentByIncidentId(trafficIncident.IncidentId);
            if (existingTrafficIncident == null)
            {
                TrafficIncidentsCollection.InsertOne(trafficIncident);
            }
            else
            {
                UpdateTrafficIncident(trafficIncident);
            }
        }

        public void UpdateTrafficIncident(TrafficIncident trafficIncident)
        {
            TrafficIncidentsCollection.ReplaceOne(t => t.IncidentId.Equals(trafficIncident.IncidentId), trafficIncident);
        }
    }
}