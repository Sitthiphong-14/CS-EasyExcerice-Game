using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class MenuController : MonoBehaviour
{
    public static MenuController Instance { set; get; }
    [SerializeField] public TMP_Text user_text;
    [SerializeField] private Animator menuAnimator;

    [Header("music")]
    [SerializeField] private Image musicBtn;
    [SerializeField] private Sprite music_default;
    [SerializeField] private Sprite music_mute;
    [Header("sound")]
    [SerializeField] private Image soundBtn;
    [SerializeField] private Sprite sound_default;
    [SerializeField] private Sprite sound_mute;

    private float currentTime;
    private float nextUpdateTime;
    private float delayUpdateTime = 120f;

    private void Awake()
    {
        Time.timeScale = 1f;
        Instance = this;

        updateSettingPanel();
        RegisterEvents();
    }

    private void LateUpdate()
    {
        if (currentTime >= nextUpdateTime || !user_text.gameObject.active)
        {
            updateSettingPanel();
            nextUpdateTime = currentTime + delayUpdateTime;
        }
    }

    //Buttons
    public void OnBackButton()
    {
        menuAnimator.SetTrigger("StartMenu");
    }

    public void OnSettingButton()
    {
        menuAnimator.SetTrigger("SettingMenu");
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
        updateSettingPanel();
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
        updateSettingPanel();
    }

    public void OnHealthMenu()
    {
        menuAnimator.SetTrigger("HealthMenu");
    }

    public void OnPlayMenu()
    {
        menuAnimator.SetTrigger("PlayMenu");
    }

    //change Scene
    public void OnRunScene()
    {
        SceneManager.LoadScene("Run");
    }

    public void OnSquatScene()
    {
        SceneManager.LoadScene("Squat");
    }

    public void updateSettingPanel()
    {
        if (ApiManager.Instance.user != null)
        {
            User user = ApiManager.Instance.user;
            user_text.gameObject.SetActive(true);
            user_text.text = user.username + "\n" + user.weight + " kg/ " + user.height + "cm/ age" + user.age;
        }
        if (!AudioManager.Instance.sfxSource.mute)
        {
            soundBtn.sprite = sound_default;
        }
        else
        {
            soundBtn.sprite = sound_mute;
        }

        if (!AudioManager.Instance.backgroundSouce.mute)
        {
            musicBtn.sprite = music_default;
        }
        else
        {
            musicBtn.sprite = music_mute;
        }
    }

    #region Server
    private void RegisterEvents()
    {
        NetUtility.S_RUN += OnGetRun;
        NetUtility.S_SQUAT += OnGetSquat;
    }
    private void UnRegisterEvents()
    {
        NetUtility.S_RUN -= OnGetRun;
        NetUtility.S_SQUAT -= OnGetSquat;
    }

    private void OnGetRun(NetworkMessage msg, NetworkConnection cnn)
    {
        OnRunScene();
    }
    private void OnGetSquat(NetworkMessage msg, NetworkConnection cnn)
    {
        OnSquatScene();
    }
    #endregion
}
