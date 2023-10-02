namespace SRTAPPS.AdmobAdManager
{


#if UNITY_EDITOR

#endif
    using UnityEngine;
    using UnityEditor;


    [CustomEditor(typeof(AdManagerConfig))]
    public class AdManagerConfigEditor : Editor
    {
        public Texture2D headerTexture;
        public override void OnInspectorGUI()
        {
            Texture banner = (Texture)AssetDatabase.LoadAssetAtPath("Assets/SRTAPPS/Sprites/logo.png", typeof(Texture));
            GUILayout.Box(banner);

            if (PlayerPrefs.GetInt("ReviewMessage", 0) == 0)
            {
                EditorGUILayout.HelpBox("Development build ve Unity editorde ADMOB test reklamları çalışır.\nDevelopment buildi kaldırınca girmiş olduğunuz reklam idleri üzerinden çalışır.\n Destek olmak için web sitemizi ziyaret edebilir, bu mesajı bir daha görmek istemiyorsanız bir daha gösterme butonuna basabilirsiniz.", MessageType.Info);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Web sitemi ziyaret et"))
                {
                    Application.OpenURL("https://srtapps.com");
                }
                if (GUILayout.Button("Bir daha gösterme"))
                {
                    PlayerPrefs.SetInt("ReviewMessage", 1);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(10);
            }

            AdManagerConfig AMC = (AdManagerConfig)target;
            EditorGUILayout.BeginVertical();
            EditorGUILayout.HelpBox("Reklamı aktif hale getirmek için true setlemeniz gerekiyor. Aksi halde reklam tipi uygulama içinde çalışmaz. Boş bırakılan reklam idleri de çalışmaz.", MessageType.Info);
            AMC.BannerEnabled = EditorGUILayout.Toggle(new GUIContent("Banner Enabled", "True olursa banner reklamı çalışır"), AMC.BannerEnabled);
            if (AMC.BannerEnabled)
            {
                AMC.BannerAdID = EditorGUILayout.TextField(new GUIContent("Banner ID ", "Banner Reklam Kimliği, Reklam kimliği boş bırakılırsa reklam otomatik olarak disabled olur"), AMC.BannerAdID);
                AMC.BannerPosition = (GoogleMobileAds.Api.AdPosition) EditorGUILayout.EnumPopup("Banner Position ", AMC.BannerPosition);
                GUILayout.Space(5);
            }
            AMC.InterstitialEnabled = EditorGUILayout.Toggle(new GUIContent("Interstitial Enabled", "True olursa Interstitial reklamı çalışır"), AMC.InterstitialEnabled);
            if (AMC.InterstitialEnabled)
            {
                AMC.InterstitialAdID = EditorGUILayout.TextField(new GUIContent("Interstitial ID ", "Interstitial Reklam Kimliği, Reklam kimliği boş bırakılırsa reklam otomatik olarak disabled olur"), AMC.InterstitialAdID);
                GUILayout.Space(5);
            }
            AMC.RewardedEnabled = EditorGUILayout.Toggle(new GUIContent("Rewarded Enabled", "True olursa Rewarded reklamı çalışır"), AMC.RewardedEnabled);
            if (AMC.RewardedEnabled)
            {
                AMC.RewardedAdID = EditorGUILayout.TextField(new GUIContent("Rewarded ID ", "Rewarded Reklam Kimliği, Reklam kimliği boş bırakılırsa reklam otomatik olarak disabled olur"), AMC.RewardedAdID);
                GUILayout.Space(5);
            }
            AMC.LogMessages = EditorGUILayout.Toggle(new GUIContent("Log Mesajları", "True olursa script içindeki log mesajları çalışır"), AMC.LogMessages);
            EditorGUILayout.EndVertical();
            GUILayout.Space(15);
            EditorGUILayout.HelpBox("Değişikliklerinizi kaydetmeyi unutmayın!", MessageType.Error);
            if (GUILayout.Button("KAYDET"))
            {
                EditorUtility.SetDirty(AMC);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
