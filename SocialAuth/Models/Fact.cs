using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace SocialAuth.Models
{
    public class Fact : INotifyPropertyChanged
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        [BsonIgnoreIfDefault]
        public Guid Id { get; set; }

        private string _fact;

        [BsonElement("fact")]
        public string FactText
        {
            get => _fact;
            set
            {
                if (_fact == value)
                    return;

                _fact = value;

                HandlePropertyChanged();
            }
        }

        private long _length;

        [BsonElement("length")]
        public long Length
        {
            get => _length;
            set
            {
                if (_length == value)
                    return;

                _length = value;

                HandlePropertyChanged();
            }
        }

        public Fact(string fact, long length)
        {
            FactText = fact;
            Length = length;
        }

        private void HandlePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}