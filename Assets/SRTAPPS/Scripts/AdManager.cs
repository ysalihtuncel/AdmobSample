using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System;

namespace SRTAPPS.AdmobAdManager
{
    public class AdManager : MonoBehaviour
    {
        //For Singleton
        public static AdManager Instance;
        public event Action<string> onAdReward;
        [Header("Ad Manager Config")]
        [SerializeField] AdManagerConfig AMC;
        BannerView _bannerView;
        InterstitialAd _interstitialAd;
        RewardedAd _rewardedAd;
        bool waitGDPR = true;

        #region AD IDS
        private const string BANNER_TEST_ID = "ca-app-pub-3940256099942544/6300978111";
        private const string INTERSTITAL_TEST_ID = "ca-app-pub-3940256099942544/1033173712";
        private const string REWARDED_TEST_ID = "ca-app-pub-3940256099942544/5224354917";
        #endregion

        void Awake()
        {
            if (Instance == null)
            {
                if (AMC == null)
                {
                    try
                    {
                        AMC = Resources.Load<AdManagerConfig>("AdManagerConfig");
                    }
                    catch { LogError("Reklamlar Başlatılamadı! AdManagerConfig bulunamadı. Inspector ekranından ekleyin."); return; }
                }
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                AMC.TestMode = true;
#else
                AMC.BannerEnabled = string.IsNullOrEmpty(AMC.BannerAdID);
                AMC.InterstitialEnabled = string.IsNullOrEmpty(AMC.InterstitialAdID);
                AMC.RewardedEnabled = string.IsNullOrEmpty(AMC.RewardedAdID);
                AMC.TestMode = false;
#endif
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
                GPDR_Initialize();
                StartCoroutine(Initialize());

            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        void GPDR_Initialize()
        {

            if (AMC.GPDR == GPDR_MODE.DevelopmentMode)
            {
                waitGDPR = false;
                return;
            }

            var debugSettings = new ConsentDebugSettings
            {
                // Geography appears as in EEA for debug devices.
                DebugGeography = DebugGeography.EEA,
                TestDeviceHashedIds = new List<string>
                {
                    AdMobUtility.GetTestDeviceId()
                }
            };
            // Set tag for under age of consent.
            // Here false means users are not under age.
            ConsentRequestParameters request = AMC.GPDR == GPDR_MODE.DevelopmentTestMode ?
            new ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = false,
                ConsentDebugSettings = debugSettings,
            } :
            new ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = false,
            };

            // Check the current consent information status.
            ConsentInformation.Update(request, OnConsentInfoUpdated);
        }

        void OnConsentInfoUpdated(FormError error)
        {
            if (error != null)
            {
                // Handle the error.
                UnityEngine.Debug.LogError(error);
                waitGDPR = false;
                return;
            }

            // If the error is null, the consent information state was updated.
            // You are now ready to check if a form is available.
            if (ConsentInformation.IsConsentFormAvailable())
            {
                LoadConsentForm();
            }
            else
            {
                waitGDPR = false;
            }
        }

        private ConsentForm _consentForm;

        void LoadConsentForm()
        {
            // Loads a consent form.
            ConsentForm.Load(OnLoadConsentForm);
        }

        void OnLoadConsentForm(ConsentForm consentForm, FormError error)
        {
            if (error != null)
            {
                waitGDPR = false;
                // Handle the error.
                UnityEngine.Debug.LogError(error);
                return;
            }

            // The consent form was loaded.
            // Save the consent form for future requests.
            _consentForm = consentForm;

            // You are now ready to show the form.
            if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
            {
                _consentForm.Show(OnShowForm);
                return;
            }
            waitGDPR = false;
        }

        void OnShowForm(FormError error)
        {
            if (error != null)
            {
                // Handle the error.
                UnityEngine.Debug.LogError(error);
                waitGDPR = false;
                return;
            }

            // Handle dismissal by reloading form.
            waitGDPR = false;
        }

        IEnumerator Initialize()
        {
            while (waitGDPR)
            {
                yield return null;
            }
            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(initStatus =>
            {
                Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
                foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
                {
                    AdapterStatus status = keyValuePair.Value;
                    switch (status.InitializationState)
                    {
                        case AdapterState.Ready:
                            LoadBannerAd();
                            LoadInterstitialAd();
                            LoadRewardedAd();
                            break;
                    }
                }
            });
        }
        AdRequest GetAdRequest()
        {
            return new AdRequest();
        }

        #region BANNER
        public void CreateBannerView()
        {
            Log("Creating banner view");
            // If we already have a banner, destroy the old one.
            if (_bannerView != null)
            {
                DestroyBannerView();
            }

            // Create a 320x50 banner at top of the screen
            // C# one line if
            _bannerView = new BannerView(AMC.TestMode ? BANNER_TEST_ID : AMC.BannerAdID, AdSize.Banner, AMC.BannerPosition);
            ListenToBannerAdEvents();
        }

        public void LoadBannerAd()
        {
            if (!AMC.BannerEnabled) return;
            // create an instance of a banner view first.
            DestroyBannerView();
            if (_bannerView == null)
            {
                CreateBannerView();
            }

            // send the request to load the ad.
            Log("Loading banner ad.");
            _bannerView.LoadAd(GetAdRequest());
        }

