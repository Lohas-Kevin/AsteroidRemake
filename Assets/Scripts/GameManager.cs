using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //This is the class to manager the game
    
    //The boundaries of the game space
    public GameObject TopRightCorner;
    public GameObject BottomLeftCorner;
    public GameplayUI GameUI;
    public int AvaliableLives = 3;

    //This is the delta for out of bound condition
    //the ship will be set inside the bound
    //this delta defines the distance between the spaceship and boundary
    public float OutOfBoundraryDelta = 0.05f;

    private int _RemainingLives;
    private int _Score = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _RemainingLives = AvaliableLives;
        UpdateScore(_Score);
        UpdateLives(_RemainingLives);
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }
}
