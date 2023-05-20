using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Viewer : MonoBehaviour
{
    bool Sim_Enable
    {
        get => Simcard.GetTwoSmallLetterCountryCodeISO().Length > 0;
    }

    delegate void ResultAction(bool IsGame);
    event ResultAction OnResultActionEvent;

    private string url;

    public struct UserAttributes { }

    public struct AppAttributes { }

    async Task Awake()
    {
        if (!Sim_Enable)
        {
            OnResultActionEvent?.Invoke(true);
            return;
        }
        
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            GameObject.Find("no connection").GetComponent<SpriteRenderer>().enabled = true;
            return;
        }

        Application.deepLinkActivated += OnDeepLinkActivated;
        if (!string.IsNullOrEmpty(Application.absoluteURL))
        {
            OnDeepLinkActivated(Application.absoluteURL);
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

        Init();
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

    private void OnEnable()
    {
        OnResultActionEvent += Viewer_OnResultActionEvent;
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

    private void OnDeepLinkActivated(string url)
    {
        if(url.Contains("game"))
        {
            OnResultActionEvent?.Invoke(true);
        }
    }

    private void Init()
    {
        Screen.fullScreen = false;
        Application.OpenURL(url);
    }


    private void OnApplicationFocus(bool focus)
    {
        if (focus && string.IsNullOrEmpty(Application.absoluteURL))
        {
            Init();
        }
    }
}
