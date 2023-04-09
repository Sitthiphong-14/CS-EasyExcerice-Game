using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SquatController : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public Animator playerAnimator;
    //[SerializeField] GameObject panel;
    public GameOverScreen GameOverScreen;
    // charge
    [Header("Charger")]
    public GaugeBar chargerBar;
    private float currentCharge = 0;
    public float maxCharge = 2;

    // counter
    [Header("Counter")]
    public TMP_Text counterText;
    private int count = 0;
    private int sumCount = 0;

    // gauge bar
    [Header("GaugeBar")]
    public CounterBar counterBar;
    public int maxCombo;
    private int currentCombo;

    //Timer
    [Header("Timer")]
    public TMP_Text timerText;
    public float maxTimer_seconds = 120f;
    private float currentTimer;

    //Calculate
    [Header("CalculateKcal")]
    public float METs = 4;
    private float weight;

    private bool isSquat = false;
    private bool reset = false;

    private void Start()
    {
        Debug.Log("START!!!");
        chargerBar.SetStartValue(maxCharge);
        counterBar.SetStartCounterValue(maxCombo);
        currentTimer = maxTimer_seconds;

        weight = ApiManager.Instance.user.weight;
    }

    private void Update()
    {
        if(PoseTransform.Instance.isLegUp())
        // if (Input.GetKey(KeyCode.Space))
        {
            Charge();
            reset = true;
        }
        else
        {
            if (reset)
            {

                Release();
                reset = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        Timer();
    }

    private void Charge()
    {
        if (currentCharge <= maxCharge)
        {
            currentCharge += Time.deltaTime;
            chargerBar.SetValue(currentCharge);
        }
        playerAnimator.SetBool("isSquating", true);
        // AudioManager.Instance.PlaySFX("Charge");
    }

    private void Release()
    {
        if (currentCharge >= maxCharge)
        {
            currentCombo += 1;
            counterBar.SetCounterValue(currentCombo);
        }
        else
        {
            currentCombo = 0;
            counterBar.ClearCounter();
        }

        Count();
        currentCharge = 0;
        chargerBar.SetValue(currentCharge);
        playerAnimator.SetBool("isSquating", false);
        // AudioManager.Instance.sfxSource.Stop();
    }

    private void Count()
    {
        count += 1;
        sumCount = count;
        counterText.text = count + "";
    }
    public int CalculateKcal()
    {
        int kcal = (int)(METs * weight * (currentTimer / 3600));
        return kcal;
    }
    private void Timer()
    {
        currentTimer += Time.deltaTime;
        float minutes = Mathf.FloorToInt(currentTimer / 60);
        float seconds = Mathf.FloorToInt(currentTimer % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void GiveUP()
    {
        Time.timeScale = 0f;
        Debug.Log("GAME OVER");
        GameOver();
        pauseMenuUI.SetActive(false);
        AudioManager.Instance.backgroundSouce.Stop();
    }

    public void GameOver()
    {
        GameOverScreen.Setup(CalculateKcal(), (int)currentTimer, "SQUAT");
    }
}
