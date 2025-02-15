using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteMenu : MonoBehaviour
{
    public GameObject CompletionMenuUI;

    public GameObject LevelSkipButton;

    public TMP_Text timeSpentDisplay;

    public TMP_Text butterflyCountDisplay;

    public GameObject checkpointPrefab;

    private List<GameObject> butterflies = new List<GameObject>();

    private LevelProgressTracker tracker;

    private float barWidth = 0;

    private AudioManager audioManager;

    void Start()
    {
        tracker =
            GameObject
                .FindGameObjectWithTag("ProgressManager")
                .GetComponent<LevelProgressTracker>();

        audioManager =
    GameObject
        .FindGameObjectWithTag("AudioManager")
        .GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tracker.IsLevelComplete() && CompletionMenuUI.activeSelf == false)
        {
            //updateScore();
            timeSpentDisplay.SetText(ConvertTime(Time.timeSinceLevelLoad));
            CreateCheckpointMarkers();
            audioManager.Play("Complete");
            CompletionMenuUI.SetActive(true);

            // Unlock the next level
            Debug.LogWarning("This is deprecated. Use the level progress tracker");

        }
        else if (tracker.IsLevelSkippable())
        {
            LevelSkipButton.SetActive(true);
        }
    }

    public void LoadNextLevel()
    {
        LevelLoader.LoadNextLevel();
    }

    public void HideSkipButton()
    {
        LevelSkipButton.SetActive(false);
    }

    public string ConvertTime(float deltaTime)
    {
        string minutes = Mathf.Floor(deltaTime / 60).ToString("00");
        string seconds = Mathf.Floor(deltaTime % 60).ToString("00");

        return minutes + ":" + seconds;
    }

    public void updateScore()
    {
        Scene scene = SceneManager.GetActiveScene();
        int level = int.Parse(scene.name.Substring(5));
        int currentScore = tracker.GetCheckpointsCompleted();
        string scoreString = "Level" + level + "score";

        if (PlayerPrefs.HasKey(scoreString))
        {
            int prevScore = PlayerPrefs.GetInt(scoreString);

            // if player got higher score this round, overwrite that value
            if (tracker.GetCheckpointsCompleted() > prevScore)
            {
                PlayerPrefs.SetInt(scoreString, currentScore);
            }
        }
        else
        {
            PlayerPrefs.SetInt(scoreString, currentScore);
        }
    }

    // Adding butterfly counts on the game complete menu
    private void CreateCheckpointMarkers()
    {
        barWidth =
            butterflyCountDisplay
                .GetComponent<RectTransform>()
                .rect
                .width;
        for (int i = 0; i < tracker.GetCheckpointsCompleted(); i++)
        {
            GameObject cp =
                Instantiate(checkpointPrefab,
                new Vector3((i / 2f) * barWidth - barWidth / 2f, 0, 0),
                Quaternion.identity);
            cp.transform.localScale =
                new Vector3(cp.transform.localScale.x * 2,
                    cp.transform.localScale.y * 2,
                    0); // change its local scale in x y z format
            cp.transform.SetParent(butterflyCountDisplay.transform, false);
            butterflies.Add(cp);
            cp.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
