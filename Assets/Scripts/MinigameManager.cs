using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public GameObject gameOverLose;
    public GameObject popUpWin;
    public GameObject gameOverWin;
    public GameObject backHomeMenu;
    public GameObject pauseMenu;
    public GameObject backHomeAdditionalText;
    public int targetScore = 25;

    private int score = 0;
    private bool isWin = false;
    public Image bar;
    private float lerpSpeed;
    public float avaXMin;
    public float avaXMax;
    public RectTransform avatarRect;
    public ObstaclePool obstaclePool;

    private int health = 3;
    public int maxHealth = 3;

    public GameObject healthPrefab;
    public Transform healthParent;

    private List<GameObject> healthObjects = new List<GameObject>();

    public BackgroundScript[] backgroundScripts;
    private bool isXPAdded = false;
    private bool isGameover = false;
    private bool isPaused = false;

    private void Start()
    {
        isWin = false;
        isGameover = false;
        isPaused = false;
        score = 0;
        gameOverLose.SetActive(false);
        gameOverWin.SetActive(false);
        popUpWin.SetActive(false);
        backHomeMenu.SetActive(false);
        pauseMenu.SetActive(false);
        GenerateHealth();
    }

    private void Update()
    {
        lerpSpeed = 2f * Time.deltaTime;
        BarFill();
    }

    public void BarFill()
    {
        // bar.fillAmount = (float)score / (float)targetScore;
        bar.fillAmount = Mathf.Lerp(bar.fillAmount, (float)score / (float)targetScore, lerpSpeed);
        AvatarMove();
    }

    public void AvatarMove()
    {
        // float range = avaXMax - avaXMin;
        float normalizedScore = Mathf.Clamp01((float)score / (float)targetScore);
        float targetX = Mathf.Lerp(avaXMin, avaXMax, normalizedScore);
        float newX = Mathf.Lerp(avatarRect.anchoredPosition.x, targetX, lerpSpeed);
        avatarRect.anchoredPosition = new Vector2(newX, avatarRect.anchoredPosition.y);
    }

    public void AddScore()
    {
        score++;
        if (score >= targetScore && !isWin)
        {
            isWin = true;
            TargetAchieved();
        }
    }

    public void TargetAchieved()
    {
        Time.timeScale = 0f;
        popUpWin.SetActive(true);
        if (!isXPAdded)
        {
            // if (GameManager.instance.CatProfile.catScriptable.playRemaining > 0)
            // {
            //     GameManager.instance.CatProfile.catScriptable.playRemaining--;
            //     GameManager.instance.AddXP();
            //     GameManager.instance.LevelUpChecker();
            // }
            GameManager.instance.CompleteMissionChecker(ref GameManager.instance.CatProfile.catScriptable.playRemaining);
            isXPAdded = true;
            if (GameManager.instance.CatProfile.catScriptable.isSad) GameManager.instance.ChangeSad();
            GameManager.instance.isPlayTimerOn = false;
        }
    }

    public void GameOverLose()
    {
        isGameover = true;
        Time.timeScale = 0f;
        gameOverLose.SetActive(true);
    }

    public void GameOverWin()
    {
        Time.timeScale = 0f;
        gameOverWin.SetActive(true);
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        gameOverLose.SetActive(false);
        gameOverWin.SetActive(false);
        popUpWin.SetActive(false);
        backHomeMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        obstaclePool.RestartObstacles();
        GenerateHealth();
        score = 0;
        isGameover = false;
        Resume();
    }

    public void HomeConfirmation()
    {
        Time.timeScale = 0f;
        if (isPaused) pauseMenu.SetActive(false);
        if (isGameover) gameOverLose.SetActive(false);
        if (isWin) backHomeAdditionalText.SetActive(false);
        backHomeMenu.SetActive(true);
    }

    public void CancelHome()
    {
        if (isGameover)
        {
            backHomeMenu.SetActive(false);
            GameOverLose();
        }
        else if (isPaused)
        {
            backHomeMenu.SetActive(false);
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void HittingEnemy()
    {
        health--;
        Destroy(healthObjects[health]);
        healthObjects.RemoveAt(health);
        if (health <= 0)
        {
            if (isWin) GameOverWin();
            else GameOverLose();
        }
        foreach (BackgroundScript backgroundScript in backgroundScripts)
        {
            backgroundScript.SpeedReduction();
        }

        ReduceSpeedForAllGroundnCoin();
    }

    void GenerateHealth()
    {
        health = maxHealth;
        while (healthObjects.Count > health)
        {
            Destroy(healthObjects[healthObjects.Count - 1]);
            healthObjects.RemoveAt(healthObjects.Count - 1);
        }

        for (int i = healthObjects.Count; i < health; i++)
        {
            GameObject healthObject = Instantiate(healthPrefab, healthParent);
            healthObjects.Add(healthObject);
        }
    }

    private void ReduceSpeedForAllGroundnCoin()
    {
        ObstacleObject[] groundnCoinObjects = FindObjectsOfType<ObstacleObject>();

        foreach (ObstacleObject obj in groundnCoinObjects)
        {
            obj.SpeedReduction();
        }
    }
}
