using System;
using System.Text.Json.Serialization;

namespace ApiService.Models
{
    public class InfoCard
    {
        private string _id;
        private string _label;
        private byte[] _imageBytes = null;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }


        public string Base64Image
        {
            get { return (_imageBytes != null) ? Convert.ToBase64String(_imageBytes) : string.Empty; }
            set { _imageBytes = Convert.FromBase64String(value); }
        }
        
        [JsonIgnore]
        public byte[] ImageBytes
        {
            get { return _imageBytes; }
            set { _imageBytes = value; }
        }
    }
}