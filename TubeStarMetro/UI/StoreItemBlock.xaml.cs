using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace TubeStarMetro
{
    public sealed partial class StoreItemBlock : UserControl
    {
        public event Action PurchaseMade;

        private StoreItem _storeItem;
        public StoreItem StoreItem
        {
            get { return _storeItem; }
            private set
            {
                _storeItem = value;
                if (_storeItem != null)
                {
                    txtName.Text = _storeItem.Name;
                    txtDescription.Text = _storeItem.Description;
                    txtPrice.Text = _storeItem.Cost.ToCurrencyString();
                    this.IsEnabled = _storeItem.Purchased ? false : true;
                    imgItem.Source = new BitmapImage(new Uri(String.Format("ms-appx:/Assets/StoreItems/{0}.jpg", _storeItem.ImageName)));
                }
            }
        }

        public StoreItemBlock(StoreItem item)
        {
            InitializeComponent();
            StoreItem = item;
        }

        public void Purchase()
        {
            if (Player.Current.Money - _storeItem.Cost < 0)
            {
                CustomMessageBox.ShowDialog(EnglishStrings.LowCashHeader.Translate(), EnglishStrings.LowCashMessage.Translate(), MessagePicture.Money);
                return;
            }

            CustomMessageBox.ShowDialog(EnglishStrings.Buy.Translate(), String.Format(EnglishStrings.BuyItem.Translate(), _storeItem.Cost.ToCurrencyString()), MessagePicture.Question, result =>
                {
                    if (result == true)
                    {
                        Player.Current.Money -= _storeItem.Cost;
                        _storeItem.Purchased = true;

                        if (_storeItem == StoreItems.Current.Loan)
                        {
                            Player.Current.LoanPayOff = (StoreItems.Current.Loan.Payout * (100 + StoreItems.Current.Loan.Interest) / 100);
                            Player.Current.Money += StoreItems.Current.Loan.Payout;
                        }

                        if (PurchaseMade != null)
                            PurchaseMade();
                    }
                });
        }
    }
}