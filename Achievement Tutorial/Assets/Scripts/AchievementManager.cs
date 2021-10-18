using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public GameObject achievementPrefab;

    // Start is called before the first frame update
    void Start()
    {
        CreateAchievement("General", "TestTitle", "This is the description", 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateAchievement(string category, string title, string description, int points)
    {
        GameObject achievement = (GameObject)Instantiate(achievementPrefab);
        SetAchievementInfo(category, title, description, points, achievement);
    }

    public void SetAchievementInfo(string category, string title, string description, int points, GameObject achievement)
    {
        achievement.transform.SetParent(GameObject.Find(category).transform);
        achievement.transform.localScale = new Vector3(1, 1, 1);
        achievement.transform.GetChild(1).GetComponent<Text>().text = title;
        achievement.transform.GetChild(2).GetComponent<Text>().text = description;
        achievement.transform.GetChild(3).GetComponent<Text>().text = points.ToString();
    }
}
