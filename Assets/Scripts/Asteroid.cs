using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AsteroidType
{
    Large = 0, Medium = 1, Small = 2
};

//This is the manager class of the Asteroids
//This class will manage the spawn of asteroids with different size
//Designers are able to config the asteroids here
public class Asteroid : MonoBehaviour
{
    public GameObject LargeAst;
    public GameObject MediumAst;
    public GameObject SmallAst;
    public float MaximumRandomSpeed = 15.0f;
    public float MinimumRandomSpeed = 5.0f;
    public int MediumAstGenerated = 1;
    public int SmallAstGenerated = 3;
    public int LargeAstScore = 250;
    public int MediumAstScore = 125;
    public int SmallAstScore = 75;

    private List<GameObject> _AstRefPool = new List<GameObject>();
    private GameManager _GameManager;
    // Start is called before the first frame update
    void Start()
    {
        InitialAstInstantiate();
        _GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //check whether the ast is out of bound
        if (_GameManager)
        {
            foreach(GameObject GB in _AstRefPool)
            {
                if (GB)
                {
                    GB.transform.position = _GameManager.CheckAndModCood(GB.transform.position);
                } 
            }
           
        }
    }

    //This function is for GameManager to instantiate a Astroid
    public void InitialAstInstantiate()
    {
        //instantiate a large Ast here
        GameObject InstantiatedGB = Instantiate(LargeAst, this.gameObject.transform);
        AsteroidCollisionHandler Handler = InstantiatedGB.GetComponent<AsteroidCollisionHandler>();
        if (Handler)
        { 
            //set the instantiated ast type and Asteroid reference
            Handler.SetAstType(AsteroidType.Large);
            Handler.SetAstManager(this);
        }
        _AstRefPool.Add(InstantiatedGB);

        //give the instantiated asteroid a initial velocity
        Vector3 RandomVel = new Vector3(Random.Range(-1f, 1f), 0.0f, Random.Range(-1f, 1f)).normalized;
        Rigidbody AstRig = InstantiatedGB.GetComponent<Rigidbody>();
        if (AstRig)
        {
            AstRig.velocity = RandomVel * Random.Range(MinimumRandomSpeed, MaximumRandomSpeed);
        }
    }

    //this function will destroy the ast attached to this manager
    //when an ast instance is destroyed by a bullet
    public void DestroyedByBullet(GameObject AstRef, AsteroidType AstRefType)
    {
        if(AstRefType == AsteroidType.Large)
        {
            //large Ast is destroyed
            //instante medium Ast
            for(int i = 0; i < MediumAstGenerated; i++)
            {
                AstInstantiate(AsteroidType.Medium, AstRef);
            }

            //update score
            _GameManager.AstDestroyed(LargeAstScore);
            
            //remove the current ast ref from the ast pool and destroy teh ast
            _AstRefPool.Remove(AstRef);
            Destroy(AstRef);
        }
        else if(AstRefType == AsteroidType.Medium)
        {
            for (int i = 0; i < SmallAstGenerated; i++)
            {
                AstInstantiate(AsteroidType.Small, AstRef);
            }

            //update score
            _GameManager.AstDestroyed(MediumAstScore);

            //remove the current ast ref from the ast pool and destroy teh ast
            _AstRefPool.Remove(AstRef);
            Destroy(AstRef);
        }
        else
        {
            //update score
            _GameManager.AstDestroyed(SmallAstScore);

            //for small ast, removal is the only task required
            //remove the current ast ref from the ast pool and destroy teh ast
            _AstRefPool.Remove(AstRef);
            Destroy(AstRef);
        }

        //add another check to the pool, if the pool is empty, reset this Ast Manager
        if(_AstRefPool.Count == 0)
        {
            _GameManager.AstDestroyed(this.gameObject, this);
        }
    }

    //general Instantiate function for medium and small ast
    private void AstInstantiate(AsteroidType InstantAstType, GameObject AstDestroyed)
    {
        GameObject InstantiatedGB;
        
        //create ast based on the input ast type
        if(InstantAstType == AsteroidType.Medium)
        {
            InstantiatedGB = Instantiate(MediumAst, AstDestroyed.transform.position, AstDestroyed.transform.rotation, this.gameObject.transform);
        }
        else
        {
            InstantiatedGB = Instantiate(SmallAst, AstDestroyed.transform.position, AstDestroyed.transform.rotation, this.gameObject.transform);
        }
        
        AsteroidCollisionHandler Handler = InstantiatedGB.GetComponent<AsteroidCollisionHandler>();
        if (Handler)
        {
            //set the configurations of the asteroid
            Handler.SetAstType(InstantAstType);
            Handler.SetAstManager(this);
        }
        _AstRefPool.Add(InstantiatedGB);

        //give the asteroid an initial velocity
        Vector3 RandomVel = new Vector3(Random.Range(-1f, 1f), 0.0f, Random.Range(-1f, 1f)).normalized;
        Rigidbody AstRig = InstantiatedGB.GetComponent<Rigidbody>();
        if (AstRig)
        {
            AstRig.velocity = RandomVel * Random.Range(MinimumRandomSpeed, MaximumRandomSpeed);
        }
    }
}
