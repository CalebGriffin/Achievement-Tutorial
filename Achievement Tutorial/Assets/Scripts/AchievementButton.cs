using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementButton : MonoBehaviour
{
    public GameObject achievementList;

    public Color neutral, highlight;

    private Image sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<Image>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
