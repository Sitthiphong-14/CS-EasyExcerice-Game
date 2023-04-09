using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public TMP_Text pointsText;
    public void Setup(float point, int time, string name)
    {
        gameObject.SetActive(true);
        pointsText.text = point.ToString() + " Kcal";

        StartCoroutine(ApiCall.storeRecord((int)point, (int)point, time, name, ApiManager.Instance.user.access_token, () =>
        {
            // success
        }, () =>
        {
            // fail
        }));
    }

    public void RestartRunButton()
    {
        SceneManager.LoadScene("Run");
        Time.timeScale = 1f;
        AudioManager.Instance.backgroundSouce.Play();
    }

    public void RestartSquatButton()
    {
        SceneManager.LoadScene("Squat");
        Time.timeScale = 1f;
        AudioManager.Instance.backgroundSouce.Play();
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("StartMenu");
        Time.timeScale = 1f;
        AudioManager.Instance.backgroundSouce.Play();
    }
}
