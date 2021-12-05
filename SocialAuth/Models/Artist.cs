using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace SocialAuth.Models
{
    public class Artist : INotifyPropertyChanged
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        [BsonIgnoreIfDefault]
        public Guid Id { get; set; }

        private string _artistId;

        [BsonElement("artistId")]
        public string ArtistId
        {
            get => _artistId;
            set
            {
                if (_artistId == value)
                    return;

                _artistId = value;

                HandlePropertyChanged();
            }
        }

        private string _name;

        [BsonElement("name")]
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                    return;

                _name = value;

                HandlePropertyChanged();
            }
        }

        private string _bio;

        [BsonElement("bio")]
        public string Bio
        {
            get => _bio;
            set
            {
                if (_bio == value)
                    return;

                _bio = value;

                HandlePropertyChanged();
            }
        }

        private string _picture;

        [BsonElement("picture")]
        public string Picture
        {
            get => _picture;
            set
            {
                if (_picture == value)
                    return;

                _picture = value;

                HandlePropertyChanged();
            }
        }

        public Artist(string artistId, string name, string bio, string picture)
        {
            ArtistId = artistId;
            Name = name;
            Bio = bio;
            Picture = picture;
        }

        private void HandlePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}