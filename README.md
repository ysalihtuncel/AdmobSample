![Cover Image](https://raw.githubusercontent.com/ysalihtuncel/AdmobSample/main/Assets/SRTAPPS/Sprites/cover_new.png)

# AdMob Reklam Yönetimi
GoogleMobileAds-v8.5.1.unitypackage kullanıılmıştır.<br>
Google'ın kullanıcı rızası yönetim çözümünü kullanmak isterseniz öncelikle GDPR mesajınızı oluşturun. UMP SDK'sı GoogleMobileAds-v8.5.1.unitypackage ile projede bulunuyor ve komut dosyası içeriğine dahil edilmiştir.<br>
Projede çalışması için projenizi imzalamış olmanız gerekiyor. GDPR constent form için config içerisinde ayarlar bulunuyor.<br>

Bu proje, Unity kullanarak AdMob reklamlarını yönetmek için bir örnek sunar. Projede banner, interstitial ve rewarded reklamları nasıl kullanacağınızı öğrenebilirsiniz.<br>
**Development build ve Unity Editor üzerinde sadece test reklamları çalışacak şekilde ayarlanmıştır. Geçersiz trafikten korunmak için projeniz bittiği zaman gerçek reklam kimliklerinizi kullanın.<br>
Hiç bir şekilde kendi reklamlarınıza tıklamayın. Aksi taktirde _geçersiz trafik_ kısıtlaması almanız muhtemel olacaktır!**<br>
<br>
Paket Halinde indirmek için:
[Paket Linki](https://drive.google.com/file/d/11MObyy09AiQ9SFIMQC_3x9Ib9fEzFKXr/view?usp=sharing)<br>

## Config Ayarları:
Assets/SRTAPPS/Resources dizini altında bulunan **AdManagerConfig** scriptable objesi ile reklam ayarlarınızı düzenleyebilirsiniz.<br> Reklam türlerine göre enabled/disabled hale getirebilirsiniz.<br>
Bunun için yapmanız gerekn çok basit:<br>
  **Banner için örnek:**<br>
  AdManagerConfig.asset dosyasında Unity editor içinde inspector penceresinden **bannerEnabled** değerini true olarak ayarlayın.<br>
  Banner ID'inizi girin.<br>
  Banner pozisyonunu setleyin.<br>
GDPR ayarlarınız için:<br>
  **DevelopmentMode** herhangi bir şekilde formu açmaz. İşlemlerine devam eder.<br>
  **DevelopmentTestMode** test etmek için kullanabilirsiniz.<br>
  **ProductionMode** uygulamanızı yayınlarken production mode a almanız gerekir.<br>

## Banner Reklamları
Eğer Banner reklamını aktif ettiyseniz otomatik olarak gösterilecektir.<br>

Banner reklamını başka bir script ile kaldırmak için:<br>
```
using SRTAPPS.AdmobAdManager;
...
AdManager.Instance.DestroyBannerView();
```
Banner reklamını başka bir script ile göstermek için:<br>
```
using SRTAPPS.AdmobAdManager;
...
AdManager.Instance.LoadBannerAd();
```
şeklinde çağırmanız yeterli olacaktır.<br>

## Interstitial Reklamları
Interstitial reklamları kullanmak için, aşağıdaki kodu kullanabilirsiniz:<br>
```
using SRTAPPS.AdmobAdManager
...
`AdManager.Instance.ShowInterstitialAd();
```
Geçiş reklama kapatıldığı zaman yeni geçiş reklamı yükleme kodu çalışacaktır.<br>

## Rewarded Reklamlar
Rewarded reklamları kullanmak için, aşağıdaki kodu kullanabilirsiniz. Ödül türünü kullanmak için onAdReward event'ine subscribe olmanız gerekiyor:<br>
```
using SRTAPPS.AdmobAdManager
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
```
Rewarded göstermek için:<br>
```
AdManager.Instance.ShowRewardedAd("ödül tipi");
```
şeklinde çağırmanız yeterli olacaktır.<br>


Bu README.md dosyası, Unity projenizde AdMob reklamlarını nasıl kullanacağınızı anlamak için başlangıç ​​noktasıdır. Daha fazla ayrıntı için [Google Mobile Ads Dokümantasyonu ve AdMob İnteraktif Reklam Rehberine](https://developers.google.com/admob/unity/quick-start) başvurun.

## Katkıda Bulunma

Eğer bu projeye katkıda bulunmak isterseniz, lütfen aşağıdaki adımları takip edin:

1. Bu projeyi kendi GitHub hesabınıza `fork` edin.
2. Değişikliklerinizi yapın ve yeni bir `branch` oluşturun: `git checkout -b feature/yenilik`
3. Değişikliklerinizi `commit` edin: `git commit -am 'Yenilikleri ekle'`
4. Yeni `branch`'inizi GitHub'a gönderin: `git push origin feature/yenilik`
5. Bir `Pull Request` oluşturun.

## Lisans

Bu proje MIT Lisansı altında lisanslanmıştır. Daha fazla bilgi için [LİSANS](LICENSE) dosyasını inceleyebilirsiniz.
