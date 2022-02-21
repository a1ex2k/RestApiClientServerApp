using ClientApp.Utility;
using System;
using System.Text.Json.Serialization;

namespace ClientApp.Models
{
    internal class InfoCard : Observable
    {
        private string _id;
        private string _label;
        private byte[] _imageBytes;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                OnPropertyChanged(nameof(Label));
            }
        }
        public string Base64Image
        {
            get { return (_imageBytes != null) ? Convert.ToBase64String(_imageBytes) : string.Empty; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    _imageBytes = null;
                else
                    _imageBytes = Convert.FromBase64String(value);
                OnPropertyChanged(nameof(Base64Image));
            }
        }

        [JsonIgnore]
        public byte[] ImageBytes
        {
            get { return _imageBytes; }
            set
            {
                _imageBytes = value;
                OnPropertyChanged(nameof(ImageBytes));
            }
        }
    }
}
