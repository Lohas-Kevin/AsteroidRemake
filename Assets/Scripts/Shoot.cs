using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check fire input here
        if (Input.GetButtonDown("Fire"))
        {
            ShootBullet();
        }
    }

    //This function will instantiate bullet and set its velocity
    private void ShootBullet()
    {
        Rigidbody ParentRig = this.transform.parent.GetComponent<Rigidbody>();
        if (ParentRig)
        {
            GameObject CreatedGameObject = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
            Bullet BulletComp = CreatedGameObject.GetComponent<Bullet>();
            if (BulletComp)
            {
                BulletComp.SetVelocity(this.transform.parent.forward, ParentRig.velocity.magnitude);
            }
        }
    }
}
