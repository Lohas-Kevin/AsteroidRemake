using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //This is the class to manager the game
    
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

    private int _RemainingLives;
    private int _Score = 0;
    private List<GameObject> _AstPool = new List<GameObject>();
    private List<GameObject> _SpawnPoints;
    private bool _GameOver = false;
    private GameObject _PlayerRef;
    
    // Start is called before the first frame update
    void Start()
    {
        _RemainingLives = AvaliableLives;
        UpdateScore(_Score);
        UpdateLives(_RemainingLives);

        //looking for the spawn points
        _SpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("SpawnPoint"));

        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart()
    {
        //the _GameOver should be true, or the game will not be started
        if(_GameOver == true)
        {
            print("ERROR: Wrong game state");
            return;
        }

        //reset the data
        _RemainingLives = AvaliableLives;
        _Score = 0;
        _GameOver = false;

        //Spawn the player
        if (!PlayerPrefab)
        {
            print("ERROR: PlayerPrefab is null");
            return;
        }
        _PlayerRef = Instantiate(PlayerPrefab);
        //Spawn the Asts
        
        List<GameObject> UsedSpawnPoints = new List<GameObject>();
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
        }
        
    }
}
