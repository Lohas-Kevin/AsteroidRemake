using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighestScore : MonoBehaviour
{
    public int HowManyScoresOnTheLeaderboard = 5;

    private List<int> _HighestScorePool = new List<int>();
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScoreToTheBoard(int InScore)
    {
        if(_HighestScorePool.Count < HowManyScoresOnTheLeaderboard)
        {
            //leader board is not full
            _HighestScorePool.Add(InScore);
        }
        else
        {
            //There is no enough space on the board
            _HighestScorePool.Add(InScore);
            _HighestScorePool.Sort();
            _HighestScorePool.RemoveAt(_HighestScorePool.Count - 1);
        }
    }

    public List<int> RetrieveSortedHighestScore()
    {
        _HighestScorePool.Sort();
        return _HighestScorePool;
    }
}
