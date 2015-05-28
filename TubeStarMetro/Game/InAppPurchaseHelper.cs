using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;

namespace TubeStarMetro
{
    public enum InAppPurchase
    {
        RemoveAds = 1,
        BornToRule = 2,
    }

    public static class InAppPurchaseHelper
    {
        public static event Action RefreshEvent;

        public static bool HasPurchase(InAppPurchase purchase)
        {
#if DEBUG
            return true;
#endif
            if (CurrentApp.LicenseInformation.ProductLicenses.ContainsKey(purchase.ToString()))
            {
                if (CurrentApp.LicenseInformation.ProductLicenses[purchase.ToString()].IsActive)
                    return true;
            }
            return false;
        }

        public static async Task BuyFeature(InAppPurchase purchase)
        {
            try
            {
                if (!CurrentApp.LicenseInformation.ProductLicenses.ContainsKey(purchase.ToString()) || !CurrentApp.LicenseInformation.ProductLicenses[purchase.ToString()].IsActive)
                {
                    await CurrentApp.RequestProductPurchaseAsync(purchase.ToString(), false);
                }
            }
            catch { }
        }

        public static async Task<Dictionary<InAppPurchase, string>> FetchInAppPurchaseData()
        {
            try
            {
                Dictionary<InAppPurchase, string> storeData = new Dictionary<InAppPurchase, string>();
                ListingInformation listing = await CurrentApp.LoadListingInformationAsync();
                storeData[InAppPurchase.RemoveAds] = listing.ProductListings[InAppPurchase.RemoveAds.ToString()].FormattedPrice;
                storeData[InAppPurchase.BornToRule] = listing.ProductListings[InAppPurchase.BornToRule.ToString()].FormattedPrice;
                return storeData;
            }
            catch
            {
                return null;
            }            
        }

        public static void Refresh()
        {
            if (RefreshEvent != null)
                RefreshEvent();
        }
    }
}