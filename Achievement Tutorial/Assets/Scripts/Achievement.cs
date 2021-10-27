using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement
{
    private string name;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    private string description; 

    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    private bool unlocked;

    public bool Unlocked
    {
        get { return unlocked; }
        set { unlocked = value; }
    }

    private int points;

    public int Points
    {
        get { return points; }
        set { points = value; }
    }

    private GameObject achievementRef;

    public GameObject AchievementRef
    {
        get { return achievementRef; }
        set { achievementRef = value; }
    }

    private List<Achievement> dependencies = new List<Achievement>();

    private string child;

    public string Child
    {
        get { return child; }
        set { child = value; }
    }

    public Achievement(string name, string description, int points, GameObject achievementRef)
    {
        this.Name = name;
        this.description = description;
        this.unlocked = false;
        this.points = points;
        this.achievementRef = achievementRef;

        LoadAchievement();
    }

    public void AddDependency(Achievement dependency)
    {
        dependencies.Add(dependency);
    }

    public bool EarnAchievement()
    {
        if (!unlocked && !dependencies.Exists(x => x.unlocked == false))
        {
            achievementRef.GetComponent<Image>().color = AchievementManager.Instance.unlockedColor;
            SaveAchievement(true);

            if (child != null)
            {
                AchievementManager.Instance.EarnAchievement(child);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SaveAchievement(bool value)
    {
        unlocked = value;

        int tmpPoints = PlayerPrefs.GetInt("Points");

        PlayerPrefs.SetInt("Points", tmpPoints += points);

        PlayerPrefs.SetInt(name, value ? 1 : 0);

        PlayerPrefs.Save();
    }

    public void LoadAchievement()
    {
        unlocked = PlayerPrefs.GetInt(name) == 1 ? true : false;

        if (unlocked)
        {
            AchievementManager.Instance.pointsText.text = "Points: " + PlayerPrefs.GetInt("Points");

            achievementRef.GetComponent<Image>().color = AchievementManager.Instance.unlockedColor;
        }
    }
}
