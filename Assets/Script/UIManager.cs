using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Panels")]
    public GameObject menuPanel;
    public GameObject trainingPanel;
    public GameObject successPanel;
    public GameObject failPanel;

    [Header("Training UI")]
    public TextMeshProUGUI stepTitleText;
    public TextMeshProUGUI stepDescText;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI trainingTimeText;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI currentStepText;
    public TextMeshProUGUI scoreText;

    [Header("Fail / Success UI")]
    public TextMeshProUGUI failDescText;
    public TextMeshProUGUI successDescText;

    [Header("References")]
    public ExtinguisherPin pinSystem;
    public ParticleSystem spray;

    [Header("Settings")]
    public float trainingTimeLimit = 60f;

    [Header("Audio")] // 🔥 音效区域
    public AudioClip successAudio;
    public AudioClip failAudio;
    private AudioSource audioSource;

    private float currentTimer;
    private bool isAimingCorrectly = false;
    private bool isSweeping = false;
    private bool fireExtinguished = false;

    private int currentStep = -1;
    private int progressStep = 0;
    private int score = 0;

    public bool isTrainingMode = false;
    public bool gameEnded = false;

    void Awake()
    {
        if (instance == null)
            instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        ShowMenu();
    }

    void Update()
    {
        if (!isTrainingMode || gameEnded)
            return;

        currentTimer -= Time.deltaTime;
        if (currentTimer < 0f)
            currentTimer = 0f;

        UpdateTimerUI();

        if (currentTimer <= 0f)
        {
            Fail();
            return;
        }

        CheckTrainingStep();
    }

    public void StartTrainingMode()
    {
        isTrainingMode = true;
        gameEnded = false;
        ResetTrainingState();
        ShowTraining();
        SetStepOnce(1);
    }

    // 🔁 重置场景（不重载）
    public void ResetScenario()
    {
        Debug.Log("Reset Scenario Called");
        gameEnded = false;
        isTrainingMode = true;
        currentStep = -1;

        ResetTrainingState();

        if (spray != null)
        {
            spray.Stop();
            spray.Clear();
        }

        if (pinSystem != null)
            pinSystem.ResetPin();

        Fire[] fires = FindObjectsOfType<Fire>();
        foreach (Fire f in fires)
        {
            f.ResetFire();
        }

        ShowTraining();
        SetStepOnce(1);

        FindObjectOfType<FireRespawn>().ResetTheFire();
    }

    void ResetTrainingState()
    {
        currentTimer = trainingTimeLimit;
        isAimingCorrectly = false;
        isSweeping = false;
        fireExtinguished = false;
        progressStep = 0;
        score = 0;

        UpdateTimerUI();
        UpdateProgressUI();
        UpdateScoreUI();
    }

    void CheckTrainingStep()
    {
        if (pinSystem == null || !pinSystem.isUnlocked)
        {
            SetStepOnce(1);
            return;
        }

        if (!isAimingCorrectly)
        {
            SetStepOnce(2);
            return;
        }

        if (spray == null || !spray.isPlaying)
        {
            SetStepOnce(3);
            return;
        }

        if (!isSweeping && !fireExtinguished)
        {
            SetStepOnce(4);
            return;
        }

        // 只要到了这一步，直接完成
        SetStepOnce(5);
    }

    void SetStepOnce(int step)
    {
        if (currentStep == step) return;

        currentStep = step;

        switch (step)
        {
            case 1:
                progressStep = 0;
                score = 0;
                SetStepUI("Step 1: Pull Pin", "Remove the safety pin before using the extinguisher.", "Hint: Pull the pin first.", "Pull Pin");
                break;
            case 2:
                progressStep = 1;
                score = 25;
                SetStepUI("Step 2: Aim", "Aim the nozzle at the base of the fire.", "Hint: Point lower at the fire base.", "Aim");
                break;
            case 3:
                progressStep = 2;
                score = 50;
                SetStepUI("Step 3: Squeeze", "Press the handle to release extinguishing agent.", "Hint: Start spraying now.", "Squeeze");
                break;
            case 4:
                progressStep = 3;
                score = 75;
                SetStepUI("Step 4: Sweep", "Move the nozzle from side to side while spraying.", "Hint: Sweep left and right while spraying, or extinguish the fire to complete.", "Sweep");
                break;
            case 5:
                progressStep = 4;
                score = 100;
                SetStepUI("Training Complete!", "You have completed all PASS steps.", "Well done!", "Completed");
                Success();
                break;
        }

        UpdateProgressUI();
        UpdateScoreUI();
    }

    void SetStepUI(string title, string desc, string feedback, string step)
    {
        if (stepTitleText != null) stepTitleText.text = title;
        if (stepDescText != null) stepDescText.text = desc;
        if (feedbackText != null) feedbackText.text = feedback;
        if (currentStepText != null) currentStepText.text = "Current Step: " + step;
    }

    void UpdateTimerUI() => trainingTimeText.text = "Time Left: " + Mathf.CeilToInt(currentTimer);
    void UpdateProgressUI() => progressText.text = "Progress: " + progressStep + " / 4";
    void UpdateScoreUI() => scoreText.text = "Score: " + score + " / 100";

    public void SetAimCorrect(bool value) => isAimingCorrectly = value;
    public void SetSweeping(bool value) => isSweeping = value;
    public void SetFireExtinguished(bool value) => fireExtinguished = value;

    public void Success()
    {
        if (gameEnded) return;
        gameEnded = true;
        trainingPanel.SetActive(false);
        successPanel.SetActive(true);

        // 用全局方式播放音效，不受位置影响
        if (successAudio != null)
        {
            AudioSource.PlayClipAtPoint(successAudio, Vector3.zero);
        }
    }

    public void Fail()
    {
        if (gameEnded) return;
        gameEnded = true;
        trainingPanel.SetActive(false);
        failPanel.SetActive(true);

        // 用全局方式播放音效，不受位置影响
        if (failAudio != null)
        {
            AudioSource.PlayClipAtPoint(failAudio, Vector3.zero);
        }
    }

    public void ReturnToMenu()
    {
        gameEnded = false;
        isTrainingMode = false;
        ShowMenu();
    }

    void ShowMenu()
    {
        menuPanel.SetActive(true);
        trainingPanel.SetActive(false);
        successPanel.SetActive(false);
        failPanel.SetActive(false);
    }

    void ShowTraining()
    {
        menuPanel.SetActive(false);
        trainingPanel.SetActive(true);
        successPanel.SetActive(false);
        failPanel.SetActive(false);
    }
}