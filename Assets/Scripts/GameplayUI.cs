using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//this is the class to handle the update of 
public class GameplayUI : MonoBehaviour
{
    public GameObject ScoreUI;
    public GameObject LivesUI;
    public GameObject GameOverUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLivesUI(int Lives)
    {
        TextMeshProUGUI text = LivesUI.GetComponent<TextMeshProUGUI>();
        if (text)
        {
            text.SetText("Lives: " + Lives.ToString());
        }
    }

    public void UpdateScoreUI(int Score)
    {
        TextMeshProUGUI text = ScoreUI.GetComponent<TextMeshProUGUI>();
        if (text)
        {
            text.SetText("Scores: " + Score.ToString());
        }
    }

    public void ActivateGameOverUI(bool Activated)
    {
        if (GameOverUI)
        {
            GameOverUI.SetActive(Activated);
        }
    }
}
