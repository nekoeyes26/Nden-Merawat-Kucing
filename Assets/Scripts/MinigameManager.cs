using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public Text scoreText;
    public Text gameOverText;
    public GameObject restartButton;
    public GameObject pauseButton;
    public GameObject resumeButton;
    public int targetScore = 25;

    private int score = 0;
    private bool isGamePaused = false;
    private bool isGameOver = false;
    public Image bar;
    private float lerpSpeed;
    public float avaXMin;
    public float avaXMax;
    public RectTransform avatarRect;
    public ObstaclePool obstaclePool;

    public int health = 3;

    public GameObject healthPrefab;
    public Transform healthParent;

    private List<GameObject> healthObjects = new List<GameObject>();

    public BackgroundScript[] backgroundScripts;
    private bool isXPAdded = false;

    private void Start()
    {
        score = 0;
        scoreText.text = "Score: 0";
        gameOverText.enabled = false;
        restartButton.SetActive(false);
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
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
        scoreText.text = "Score: " + score;
        if (score >= targetScore)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        gameOverText.enabled = true;
        gameOverText.text = "Congratulations! You won! Your score is " + score;
        restartButton.SetActive(true);
        pauseButton.SetActive(false);
        resumeButton.SetActive(false);
        if (!isXPAdded && GameManager.instance.CatProfile.catScriptable.playRemaining > 0)
        {
            GameManager.instance.AddXP();
            GameManager.instance.CatProfile.catScriptable.playRemaining--;
            isXPAdded = true;
            GameManager.instance.LevelUpChecker();
            if (GameManager.instance.CatProfile.catScriptable.isSad) GameManager.instance.ChangeSad();
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        gameOverText.enabled = true;
        gameOverText.text = "Game Over! Your score is " + score;
        restartButton.SetActive(true);
        pauseButton.SetActive(false);
        resumeButton.SetActive(false);
    }

    public void Pause()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        resumeButton.SetActive(true);
    }

    public void Resume()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
    }

    public void Restart()
    {
        obstaclePool.Restart();
        GenerateHealth();
        pauseButton.SetActive(true);
        restartButton.SetActive(false);
        gameOverText.enabled = false;
        score = 0;
        scoreText.text = "Score: " + score;
        Time.timeScale = 1f;
    }

    public void HittingEnemy()
    {
        health--;
        Debug.Log(health);
        Destroy(healthObjects[health]);
        healthObjects.RemoveAt(health);
        if (health <= 0)
        {
            GameOver();
        }
        foreach (BackgroundScript backgroundScript in backgroundScripts)
        {
            backgroundScript.SpeedReduction();
        }

        ReduceSpeedForAllGroundnCoin();
    }

    void GenerateHealth()
    {
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
        GroundnCoinMove[] groundnCoinObjects = FindObjectsOfType<GroundnCoinMove>();

        foreach (GroundnCoinMove obj in groundnCoinObjects)
        {
            obj.SpeedReduction();
        }
    }
}
