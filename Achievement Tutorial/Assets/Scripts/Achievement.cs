using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement
{
    // The name of the achievement
    private string name;

    // A property to expose the name of the achievement
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    // The description attached to the achievement
    private string description; 

    // The property to expose the description
    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    // A boolean to track whether the achievement has been achieved
    private bool unlocked;

    // A property to expose the unlocked boolean
    public bool Unlocked
    {
        get { return unlocked; }
        set { unlocked = value; }
    }

    // How many points is the achievement worth
    private int points;

    // A property to expose how many points the achievement is worth
    public int Points
    {
        get { return points; }
        set { points = value; }
    }

    // Gets a reference to the actual achievement GameObject
    private GameObject achievementRef;

    // A property to expose the reference to the achievement GameObject
    public GameObject AchievementRef
    {
        get { return achievementRef; }
        set { achievementRef = value; }
    }

    // A list of achievements that must be achieved before this achievement 
    private List<Achievement> dependencies = new List<Achievement>();

    // The name of the children achievements
    private string child;

    // A property to expose the name of the children achievements
    public string Child
    {
        get { return child; }
        set { child = value; }
    }

    // A function that sets all of the different parts of the achievement
    public Achievement(string name, string description, int points, GameObject achievementRef)
    {
        this.Name = name;
        this.description = description;
        this.unlocked = false;
        this.points = points;
        this.achievementRef = achievementRef;

        LoadAchievement();
    }

    // A function that will add the provided achievement as a dependency to this achievement
    public void AddDependency(Achievement dependency)
    {
        dependencies.Add(dependency);
    }

    // A function that will be run when the achievement is earned
    public bool EarnAchievement()
    {
        // Check to see if it is not already unlocked and that all of the dependencies have been achieved
        if (!unlocked && !dependencies.Exists(x => x.unlocked == false))
        {
            // Change the colour of the achievement
            achievementRef.GetComponent<Image>().color = AchievementManager.Instance.unlockedColor;
            // Save the achievement
            SaveAchievement(true);

            // If the achievement has any children, then check if they have been achieved
            if (child != null)
            {
                AchievementManager.Instance.EarnAchievement(child);
            }
            // Return that the achievement has been achieved
            return true;
        }
        else
        {
            // Return that the achievement has not been achieved
            return false;
        }
    }

    // A function that will save the achievement and the relevant points to the PlayerPrefs
    public void SaveAchievement(bool value)
    {
        unlocked = value;

        int tmpPoints = PlayerPrefs.GetInt("Points");

        PlayerPrefs.SetInt("Points", tmpPoints += points);

        // Using a ternary operator to set the achievement to 1 or 0 based on if it has been unlocked or not
        PlayerPrefs.SetInt(name, value ? 1 : 0);

        PlayerPrefs.Save();
    }

    // A function to get all of the information about the achievement from PlayerPrefs and apply it to the achievement
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