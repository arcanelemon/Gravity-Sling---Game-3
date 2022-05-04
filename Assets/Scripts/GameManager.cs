using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables

    [SerializeField]
    Image scrapFillImage;

    [SerializeField]
    Image timerAlertImage;

    [SerializeField]
    TextMeshProUGUI timerUGUI;

    [SerializeField]
    TextMeshProUGUI endCountdownUGUI;

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject endMenu;

    [SerializeField]
    int timer;

    [SerializeField]
    int endTimer;

    [SerializeField]
    Vector2 bounds;

    float time;

    int totalScrap;

    int scrapBank;

    IEnumerator slowTimeCoroutine;

    public static GameManager instance;

    #endregion Variables
    
    #region Engine

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
            Destroy(this.gameObject);

        instance = this;
        time = (float)timer;

        Scrap[] scraps = GameObject.FindObjectsOfType<Scrap>();
        foreach (Scrap scrap in scraps)
        {
            totalScrap += scrap.Value;
        }

        Time.timeScale = 1;

        PlayerData data = SaveManager.LoadPlayerData();
        if (data != null)
            scrapBank = data.scraps;
        AudioManager.instance.Play("Background_music_2");
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        FormatTimer();

        if(time <= 1.5)
        {
            AudioManager.instance.Play("Time_Up");
        }

        if (time <= 0)
            ResetGame();
    }

    #endregion Engine

    #region Private Methods

    void FormatTimer() 
    {

        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.RoundToInt(time % 60);

        string minutesText = minutes.ToString();
        string secondsText = Mathf.RoundToInt(seconds).ToString();

        if (minutes < 10)
            minutesText = "0" + minutesText;
        if (seconds < 10)
            secondsText = "0" + secondsText;

        if (time / (float)timer < 0.25f)
            timerAlertImage.gameObject.SetActive(true);

        timerUGUI.text = minutesText + ":" + secondsText;
    }

    void StartSlowTimeCoroutine() 
    {
        if (slowTimeCoroutine != null)
            StopSlowTimeCoroutine();

        slowTimeCoroutine = SlowTime();
        StartCoroutine(slowTimeCoroutine);
    }

    void StopSlowTimeCoroutine() 
    {
        StopCoroutine(slowTimeCoroutine);
        slowTimeCoroutine = null;
        Time.timeScale = 1;
    }

    IEnumerator SlowTime() 
    {
        float slowTimer = 0.1f;
        float slowTime = 0;
        float increment = 0.001f;
        float timeScale = Time.timeScale;

        while (slowTime < slowTimer)
        {
            timeScale -= increment / slowTimer;
            Time.timeScale = Mathf.Max(timeScale, 0);
            slowTime += increment;
            yield return new WaitForSecondsRealtime(increment);
        }
    }
    
    IEnumerator StartCountdownTimer() 
    {
        int endTime = endTimer;

        while (endTime > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            endTime -= 1;
            endCountdownUGUI.text = endTime.ToString();
        }

        Time.timeScale = 1;
        SaveManager.SavePlayerData(new PlayerData(totalScrap + scrapBank));
        SceneManager.LoadScene("Main Menu");
    }

    void PopulateEndMenu() 
    {
    
    }

    #endregion Private Methods

    #region Public Methods

    public void ResetGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndLevel() 
    {
        endMenu.SetActive(true);
        //endMenu.GetComponent<EndMenu>().Populate(totalScrap, time);
        StartSlowTimeCoroutine();
        StartCoroutine(StartCountdownTimer());
    }

    public void IncrementScrap(int value) 
    {
        scrapFillImage.fillAmount = scrapFillImage.fillAmount + value / totalScrap;
    }

    public void Pause() 
    {
        pauseMenu.SetActive(true);
        StartSlowTimeCoroutine();
    }

    public void Resume()
    {
        StopSlowTimeCoroutine();
        pauseMenu.SetActive(false);
    }

    public void ReturnToMenu() 
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit() 
    {
        Application.Quit();
    }

    #endregion Public Methods
}
