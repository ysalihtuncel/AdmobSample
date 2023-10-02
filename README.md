![Cover Image](https://github.com/ysalihtuncel/AdmobSample/tree/main/Assets/SRTAPPS/Sprites/cover.png?raw=true)

# AdMob Reklam Yönetimi
GoogleMobileAds-v8.5.1.unitypackage kullanıılmıştır.
Google'ın kullanıcı rızası yönetim çözümünü kullanmak isterseniz öncelikle GDPR mesajınızı oluşturun. UMP SDK'sı GoogleMobileAds-v8.5.1.unitypackage ile projede bulunuyor ve komut dosyası içeriğine dahil edilmiştir. 
Projede çalışması için projenizi imzalamış olmanız gerekiyor. GDPR constent form için config içerisinde ayarlar bulunuyor.

Bu proje, Unity kullanarak AdMob reklamlarını yönetmek için bir örnek sunar. Projede banner, interstitial ve rewarded reklamları nasıl kullanacağınızı öğrenebilirsiniz.
**Development build ve Unity Editor üzerinde sadece test reklamları çalışacak şekilde ayarlanmıştır. Geçersiz trafikten korunmak için projeniz bittiği zaman gerçek reklam kimliklerinizi kullanın.
Hiç bir şekilde kendi reklamlarınıza tıklamayın. Aksi taktirde _geçersiz trafik_ kısıtlaması almanız muhtemel olacaktır!**

## Config Ayarları:
Assets/SRTAPPS/Resources dizini altında bulunan **AdManagerConfig** scriptable objesi ile reklam ayarlarınızı düzenleyebilirsiniz. Reklam türlerine göre enabled/disabled hale getirebilirsiniz.
Bunun için yapmanız gerekn çok basit:
  **Banner için örnek:**
  AdManagerConfig.asset dosyasında Unity editor içinde inspector penceresinden **bannerEnabled** değerini true olarak ayarlayın.
  Banner ID'inizi girin.
  Banner pozisyonunu setleyin.
GDPR ayarlarınız için:
  **DevelopmentMode** herhangi bir şekilde formu açmaz. İşlemlerine devam eder.
  **DevelopmentTestMode** test etmek için kullanabilirsiniz.
  **ProductionMode** uygulamanızı yayınlarken production mode a almanız gerekir.

## Banner Reklamları
Eğer Banner reklamını aktif ettiyseniz otomatik olarak gösterilecektir. 
Banner reklamını başka bir script ile kaldırmak için:
   `using SRTAPPS.AdmobAdManager;
   ...
   AdManager.Instance.DestroyBannerView();`
Banner reklamını başka bir script ile göstermek için:
   `using SRTAPPS.AdmobAdManager;
   ...
   AdManager.Instance.LoadBannerAd();`
şeklinde çağırmanız yeterli olacaktır.

## Interstitial Reklamları
Interstitial reklamları kullanmak için, aşağıdaki kodu kullanabilirsiniz:
`using SRTAPPS.AdmobAdManager
...
AdManager.Instance.ShowInterstitialAd();`
Geçiş reklama kapatıldığı zaman yeni geçiş reklamı yükleme kodu çalışacaktır.

## Rewarded Reklamlar
Rewarded reklamları kullanmak için, aşağıdaki kodu kullanabilirsiniz. Ödül türünü kullanmak için onAdReward event'ine subscribe olmanız gerekiyor:
`using SRTAPPS.AdmobAdManager
...
//Event yardımı ile ödüllü reklam çalıştırılıyor
private void OnEnable() {
    AdManager.Instance.onAdReward += UserEarnedReward;
}
private void OnDisable() {
    AdManager.Instance.onAdReward -= UserEarnedReward;
}
void UserEarnedReward(string reward)
{
    //Hangi ödül olduğu mantığını burada tanımlamak gerekiyor
   Debug.Log($"\n{System.DateTime.Now.ToString("HH:mm")}: Rewarded öldülü verildi. Ödül: {reward}");
}
`
Rewarded göstermek için:
`AdManager.Instance.ShowRewardedAd("ödül tipi");`
şeklinde çağırmanız yeterli olacaktır.


Bu README.md dosyası, Unity projenizde AdMob reklamlarını nasıl kullanacağınızı anlamak için başlangıç ​​noktasıdır. Daha fazla ayrıntı için [Google Mobile Ads Dokümantasyonu ve AdMob İnteraktif Reklam Rehberine](https://developers.google.com/admob/unity/quick-start) başvurun.
