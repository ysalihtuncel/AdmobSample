using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SRTAPPS.AdmobAdManager
{
    [CreateAssetMenu(fileName = "New Ad Manager", menuName = "SRTAPPS/Ad Manager")]
    public class AdManagerConfig : ScriptableObject
    {
        public bool TestMode = true;
        public GoogleMobileAds.Api.AdPosition BannerPosition;
        public bool BannerEnabled = true;
        public bool InterstitialEnabled = true;
        public bool RewardedEnabled = true;
        public string BannerAdID;
        public string InterstitialAdID;
        public string RewardedAdID;
        public bool LogMessages = true;
        public GPDR_MODE GPDR;
    }

    public enum GPDR_MODE { DevelopmentMode = 0, DevelopmentTestMode = 1, ProductionMode = 2, }

}
