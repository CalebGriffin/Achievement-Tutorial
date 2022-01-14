using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    // Get a reference to the achievement prefab
    public GameObject achievementPrefab;

    // The part of the achievement UI that contains all of the achievements
    public ScrollRect scrollRect;

    // The GameObject that stores the 'Other' category achievements
    private GameObject otherCategory;

    // The GameObject that stores all of the UI for the Achievement Menu
    public GameObject achievementMenu;
    
    // A copy of each achievement that appears at the bottom of the screen when the achievement is earned
    public GameObject visualAchievement;

    // Dictionary that stores all of the achievements
    public Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();

    // The colour that the achievement will be when it has been unlocked
    public Color unlockedColor;

    // A reference to the UI text that shows the points
    public Text pointsText;

    // A reference to an instance of the AchievementManager object
    private static AchievementManager instance;

    // A public property to expose the instance
    public static AchievementManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<AchievementManager>();
            }
            return AchievementManager.instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // REMEMBER TO REMOVE
        // This deletes all of the saved data each time the game is run
        PlayerPrefs.DeleteAll();

        // Creates the achievements in this example
        CreateAchievement("General", "Press B", "Press B to unlock this achievement", 5);
        CreateAchievement("General", "Press N", "Press N to unlock this achievement", 5);
        CreateAchievement("General", "Press All Keys", "Unlock 'Press B' and 'Press N' to unlock this achievement", 5, new string[]{"Press B", "Press N"});

        // Finds the 'Other' category GameObject and sets it to inactive
        otherCategory = GameObject.Find("Other");
        otherCategory.SetActive(false);

        // Hides the achievement menu
        achievementMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Binds the 'A' key to showing and hiding the achievement menu
        if (Input.GetKeyDown(KeyCode.A))
        {
            achievementMenu.SetActive(!achievementMenu.activeSelf);
        }

        // If the user presses the 'B' key then they have achieved that achievement
        if (Input.GetKeyDown(KeyCode.B))
        {
            EarnAchievement("Press B");
        }

        // If the user presses the 'N' key then they have achieved that achievement
        if (Input.GetKeyDown(KeyCode.N))
        {
            EarnAchievement("Press N");
        }
    }

    // This function is run when the achievement is earned
    public void EarnAchievement(string title)
    {
        if (achievements[title].EarnAchievement())
        {
            // Create a visual achievement at the bottom of the screen to let the user know they have achieved it
            GameObject achievement = (GameObject)Instantiate(visualAchievement);

            // Set all of the info for the visual achievement
            SetAchievementInfo("Earn Canvas", title, achievement);
            
            // Increase the user's points based on how many points the achievement is worth
            pointsText.text = "Points: " + PlayerPrefs.GetInt("Points");

            // Start the coroutine which will slowly fade out the visual achievement
            StartCoroutine(FadeAchievement(achievement));
        }
    }

    // This is called to destroy the visual achievement after it has been created
    // NOTE: NOT BEING USED
    public IEnumerator HideAchievement(GameObject achievement)
    {
        yield return new WaitForSeconds(3);
        Destroy(achievement);
    }

    // This function is used to create the achievements
    // It takes in all of the information needed to create the achievement as parameters
    public void CreateAchievement(string parent, string title, string description, int points, string[] dependencies = null)
    {
        // Create the achievement GameObject
        GameObject achievement = (GameObject)Instantiate(achievementPrefab);

        // Create the achievement object
        Achievement newAchievement = new Achievement(name, description, points, achievement);

        // Add the achievement to the Dictionary
        achievements.Add(title, newAchievement);

        // Set the information for the new achievement
        SetAchievementInfo(parent, title, achievement);

        // Set up the dependencies of the achievement if it has any, otherwise do nothing
        if (dependencies != null)
        {
            foreach (string achievementTitle in dependencies)
            {
                Achievement dependency = achievements[achievementTitle];
                dependency.Child = title;
                newAchievement.AddDependency(dependency);
            }
        }
    }

    // Sets the information of the provided achievement based on the input it takes as parameters
    public void SetAchievementInfo(string parent, string title, GameObject achievement)
    {
        // Set's the achievement's parent
        achievement.transform.SetParent(GameObject.Find(parent).transform);

        // Sets the correct scale for the achievement
        achievement.transform.localScale = new Vector3(1, 1, 1);

        // Sets up the different UI elements of the achievement
        achievement.transform.GetChild(1).GetComponent<Text>().text = title;
        achievement.transform.GetChild(2).GetComponent<Text>().text = achievements[title].Description;
        achievement.transform.GetChild(3).GetComponent<Text>().text = achievements[title].Points.ToString();
    }

    // This function is called everytime that the user wants to look at a different category
    public void ChangeCategory(GameObject button)
    {
        // Check which button called this function and then show that category
        AchievementButton achievementButton = button.GetComponent<AchievementButton>();
        scrollRect.content = achievementButton.achievementList.GetComponent<RectTransform>();
    }

    // This is called when a visual achievement is created
    public IEnumerator FadeAchievement(GameObject achievement)
    {
        // Find the CanvasGroup of the visual achievement
        CanvasGroup canvasGroup = achievement.GetComponent<CanvasGroup>();

        // Set the rate of how fast the achievement will fade
        float rate = 1.0f;

        // Set the start and end alphas for fading the achievement in
        int startAlpha = 0;
        int endAlpha = 1;

        // A for loop that repeats twice to fade the achievement in and then out again
        for (int i = 0; i < 2; i++)
        {
            // Variable to track how long the achievement has been fading
            float progress = 0.0f;

            // While the achievement is still fading
            while (progress < 1.0)
            {
                // Set the alpha or the visibility of the achievement based on the progress variable
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);

                // Increase the progress variable by the amount of time that has passed since the previous frame multiplied by the rate
                progress += rate * Time.deltaTime;

                // Return nothing
                yield return null;
            }

            // Wait for 1 second so that the achievement will stay on the screen for a bit of time
            yield return new WaitForSeconds(1);

            // Set the start and end alphas for fading the achievement out
            startAlpha = 1;
            endAlpha = 0;
        }

        // Destroy the achievement GameObject
        Destroy(achievement);
    }
}