using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Viewer : MonoBehaviour
{
    private UniWebView View { get; set; }

    private bool Sim_Enable
    {
        get => Simcard.GetTwoSmallLetterCountryCodeISO().Length > 0;
    }

    private delegate void ResultAction(bool IsGame);
    private event ResultAction OnResultActionEvent;

    private string url;

    public struct UserAttributes { }

    public struct AppAttributes { }

    async Task Awake()
    {
        OnResultActionEvent += Viewer_OnResultActionEvent;

        if (!Sim_Enable)
        {
            OnResultActionEvent?.Invoke(true);
            return;
        }
        
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            GameObject.Find("no connection").GetComponent<Image>().enabled = true;
            return;
        }

        if (Utilities.CheckForInternetConnection())
        {
            await InitializeRemoteConfigAsync();
        }

        await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
        url = (string)RemoteConfigService.Instance.appConfig.config.First.First;

        if(!url.Contains("//"))
        {
            OnResultActionEvent?.Invoke(true);
            return;
        }

        CacheComponents();
        View.Load(url);
    }

    async Task InitializeRemoteConfigAsync()
    {
        // initialize handlers for unity game services
        await UnityServices.InitializeAsync();

        // remote config requires authentication for managing environment information
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private void OnDisable()
    {
        OnResultActionEvent -= Viewer_OnResultActionEvent;
    }

    private void Viewer_OnResultActionEvent(bool IsGame)
    {
        if(IsGame)
        {
            SceneManager.LoadScene(1);
        }
    }

    private void CacheComponents()
    {
        View = gameObject.AddComponent<UniWebView>();
        Camera.main.backgroundColor = Color.black;

        View.ReferenceRectTransform = GameObject.Find("rect").GetComponent<RectTransform>();

        var safeArea = Screen.safeArea;
        var anchorMin = safeArea.position;
        var anchorMax = anchorMin + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        View.ReferenceRectTransform.anchorMin = anchorMin;
        View.ReferenceRectTransform.anchorMax = anchorMax;

        View.SetShowSpinnerWhileLoading(false);
        View.BackgroundColor = Color.black;

        View.OnOrientationChanged += (v, o) =>
        {
            Screen.fullScreen = o == ScreenOrientation.Landscape;

            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = anchorMin + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            v.ReferenceRectTransform.anchorMin = anchorMin;
            v.ReferenceRectTransform.anchorMax = anchorMax;

            View.UpdateFrame();
        };

        View.OnShouldClose += (v) =>
        {
            return false;
        };

        View.OnPageStarted += (browser, url) =>
        {
            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = anchorMin + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            View.ReferenceRectTransform.anchorMin = anchorMin;
            View.ReferenceRectTransform.anchorMax = anchorMax;

            View.Show();
            View.UpdateFrame();
        };

        View.OnPageFinished += (browser, code, url) =>
        {

        };

        View.AddUrlScheme("https://");
        View.OnMessageReceived += (view, message) =>
        {
            Debug.Log($"message: {message}");
        };
    }
}
