using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public float maxTimer = 0f;
    public float currentTimer;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hightScoreText;
    public TextMeshProUGUI finalScoreText;
    public TargetUI _targetUI;
    public GameObject TutorialPanel;
    public GameObject HighScorePanel;
    public GameObject OptionPanel;
    public bool isStart;
    [SerializeField]private SC_FPSController fpsController;

    private void Start()
    {
        TutorialPanel.gameObject.SetActive(true);
        HighScorePanel.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        fpsController.imageCooldown.gameObject.SetActive(false);
        AudioManager.Instance.PlayMusic("InGame");
        StartCoroutine(GameStartVoiceLine());
        UpdateHighScoreText();
        Time.timeScale = 0;
        fpsController.enabled = false;
       
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isStart)
            {
                
                StartGame();
            }
            else
            {
                Time.timeScale = 0;
                OptionPanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                fpsController.enabled = false;
            }
        }
    }

    public void CloseOption()
    {
        Time.timeScale = 1;
        OptionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        fpsController.enabled = true;
    }

    public void ScoreChange(int value)
    {
        score += value;
        CheckHighScore();
        if (score <= 0)
        {
            score = 0;
        }
        //Change UI's score
        ScoreUI(score);
    }
    
    public IEnumerator Timer()
    {
        Debug.Log("In Timer");
        while (currentTimer > 0)
        {
            yield return new WaitForSeconds(1f);
            TimeChange(-1);

            if (currentTimer <= 0)
            {
                //0[gd,c]h;0hk let's gooo https://lullar-de-3.appspot.com/ 
                Time.timeScale = 0;

                fpsController.enabled = false;
                HighScorePanel.gameObject.SetActive(true);
                timerText.gameObject.SetActive(false);
                scoreText.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void TimeChange(float value)
    {
        currentTimer += value;
        if (currentTimer > maxTimer)
        {
            currentTimer = maxTimer;
        }
        //Change UI's timer
        TimerUI(currentTimer);
    }

    public void TimerUI(float timerUI)
    {
        if (timerUI < 0)
        {
            timerUI = 0;
        }

        float minutes = Mathf.FloorToInt(timerUI / 60);
        float seconds = Mathf.FloorToInt(timerUI % 60);

        timerText.text = string.Format("Time {0:00}:{1:00}", minutes, seconds);
    }

    public void ScoreUI(int scoreUI)
    {
        if (scoreUI < 0)
        {
            scoreUI = 0;
        }

        scoreText.text = string.Format("Your Score : " + scoreUI);
        finalScoreText.text = string.Format("Your Score : " + scoreUI);
    }

    public void StartGame()
    {
        score = 0;
        Debug.Log("In Start");
        Time.timeScale = 1;
        isStart = true;
        TutorialPanel.gameObject.SetActive(false);
        timerText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        HighScorePanel.gameObject.SetActive(false);
        currentTimer = maxTimer;
        fpsController.enabled = true;
        fpsController.imageCooldown.gameObject.SetActive(true);
        StartCoroutine(Timer());
        ScoreUI(score);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    void CheckHighScore()
    {
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            UpdateHighScoreText();
        }
    }

    void UpdateHighScoreText()
    {
        hightScoreText.text = "HighScore : "+ PlayerPrefs.GetInt("HighScore", 0);
    }

    IEnumerator GameStartVoiceLine()
    {
        yield return new WaitForSeconds(1f);
        AudioManager.Instance.PlaySFX("GameStart");
    }
}