        /// <summary>
        /// Destroys the banner view.
        /// </summary>
        public void DestroyBannerView()
        {
            if (_bannerView != null)
            {
                Log("Destroying banner view.");
                _bannerView.Destroy();
                _bannerView = null;
            }
        }

        /// <summary>
        /// listen to events the banner view may raise.
        /// </summary>
        private void ListenToBannerAdEvents()
        {
            // Raised when an ad is loaded into the banner view.
            _bannerView.OnBannerAdLoaded += () =>
            {
                Log("Banner view loaded an ad with response : "
                    + _bannerView.GetResponseInfo());
            };
            // Raised when an ad fails to load into the banner view.
            _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
            {
                LogError("Banner view failed to load an ad with error : "
                    + error);
            };
            // Raised when the ad is estimated to have earned money.
            _bannerView.OnAdPaid += (AdValue adValue) =>
            {
                Log($"Banner view paid {adValue.Value} {adValue.CurrencyCode}.");
            };
            // Raised when an impression is recorded for an ad.
            _bannerView.OnAdImpressionRecorded += () =>
            {
                Log("Banner view recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            _bannerView.OnAdClicked += () =>
            {
                Log("Banner view was clicked.");
            };
            // Raised when an ad opened full screen content.
            _bannerView.OnAdFullScreenContentOpened += () =>
            {
                Log("Banner view full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            _bannerView.OnAdFullScreenContentClosed += () =>
            {
                Log("Banner view full screen content closed.");
            };
        }
        #endregion

        #region Interstitial
        /// <summary>
        /// Loads the interstitial ad.
        /// </summary>
        public void LoadInterstitialAd()
        {
            if (!AMC.InterstitialEnabled) return;
            // Clean up the old ad before loading a new one.
            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }

            Log("Loading the interstitial ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            InterstitialAd.Load(AMC.TestMode ? INTERSTITAL_TEST_ID : AMC.InterstitialAdID, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        LogError("interstitial ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Log("Interstitial ad loaded with response : "
                              + ad.GetResponseInfo());

                    _interstitialAd = ad;
                    RegisterEventHandlers(_interstitialAd);
                });
        }

        /// <summary>
        /// Shows the interstitial ad.
        /// </summary>
        public void ShowInterstitialAd()
        {
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                Log("Showing interstitial ad.");
                _interstitialAd.Show();
            }
            else
            {
                LogError("Interstitial ad is not ready yet.");
            }
        }

        private void RegisterEventHandlers(InterstitialAd interstitialAd)
        {
            // Raised when the ad is estimated to have earned money.
            interstitialAd.OnAdPaid += (AdValue adValue) =>
            {
                Log(string.Format("Interstitial ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            interstitialAd.OnAdImpressionRecorded += () =>
            {
                Log("Interstitial ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            interstitialAd.OnAdClicked += () =>
            {
                Log("Interstitial ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            interstitialAd.OnAdFullScreenContentOpened += () =>
            {
                Log("Interstitial ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Log("Interstitial ad full screen content closed.");
                LoadInterstitialAd();
            };
            // Raised when the ad failed to open full screen content.
            interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
            {
                LogError("Interstitial ad failed to open full screen content " +
                               "with error : " + error);
                LoadInterstitialAd();
            };
        }
        #endregion

        #region REWARDED
        /// <summary>
        /// Loads the rewarded ad.
        /// </summary>
        public void LoadRewardedAd()
        {
            if (!AMC.RewardedEnabled) return;
            // Clean up the old ad before loading a new one.
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }

            Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            RewardedAd.Load(AMC.TestMode ? REWARDED_TEST_ID : AMC.RewardedAdID, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        LogError("Rewarded ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Log("Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());

                    _rewardedAd = ad;
                    RegisterEventHandlers(_rewardedAd);
                });
        }

        public void ShowRewardedAd(string rewardType)
        {
            const string rewardMsg =
                "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                _rewardedAd.Show((Reward reward) =>
                {
                    Log(string.Format(rewardMsg, reward.Type, reward.Amount));
                    UserEarnedReward(rewardType);
                });
            }
        }

        private void RegisterEventHandlers(RewardedAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Log(string.Format("Rewarded ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
            {
                Log("Rewarded ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
            {
                Log("Rewarded ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Log("Rewarded ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Log("Rewarded ad full screen content closed.");
                LoadRewardedAd();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                LogError("Rewarded ad failed to open full screen content " +
                               "with error : " + error);
                LoadRewardedAd();
            };
        }
        #endregion

        #region USER REWARD
        public void UserEarnedReward(string rewardType)
        {
            onAdReward?.Invoke(rewardType);
        }
        #endregion

        #region Log Region
        void Log<T>(T log)
        {
            if (!AMC.LogMessages) return;
            Debug.Log($"<color=cyan>{log}</color>", this);
        }
        void LogError<T>(T log)
        {
            if (!AMC.LogMessages) return;
            Debug.Log($"<color=cyan>Hata Mesajı!!</color>");
            Debug.LogError(log, this);
        }
        #endregion
    }
}