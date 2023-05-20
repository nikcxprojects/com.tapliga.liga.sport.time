using OneSignalSDK;
using UnityEngine;

public class OneSignalInitializer : MonoBehaviour
{

    private void Start()
    {
        // Enable lines below to debug issues with OneSignal
        OneSignal.Default.LogLevel = LogLevel.Info;
        OneSignal.Default.AlertLevel = LogLevel.Fatal;

        OneSignal.Default.Initialize("959ca1c6-c290-4064-af94-299e0a88b5ac");
    }
}