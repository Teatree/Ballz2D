using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class Purchaser : MonoBehaviour, IStoreListener
{

    public static Purchaser purchaser { get; set; }
    public List<ShopOffer> offers;

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    public static string GEMS_200 = "100_gems";
    public static string GEMS_400 = "400_gems";
    public static string GEMS_600 = "600_gems";
    public static string GEMS_1100 = "1100_gems";
    public static string GEMS_2300 = "2300_gems";
    public static string GEMS_7200 = "7200_gems";
    public static string GEMS_12500 = "12500_gems";
    public static string GEMS_30000 = "30000_gems";
    public static string NO_ADS = "no_ads";

    public static string STARTER_PACK = "pack_starter";
    public static string WEEKEND_PACK = "pack_weekend";
    public static string STATIC_PACK = "pack_static";

    public static string SPECIAL_PACK_1 = "pack_special_1";
    public static string SPECIAL_PACK_2 = "pack_special_2";
    public static string SPECIAL_PACK_3 = "pack_special_3";

    public static string WEEK_SUB = "week_sub";
    public static string MONTH_SUB = "month_sub";
    public static string YEAR_SUB = "year_sub";

    public void Awake()
    {
        purchaser = this;
    }

    void Start()
    {
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(GEMS_200, ProductType.Consumable);
        builder.AddProduct(GEMS_400, ProductType.Consumable);
        builder.AddProduct(GEMS_600, ProductType.Consumable);
        builder.AddProduct(GEMS_1100, ProductType.Consumable);
        builder.AddProduct(GEMS_2300, ProductType.Consumable);
        builder.AddProduct(GEMS_7200, ProductType.Consumable);
        builder.AddProduct(GEMS_12500, ProductType.Consumable);
        builder.AddProduct(GEMS_30000, ProductType.Consumable);
        builder.AddProduct(NO_ADS, ProductType.NonConsumable);

        builder.AddProduct(STARTER_PACK, ProductType.Consumable);
        builder.AddProduct(WEEKEND_PACK, ProductType.Consumable);
        builder.AddProduct(STATIC_PACK, ProductType.Consumable);
        builder.AddProduct(SPECIAL_PACK_1, ProductType.Consumable);
        builder.AddProduct(SPECIAL_PACK_2, ProductType.Consumable);
        builder.AddProduct(SPECIAL_PACK_3, ProductType.Consumable);

        builder.AddProduct(WEEK_SUB, ProductType.Subscription);
        builder.AddProduct(MONTH_SUB, ProductType.Subscription);
        builder.AddProduct(YEAR_SUB, ProductType.Subscription);

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyGems200()
    {
        BuyProductID(GEMS_200);
    }

    public void BuyGems400()
    {
        BuyProductID(GEMS_400);
    }

    public void BuyGems600()
    {
        BuyProductID(GEMS_600);
    }

    public void BuyGems1100()
    {
        BuyProductID(GEMS_1100);
    }

    public void BuyGems2300()
    {
        BuyProductID(GEMS_2300);
    }

    public void BuyGems7200()
    {
        BuyProductID(GEMS_7200);
    }

    public void BuyGems12500()
    {
        BuyProductID(GEMS_12500);
    }

    public void BuyGems30000()
    {
        BuyProductID(GEMS_30000);
    }

    public void BuyNoAds()
    {
        BuyProductID(NO_ADS);
    }

    public void BuySpecialPack_1()
    {
        BuyProductID(SPECIAL_PACK_1);
    }

    public void BuySpecialPack_2()
    {
        BuyProductID(SPECIAL_PACK_2);
    }
    public void BuySpecialPack_3()
    {
        BuyProductID(SPECIAL_PACK_3);
    }

    public void BuyStarterPack()
    {
        BuyProductID(STARTER_PACK);
    }

    public void BuyWeekendPack()
    {
        BuyProductID(WEEKEND_PACK);
    }
    public void BuyStaticPack()
    {
        BuyProductID(STATIC_PACK);
    }

    public void BuyWeekSub()
    {
        BuyProductID(WEEK_SUB);
    }
    public void BuyMonthSub()
    {
        BuyProductID(MONTH_SUB);
    }
    public void BuyYearSub()
    {
        BuyProductID(YEAR_SUB);
    }


    public string GetLocalPrice(string prodId)
    {
        return m_StoreController.products.WithID(prodId).metadata.localizedPriceString;
    }

    public void buyOffer(ShopOffer o)
    {
        BuyProductID(o.id);
    }

    private void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }


    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) =>
            {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }


    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {

        String prodId = args.purchasedProduct.definition.id;
        String[] prodVal = prodId.Split("_".ToCharArray());
        if (prodVal.Length > 1 && prodVal[1].Equals("gems", StringComparison.CurrentCultureIgnoreCase))
        {
            PlayerController.player.gems += int.Parse(prodVal[0]);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, NO_ADS, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            PlayerController.player.noAds = true;
        }
        else if (prodVal.Length > 1 && prodVal[0].Contains("pack"))
        {
            foreach (ShopOffer s in offers)
            {
                Debug.Log(">>>> check > " + s.id + " >? " + prodId);
                if (prodId.Equals(s.id, StringComparison.CurrentCultureIgnoreCase))
                {
                    s.GivePlayerStuff();
                    break;
                }
            }
        }

        else if (String.Equals(args.purchasedProduct.definition.id, WEEK_SUB, StringComparison.Ordinal))
        {

        }
        else if (String.Equals(args.purchasedProduct.definition.id, MONTH_SUB, StringComparison.Ordinal))
        {

        }
        else if (String.Equals(args.purchasedProduct.definition.id, YEAR_SUB, StringComparison.Ordinal))
        {

        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }


}