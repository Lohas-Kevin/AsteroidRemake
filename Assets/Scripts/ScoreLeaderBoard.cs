using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This is the class to show retrive the leaderboard data
//and show it on the leaderboard
public class ScoreLeaderBoard : MonoBehaviour
{
    public GameObject ScoreUIElementPrefab;
    public string StartSceneName = "StartScene";
    public int MaximumRecords = 5;

    private HighestScore _CrossSceneScoreManager;
    // Start is called before the first frame update
    void Start()
    {
        _CrossSceneScoreManager = GameObject.FindGameObjectWithTag("ScoreTracker").GetComponent<HighestScore>();

        //update the score leaderboard
        if (_CrossSceneScoreManager && ScoreUIElementPrefab)
        {
            List<int> ScorePool = _CrossSceneScoreManager.RetrieveSortedHighestScore();
            int ScoreUICounter = 0;
            foreach(int Score in ScorePool)
            {
                ScoreUICounter += 1;
                GameObject ScoreUIElement = Instantiate(ScoreUIElementPrefab, this.gameObject.transform);
                TextMeshProUGUI Text = ScoreUIElement.GetComponent<TextMeshProUGUI>();
                if (Text)
                {
                    Text.text = Score.ToString();
                }

                //check the maximum score records showing
                if(ScoreUICounter == MaximumRecords)
                {
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //check the input of return to menu
        if (_CrossSceneScoreManager && Input.GetButtonDown("SwitchScene"))
        {
            _CrossSceneScoreManager.StartLoadScene(StartSceneName);
        }
    }
}
