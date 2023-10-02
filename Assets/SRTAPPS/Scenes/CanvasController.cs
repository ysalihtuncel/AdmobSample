using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SRTAPPS.AdmobAdManager;
using TMPro;

public class CanvasController : MonoBehaviour
{
    [SerializeField] AdManagerConfig AMC;
    [SerializeField] TMP_InputField input;
    [SerializeField] TextMeshProUGUI logMessages;
    // Start is called before the first frame update
    void Awake()
    {
        try
        {
            AMC = Resources.Load<AdManagerConfig>("AdManagerConfig");
        }
        catch { Debug.LogError("AdManagerConfig bulunamadı. Inspector ekranından ekleyin."); 
        logMessages.text += $"\nAdManagerConfig bulunamadı. Inspector ekranından ekleyin.";}
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void BannerShow()
    {
        logMessages.text += $"\n{System.DateTime.Now.ToString("HH:mm")}: Banner gösterildi";
        AdManager.Instance.LoadBannerAd();
    }
    public void BannerClose()
    {
        logMessages.text += $"\n{System.DateTime.Now.ToString("HH:mm")}: Banner kapatıldı";
        AdManager.Instance.DestroyBannerView();
    }

    public void InterstitialShow()
    {
        logMessages.text += $"\n{System.DateTime.Now.ToString("HH:mm")}: Interstital gösterildi";
        AdManager.Instance.ShowInterstitialAd();
    }
    public void RewardedShow()
    {
        logMessages.text += $"\n{System.DateTime.Now.ToString("HH:mm")}: Rewarded gösterildi. Ödül: {input.text}";
        AdManager.Instance.ShowRewardedAd(input.text);
    }

    void UserEarnedReward(string reward)
    {
        //Hangi ödül olduğu mantığını burada tanımlamak gerekiyor
        logMessages.text += $"\n{System.DateTime.Now.ToString("HH:mm")}: Rewarded öldülü verildi. Ödül: {reward}";
    }

    //Event yardımı ile ödüllü reklam çalıştırılıyor
    private void OnEnable() {
        AdManager.Instance.onAdReward += UserEarnedReward;
    }
    private void OnDisable() {
        AdManager.Instance.onAdReward -= UserEarnedReward;
    }


}
