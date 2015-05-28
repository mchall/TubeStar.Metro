using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace TubeStarMetro
{
    public sealed partial class OnlineStoreControl : UserControl
    {
        public OnlineStoreControl()
        {
            InitializeComponent();
            Translate();
        }

        private void Translate()
        {
        }

        public void Update()
        {
            itemPanel.Items.Clear();
            foreach (var item in StoreItems.Current.All)
            {
                if (!item.Purchased)
                {
                    var storeBlock = new StoreItemBlock(item);
                    storeBlock.PurchaseMade += StoreBlock_PurchaseMade;
                    itemPanel.Items.Insert(0, storeBlock);
                }
            }
        }

        private void StoreBlock_PurchaseMade()
        {
            Update();
        }

        private void ItemPanel_ItemClick(object sender, ItemClickEventArgs e)
        {
            var block = e.ClickedItem as StoreItemBlock;
            if (block != null)
            {
                block.Purchase();
            }
        }
    }
}