using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuInputHandler : MonoBehaviour
{
    public string MainGameSceneName = "MainGame";
    private HighestScore _CrossSceneScoreManager;
    // Start is called before the first frame update
    void Start()
    {
        _CrossSceneScoreManager = GameObject.FindGameObjectWithTag("ScoreTracker").GetComponent<HighestScore>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_CrossSceneScoreManager && Input.GetButtonDown("SwitchScene"))
        {
            _CrossSceneScoreManager.StartLoadScene(MainGameSceneName);    
        }
    }
}
