using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the class to manager the game
//This class handles the game flow and game basic settings
//This class also handles the next scene input
public class GameManager : MonoBehaviour
{
    //The boundaries of the game space
    public GameObject TopRightCorner;
    public GameObject BottomLeftCorner;
    public GameObject AstPrefab;
    public GameObject PlayerPrefab;

    
    public GameplayUI GameUI;

    public Vector3 PlayerRespawnPos = Vector3.zero;
    public int MaximumLargeAst = 5;
    public int AvaliableLives = 3;

    //This is the delta for out of bound condition
    //the ship will be set inside the bound
    //this delta defines the distance between the spaceship and boundary
    public float OutOfBoundraryDelta = 0.05f;

    public string LeaderboardSceneName = "Leaderboard";

    private int _RemainingLives;
    private int _Score = 0;
    private List<GameObject> _AstPool = new List<GameObject>();
    private List<GameObject> _SpawnPoints;
    private bool _GameOver = false;
    private GameObject _PlayerRef;
    private HighestScore _HighestScoreTracker;

    // Start is called before the first frame update
    void Start()
    {
        _RemainingLives = AvaliableLives;
        UpdateScore(_Score);
        UpdateLives(_RemainingLives);

        //looking for the spawn points
        _SpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("SpawnPoint"));

        //looking for the score tracker
        _HighestScoreTracker = GameObject.FindGameObjectWithTag("ScoreTracker").GetComponent<HighestScore>();

        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        if(_GameOver && Input.GetButtonDown("SwitchScene"))
        {
            //jump to leader board scene
            if (_HighestScoreTracker)
            {
                _HighestScoreTracker.StartLoadScene(LeaderboardSceneName);
            }
        }
    }

    public void GameStart()
    {
        //reset the data
        _RemainingLives = AvaliableLives;
        _Score = 0;
        _GameOver = false;

        //reset the GameUI
        UpdateScore(_Score);
        UpdateLives(_RemainingLives);

        //Spawn the player
        if (!PlayerPrefab)
        {
            print("ERROR: PlayerPrefab is null");
            return;
        }
        _PlayerRef = Instantiate(PlayerPrefab);
        //Spawn the Asts
        
        List<GameObject> UsedSpawnPoints = new List<GameObject>();
        if(_AstPool.Count != 0)
        {
            _AstPool.Clear();
        }
        for(int i = 0; i < MaximumLargeAst; i++)
        {
            GameObject RandSpawnPoint = _SpawnPoints[Random.Range(0, _SpawnPoints.Count)];
            UsedSpawnPoints.Add(RandSpawnPoint);
            _SpawnPoints.Remove(RandSpawnPoint);

            GameObject SpawndAst = Instantiate(AstPrefab, RandSpawnPoint.transform.position, RandSpawnPoint.transform.rotation);
            _AstPool.Add(SpawndAst);
        }
        //restore the used spawn points back to the spawn pool
        _SpawnPoints.AddRange(UsedSpawnPoints);
        UsedSpawnPoints.Clear();
    }

    public void AstDestroyed(GameObject AstObj, Asteroid Ast)
    {
        //if an ast is completely destroyed (after 3 splits)
        //remove this Ast from the AstPool and Spawn a new Ast
        GameObject RandSpawnPoint = _SpawnPoints[Random.Range(0, _SpawnPoints.Count)];
        AstObj.transform.SetPositionAndRotation(RandSpawnPoint.transform.position, RandSpawnPoint.transform.rotation);
        Ast.InitialAstInstantiate();
    }

    public Vector3 GetTopRightCood()
    {
        if (!TopRightCorner)
        {
            return Vector3.zero;
        }
        return TopRightCorner.transform.position;
    }
    
    public Vector3 GetBottomLeftCood()
    {
        if (!BottomLeftCorner)
        {
            return Vector3.zero;
        }
        return BottomLeftCorner.transform.position;
    }

    public Vector3 CheckAndModCood(Vector3 InCood)
    {
        Vector3 Result = InCood;
        //Get the boundary of the game space

        Vector3 TRCorner = GetTopRightCood();
        Vector3 BLCorner = GetBottomLeftCood();

        //check the x-axis cood
        if (InCood.x > TRCorner.x)
        {
            //go over right boundary, set to left boundary
            //add a small delta to avoid the problem with comparison
            Result.x = BLCorner.x + OutOfBoundraryDelta;
        }
        else if (InCood.x < BLCorner.x)
        {
            //go over left boundary, set to Right boundary
            //add a small delta to avoid the problem with comparison
            Result.x = TRCorner.x - OutOfBoundraryDelta;
        }

        //check the z-axis cood
        if (InCood.z > TRCorner.z)
        {
            //go over top boundary, set to bottom boundary
            //add a small delta to avoid the problem with comparison
            Result.z = BLCorner.z + OutOfBoundraryDelta;
        }
        else if (InCood.z < BLCorner.z)
        {
            //go over bottom boundary, set to top boundary
            //add a small delta to avoid the problem with comparison
            Result.z = TRCorner.z - OutOfBoundraryDelta;
        }

        return Result;
    }

    public void UpdateScore(int Score)
    {
        GameUI.UpdateScoreUI(Score);
    }

    public void UpdateLives(int Lives)
    {
        GameUI.UpdateLivesUI(Lives);
    }

    public void AstDestroyed(int Score)
    {
        _Score += Score;
        GameUI.UpdateScoreUI(_Score);
    }

    public void ShipDestroyed()
    {
        if(_RemainingLives > 0)
        {
            _RemainingLives -= 1;
        }
        GameUI.UpdateLivesUI(_RemainingLives);

        //reset the ship if there are reamining lives
        if(_RemainingLives > 0)
        {
            _PlayerRef.transform.position = Vector3.zero;
            _PlayerRef.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            Rigidbody Rig = _PlayerRef.GetComponent<Rigidbody>();
            if (Rig)
            {
                Rig.velocity = Vector3.zero;
                Rig.angularVelocity = Vector3.zero;
            }
        }

        if(_RemainingLives == 0)
        {
            Destroy(_PlayerRef);
            _GameOver = true;

            //destroy the asts in the scene
            foreach(GameObject GO in _AstPool)
            {
                Destroy(GO);
            }
            _AstPool.Clear();

            //set up the game over screen
            GameUI.ActivateGameOverUI(true);

            //Send the Score to Score Tracker
            //we don't send "0" to the highest Score
            if (!_HighestScoreTracker)
            {
                _HighestScoreTracker = GameObject.FindGameObjectWithTag("ScoreTracker").GetComponent<HighestScore>();
            }
            if (_HighestScoreTracker && _Score != 0)
            {
                _HighestScoreTracker.AddScoreToTheBoard(_Score);
            }
        }    
    }
}
