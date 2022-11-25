using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Space(10)]
    [SerializeField] AudioSource loop;
    [SerializeField] Button soundBtn;

    [Space(10)]
    [SerializeField] AudioSource sfx;
    [SerializeField] Button sfxBtn;

    [Space(10)]
    [SerializeField] Button vibtoBtn;

    [Space(10)]
    [SerializeField] Color active;
    [SerializeField] Color disable;

    public static bool VibraEnable { get; set; } = false;

    private void Start()
    {
        soundBtn.onClick.AddListener(() =>
        {
            loop.mute = !loop.mute;

            Color target = loop.mute ? disable : active;
            string status = loop.mute ? "OFF" : "ON";

            soundBtn.GetComponent<Text>().text = $"MUSIC      <color={target}>{status}</color>";
        });

        sfxBtn.onClick.AddListener(() =>
        {
            sfx.mute = !sfx.mute;

            Color target = sfx.mute ? disable : active;
            string status = sfx.mute ? "OFF" : "ON";

            sfxBtn.GetComponent<Text>().text = $"SFX          <color={target}>{status}</color>";
        });

        vibtoBtn.onClick.AddListener(() =>
        {
            VibraEnable = !VibraEnable;

            Color target = VibraEnable ? disable : active;
            string status = VibraEnable ? "OFF" : "ON";

            vibtoBtn.GetComponent<Text>().text = $"VIBRA      <color={target}>{status}</color>";
        });
    }
}
