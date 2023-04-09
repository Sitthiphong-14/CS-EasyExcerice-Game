using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RunController : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameOverScreen GameOverScreen;
    // counter
    [Header("Counter")]
    public TMP_Text counterText;
    private int count = 0;

    //Timer
    [Header("Timer")]
    public TMP_Text timerText;
    public float maxTimer_seconds = 120f;
    private float currentTimer;

    //Item
    [Header("Item")]
    public GameObject[] items;
    public GameObject[] lanes;
    private float delaySpawnTime = 5f;
    private float nextSpawnTime = 0f;

    //player
    [Header("player")]
    public PlayerRunController player;

    //Calculate
    [Header("CalculateKcal")]
    public float METs = 8.8f;
    private float weight;

    private void Start()
    {
        currentTimer = maxTimer_seconds;
        nextSpawnTime = maxTimer_seconds;
        Time.timeScale = 1f;

        weight = ApiManager.Instance.user.weight;
    }

    private void Update()
    {
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
        CountDownTimer();
        spawnItem();
        udpateScore();
    }

    private void udpateScore()
    {
        count = (int)player.point;
        counterText.text = count + "";
    }

    private void CountDownTimer()
    {
        currentTimer += Time.deltaTime;
        float minutes = Mathf.FloorToInt(currentTimer / 60);
        float seconds = Mathf.FloorToInt(currentTimer % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void spawnItem()
    {
        if (currentTimer >= nextSpawnTime)
        {
            GameObject newItem = Instantiate(randomItems(), randomLanes().transform.position, Quaternion.identity);
            nextSpawnTime = currentTimer + delaySpawnTime;
        }
    }

    private GameObject randomItems()
    {
        float value = Random.value;
        if (value <= 0.7)
        {
            return items[0];
        }
        else if (value > 0.7)
        {
            return items[1];
        }
        return null;
    }

    private GameObject randomLanes()
    {
        float value = Random.value;
        if (value < 0.25)
        {
            return lanes[0];
        }
        else if (value < 0.50)
        {
            return lanes[1];
        }
        else if (value < 0.75)
        {
            return lanes[2];
        }
        else if (value < 1)
        {
            return lanes[3];
        }
        return null;
    }

    public int CalculateKcal()
    {
        int kcal = (int)(METs * weight * (currentTimer / 3600));
        return kcal;
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
        GameOverScreen.Setup(CalculateKcal(), (int)currentTimer, "RUN");
    }
}
