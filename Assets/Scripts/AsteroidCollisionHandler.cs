using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The class to handles the collision of asteroid
public class AsteroidCollisionHandler : MonoBehaviour
{
    private AsteroidType _AstType;
    private Asteroid _ParentAstManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_ParentAstManager)
        {
            if(collision.gameObject.tag == "Bullet")
            {
                _ParentAstManager.DestroyedByBullet(this.gameObject, this._AstType);
            }
        }
    }

    public AsteroidType GetAstType()
    {
        return _AstType;
    }

    public void SetAstType(AsteroidType InAstType)
    {
        _AstType = InAstType;
    }

    public void SetAstManager(Asteroid Manager)
    {
        _ParentAstManager = Manager;
    }
}
