using System.Windows.Media.Imaging;
using ClientApp.Models;
using ClientApp.Utility;

namespace ClientApp.ViewModels
{
    internal class CardVM : Observable
    {
        private InfoCard _infoCard;
        private bool _checked;

        public CardVM(InfoCard infoCard)
        {
            _infoCard = infoCard;
            OnPropertyChanged(nameof(Label));
            OnPropertyChanged(nameof(ImageSource));

        }

        public InfoCard InfoCard
        {
            get { return _infoCard; }
            set
            {
                _infoCard = value;
                OnPropertyChanged(nameof(InfoCard));
            }
        }

        public string Label
        {
            get { return _infoCard.Label; }
            set
            {
                _infoCard.Label = value;
                OnPropertyChanged(nameof(Label));
            }
        }

        public byte[] ImageBytes
        {
            get { return _infoCard.ImageBytes; }
            set
            {
                _infoCard.ImageBytes = value;
                OnPropertyChanged(nameof(ImageBytes));
                OnPropertyChanged(nameof(ImageSource));
            }
            
        }


        public BitmapFrame ImageSource
        {
            get
            {
                if (_infoCard?.ImageBytes != null)
                    return ImageConverter.FromByteArray(_infoCard.ImageBytes);
                else return null;
            }
            set
            {
                _infoCard.ImageBytes = ImageConverter.CompressToByteArray(value);
                OnPropertyChanged(nameof(ImageBytes));
                OnPropertyChanged(nameof(ImageSource));
            }
        }
        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                OnPropertyChanged(nameof(Checked));
            }
        }

    }
}
