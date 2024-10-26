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
    public GameObject emptyHealthPrefab;
    public Transform emptyHealthParent;
    public float maxFillAmount = 0.75f;
    private bool isPopUpShowed = false;
    private bool groundStopped = false;

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
        GenerateEmptyHealth();
    }

    private void Update()
    {
        lerpSpeed = 2f * Time.deltaTime;
        BarFill();
    }

    public void BarFill()
    {
        // bar.fillAmount = (float)score / (float)targetScore;
        bar.fillAmount = Mathf.MoveTowards(bar.fillAmount, ((float)score / (float)targetScore) * maxFillAmount, lerpSpeed);
        AvatarMove();
    }

    public void AvatarMove()
    {
        // float range = avaXMax - avaXMin;
        float normalizedScore = Mathf.Clamp01((float)score / (float)targetScore);
        float targetX = Mathf.Lerp(avaXMin, avaXMax, normalizedScore);
        float newX = Mathf.Lerp(avatarRect.anchoredPosition.x, targetX, lerpSpeed);
        avatarRect.anchoredPosition = new Vector2(newX, avatarRect.anchoredPosition.y);
        // Debug.Log(avatarRect.anchoredPosition.x);
        float percentForDeactive = 0.18f; // 0.00 - 1.0f
        if (avatarRect.anchoredPosition.x >= avaXMax - (avaXMax * percentForDeactive))
        {
            avatarRect.gameObject.SetActive(false);
        }
        else
        {
            avatarRect.gameObject.SetActive(true);
        }
    }

    public void AddScore()
    {
        score++;
        if (score >= targetScore && !isPopUpShowed)
        {
            isWin = true;
            isPopUpShowed = true;
            TargetAchieved();
        }
    }

    public void TargetAchieved()
    {
        //Time.timeScale = 0f;
        foreach (BackgroundScript backgroundScript in backgroundScripts)
        {
            backgroundScript.animationSpeed = 0;
        }
        StopGround();
        SpineAnimationController.instance.FreezeAnimation();
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
        //Time.timeScale = 0f;
        foreach (BackgroundScript backgroundScript in backgroundScripts)
        {
            backgroundScript.animationSpeed = 0;
        }
        StopGround();
        SpineAnimationController.instance.FreezeAnimation();
        gameOverLose.SetActive(true);
    }

    public void GameOverWin()
    {
        //Time.timeScale = 0f;
        foreach (BackgroundScript backgroundScript in backgroundScripts)
        {
            backgroundScript.animationSpeed = 0;
        }
        StopGround();
        SpineAnimationController.instance.FreezeAnimation();
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
        foreach (BackgroundScript backgroundScript in backgroundScripts)
        {
            backgroundScript.animationSpeed = backgroundScript.originalSpeed;
        }
        ContinueGround();
        SpineAnimationController.instance.UnfreezeAnimation();
    }

    public void Restart()
    {
        obstaclePool.RestartObstacles();
        GenerateHealth();
        score = 0;
        isGameover = false;
        isPopUpShowed = false;
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
            return;
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

    void GenerateEmptyHealth()
    {
        for (int i = 0; i < health; i++)
        {
            Instantiate(emptyHealthPrefab, emptyHealthParent);
        }
    }

    private void StopGround()
    {
        ObstacleObject[] groundnCoinObjects = FindObjectsOfType<ObstacleObject>();

        foreach (ObstacleObject obj in groundnCoinObjects)
        {
            obj.speed = 0;
        }
        groundStopped = true;
        GameEvents.GroundStopped(groundStopped);
    }

    private void ContinueGround()
    {
        ObstacleObject[] groundnCoinObjects = FindObjectsOfType<ObstacleObject>();

        foreach (ObstacleObject obj in groundnCoinObjects)
        {
            obj.speed = obj.originalSpeed;
        }
        groundStopped = false;
        GameEvents.GroundStopped(groundStopped);
    }
}
