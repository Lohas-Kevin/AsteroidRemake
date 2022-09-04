using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float VelocityMultiplyer = 1.5f;
    public float HowLongBulletLast = 2.0f;
    public Rigidbody MyRigidBody;

    private GameManager _GameManager;
    // Start is called before the first frame update
    void Start()
    {
        _GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Do Out of bound check here
        if (_GameManager)
        {
            this.transform.position = _GameManager.CheckAndModCood(this.transform.position);
        }
    }

    public void SetVelocity(Vector3 Direction, float scale)
    {
        if (MyRigidBody)
        {
            MyRigidBody.velocity = Direction*(VelocityMultiplyer+scale);
        }

        //set a countdown for destroy
        Destroy(this.gameObject, HowLongBulletLast);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Star")
        {
            Destroy(this.gameObject);
        }
    }

}
