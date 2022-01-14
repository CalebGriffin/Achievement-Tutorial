using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementButton : MonoBehaviour
{
    // Get a reference to the list of achievements
    public GameObject achievementList;

    // Get a reference to the 2 colours that are used for the achievement background
    public Color neutral, highlight;

    // Get a reference to the image in the background of the achievement
    private Image sprite;

    // Start is called before the first frame update
    void Start()
    {
        // Set the sprite variable to the Image component on this GameObject
        sprite = GetComponent<Image>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
