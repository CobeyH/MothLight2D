using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtons : MonoBehaviour
{
    [SerializeField]
    GameObject buttonPrefab;

    List<Button> levelButtons = new List<Button>();

    // Start is called before the first frame update
    void Start()
    {
        // substracting start screen and level selctor scene
        int levelCount = SceneManager.sceneCountInBuildSettings - 3;

        for (int levelIndex = 1; levelIndex <= levelCount; levelIndex++)
        {
            GameObject buttonObject =
                Instantiate(buttonPrefab, gameObject.transform);
            buttonObject.GetComponent<LevelChanger>().level = levelIndex;
            Button button =
                buttonObject.GetComponent<MothLightLevelSelection>().button;
            levelButtons.Add(button);
        }
        UpdateButtonInteraction();
    }

    void UpdateButtonInteraction()
    {
        int levelCount = SceneManager.sceneCountInBuildSettings - 3;
        int furthestLevel = PlayerPrefs.GetInt("furthestUnlock", 1);
        for (int buttonIndex = 0; buttonIndex < levelCount; buttonIndex++)
        {
            levelButtons[buttonIndex].interactable =
                buttonIndex < furthestLevel;
        }
    }

    public void ResetLevelProgress()
    {
        PlayerPrefs.SetInt("furthestUnlock", 1);
        UpdateButtonInteraction();
        GameObject[] checkpoints =
            GameObject.FindGameObjectsWithTag("Checkpoint");
        PlayerPrefs.DeleteAll();
        foreach (GameObject cp in checkpoints)
        {
            Destroy(cp);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
