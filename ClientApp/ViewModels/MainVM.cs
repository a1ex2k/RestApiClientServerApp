using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ClientApp.Models;
using ClientApp.Utility;
using Microsoft.Win32;

namespace ClientApp.ViewModels
{
    internal class MainVM : Observable
    {
        public ObservableCollection<CardVM> Cards { get; private set; }
        private bool _isAddingMode;
        private CardVM _selectedCard;
        private CardVM _tempCard;
        private DataService.DataService _dataService;

        public CardVM TempCard
        {
            get { return _tempCard; }
            set
            {
                _tempCard = value;
                OnPropertyChanged(nameof(TempCard));
            }
        }

        public CardVM SelectedCard
        {
            get { return _selectedCard; }
            set
            {
                _selectedCard = value;
                if (value != null)
                    TempCard = new CardVM(new InfoCard() { Label = value.InfoCard.Label, ImageBytes = value.InfoCard.ImageBytes });
                else
                {
                    TempCard = null;
                }
                OnPropertyChanged(nameof(SelectedCard));
                OnPropertyChanged(nameof(CardEditIsActive));
                OnPropertyChanged(nameof(CollectionViewIsEnabled));
            }
        }

        public bool CardEditIsActive { get => _selectedCard != null; }
        public bool CollectionViewIsEnabled { get => !CardEditIsActive; }
        public MainVM()
        {
            UpdateButtonCommand = new RelayCommand(a => RefreshCollection());
            AddButtonCommand = new RelayCommand(a => AddButtonClick());
            DeleteButtonCommand = new RelayCommand(a => DeleteButtonClick());
            CardCommand = new RelayCommand(a => CardClick());
            SaveCardCommand = new RelayCommand(a => SaveCard());
            CancelCardEditCommand = new RelayCommand(a => CancelEditingCard());
            ChooseImageCommand = new RelayCommand(a => LoadImage());

            _dataService = new DataService.DataService();
            RefreshCollection();
        }

        public ICommand UpdateButtonCommand { get; set; }
        public ICommand AddButtonCommand { get; set; }
        public ICommand DeleteButtonCommand { get; set; }
        public ICommand CardCommand { get; set; }
        public ICommand SaveCardCommand { get; set; }
        public ICommand CancelCardEditCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }


        private void RefreshCollection()
        {
            var infocards = _dataService.GetInfoCards();
            Cards = new ObservableCollection<CardVM>(infocards.Select(card => new CardVM(card)));
            OnPropertyChanged(nameof(Cards));
        }
        private void AddButtonClick()
        {
            SelectedCard = new CardVM(new InfoCard());
            _isAddingMode = true;

        }
        private void DeleteButtonClick()
        {
            var toRemove = Cards.Where(x => x.Checked).ToList();
            foreach (var card in toRemove)
            {
                _dataService.DeleteInfoCard(card.InfoCard);
                Cards.Remove(card);
            }
        }

        private void CardClick()
        {
            Cards.Add(new CardVM(new InfoCard()));
        }

        private void SaveCard()
        {
            if (_tempCard.Label != null && _tempCard.Label.Trim() != string.Empty && _tempCard.InfoCard.ImageBytes != null)
            {
                try
                {
                    if (_isAddingMode)
                    {
                        Cards.Insert(0, _tempCard);
                        _dataService.PostInfoCard(_tempCard.InfoCard);
                    }
                    else
                    {
                        int index = Cards.IndexOf(SelectedCard);
                        Cards[index].Label = _tempCard.InfoCard.Label;
                        Cards[index].ImageBytes = _tempCard.ImageBytes;
                        _dataService.PutInfoCard(Cards[index].InfoCard);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.Message);
                    MessageBox.Show("Failed to save the card", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                SelectedCard = null;
                _isAddingMode = false;
            }
            else
            {
                MessageBox.Show("Name and Image cannot be empty", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        private void CancelEditingCard()
        {
            SelectedCard = null;
        }

        private void LoadImage()
        {
            var d = new OpenFileDialog()
            {
                Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp",
            };
            if (d.ShowDialog() == true)
            {
                try
                {
                    _tempCard.ImageSource = ImageConverter.FromFile(d.FileName);
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.Message);
                    MessageBox.Show("Failed to load chosen image", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }
}
