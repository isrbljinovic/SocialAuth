using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace SocialAuth.Models
{
    public class TrafficIncident : INotifyPropertyChanged
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        [BsonIgnoreIfDefault]
        public Guid Id { get; set; }

        private string _incidentId;

        [BsonElement("incidentId")]
        public string IncidentId
        {
            get => _incidentId;
            set
            {
                if (_incidentId == value)
                    return;

                _incidentId = value;

                HandlePropertyChanged();
            }
        }

        private string _shortDesc;

        [BsonElement("shortDesc")]
        public string ShortDesc
        {
            get => _shortDesc;
            set
            {
                if (_shortDesc == value)
                    return;

                _shortDesc = value;

                HandlePropertyChanged();
            }
        }

        private string _fullDesc;

        [BsonElement("fullDesc")]
        public string FullDesc
        {
            get => _fullDesc;
            set
            {
                if (_fullDesc == value)
                    return;

                _fullDesc = value;

                HandlePropertyChanged();
            }
        }

        public TrafficIncident(string incidentId, string shortDesc, string fullDesc)
        {
            IncidentId = incidentId;
            ShortDesc = shortDesc;
            FullDesc = fullDesc;
        }

        private void HandlePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}