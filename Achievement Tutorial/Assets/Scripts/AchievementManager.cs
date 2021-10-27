using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public GameObject achievementPrefab;

    public ScrollRect scrollRect;

    private GameObject otherCategory;

    public GameObject achievementMenu;
    
    public GameObject visualAchievement;

    public Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();

    public Color unlockedColor;

    public Text pointsText;

    private static AchievementManager instance;

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
        PlayerPrefs.DeleteAll();

        CreateAchievement("General", "Press B", "Press B to unlock this achievement", 5);
        CreateAchievement("General", "Press N", "Press N to unlock this achievement", 5);
        CreateAchievement("General", "Press All Keys", "Unlock 'Press B' and 'Press N' to unlock this achievement", 5, new string[]{"Press B", "Press N"});

        otherCategory = GameObject.Find("Other");
        otherCategory.SetActive(false);

        achievementMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            achievementMenu.SetActive(!achievementMenu.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            EarnAchievement("Press B");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            EarnAchievement("Press N");
        }
    }

    public void EarnAchievement(string title)
    {
        if (achievements[title].EarnAchievement())
        {
            GameObject achievement = (GameObject)Instantiate(visualAchievement);
            SetAchievementInfo("Earn Canvas", title, achievement);
            pointsText.text = "Points: " + PlayerPrefs.GetInt("Points");
            StartCoroutine(FadeAchievement(achievement));
        }
    }

    public IEnumerator HideAchievement(GameObject achievement)
    {
        yield return new WaitForSeconds(3);
        Destroy(achievement);
    }

    public void CreateAchievement(string parent, string title, string description, int points, string[] dependencies = null)
    {
        GameObject achievement = (GameObject)Instantiate(achievementPrefab);

        Achievement newAchievement = new Achievement(name, description, points, achievement);

        achievements.Add(title, newAchievement);

        SetAchievementInfo(parent, title, achievement);

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

    public void SetAchievementInfo(string parent, string title, GameObject achievement)
    {
        achievement.transform.SetParent(GameObject.Find(parent).transform);
        achievement.transform.localScale = new Vector3(1, 1, 1);
        achievement.transform.GetChild(1).GetComponent<Text>().text = title;
        achievement.transform.GetChild(2).GetComponent<Text>().text = achievements[title].Description;
        achievement.transform.GetChild(3).GetComponent<Text>().text = achievements[title].Points.ToString();
    }

    public void ChangeCategory(GameObject button)
    {
        AchievementButton achievementButton = button.GetComponent<AchievementButton>();

        scrollRect.content = achievementButton.achievementList.GetComponent<RectTransform>();
    }

    public IEnumerator FadeAchievement(GameObject achievement)
    {
        CanvasGroup canvasGroup = achievement.GetComponent<CanvasGroup>();

        float rate = 1.0f;

        int startAlpha = 0;
        int endAlpha = 1;

        for (int i = 0; i < 2; i++)
        {

            float progress = 0.0f;

            while (progress < 1.0)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);

                progress += rate * Time.deltaTime;

                yield return null;
            }
            yield return new WaitForSeconds(1);

            startAlpha = 1;
            endAlpha = 0;
        }

        Destroy(achievement);
    }
}