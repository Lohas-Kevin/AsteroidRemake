using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float AccelerateRate = 1.0f;
    public float RotateRate = 1.0f;
    public float MaximumSpeed = 50.0f;
    public float MaximumAngularSpeed = 20.0f;

    private Rigidbody _MyRig;
    private GameManager GameManager;

    // Start is called before the first frame update
    void Start()
    {
        _MyRig = this.GetComponent<Rigidbody>();
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //put other input check here

        //since boundrary check is not rigid body related, put it here
        if (GameManager)
        {
            this.transform.position = GameManager.CheckAndModCood(this.transform.position);
        }
    }

    //Physical calculations here
    private void FixedUpdate()
    {
        //put physical related input check here

        //handle the input
        if (_MyRig)
        {
            _MyRig.AddForce(CheckAcceleration(), ForceMode.Impulse);
            _MyRig.AddTorque(CheckRotation(), ForceMode.Impulse);

            if(Input.GetButtonUp("RotateCounterClockwise") || Input.GetButtonUp("RotateClockwise"))
            {
                _MyRig.angularVelocity = Vector3.zero;
            }
        }
        
        //add another check to overspeed
        if(_MyRig.velocity.magnitude > MaximumSpeed)
        {
            _MyRig.velocity = _MyRig.velocity.normalized * MaximumSpeed;
        }
        if(_MyRig.angularVelocity.magnitude > MaximumAngularSpeed)
        {
            _MyRig.angularVelocity = _MyRig.angularVelocity.normalized * MaximumAngularSpeed;
        }

        print(_MyRig.angularVelocity);
    }

    /*
     * This Function is going to handle:
     *     1.Input from players
     *     2.Avoid double Pressing ('W'and 'up' are pressed together)
     *     3.Return an Acceleration
    */
    private Vector3 CheckAcceleration()
    {
        Vector3 AccelerateResult = Vector3.zero;
        //check the input continuously
        if (Input.GetButton("Accelerate"))
        {
            Vector3 ForwardVec = this.transform.forward;

            //add the impulse here
            if (_MyRig)
            {
                //Check the speed here
                if(_MyRig.velocity.magnitude <= MaximumSpeed)
                {
                    //Add impulse here
                    AccelerateResult = ForwardVec * AccelerateRate;
                }
            }
        }
        return AccelerateResult;
    }

    /*
     * This Function is going to handle:
     *     1.Input from players
     *     2.Avoid double Pressing ('W'and 'up' are pressed together)
     *     3.Return an rotation
    */
    private Vector3 CheckRotation()
    {
        Vector3 RotateResult = Vector3.zero;
        //check is a player keep pressing buttons
        if (Input.GetButton("RotateCounterClockwise"))
        {
            //add positive value to y-axis,
            //in the game, the spaceship will rotate around the y-axis
            RotateResult -= Vector3.up * RotateRate;
        }
        if (Input.GetButton("RotateClockwise"))
        {
            //add negative value to y-axis,
            //in the game, the spaceship will rotate around the y-axis
            RotateResult += Vector3.up * RotateRate;
        }
        if(_MyRig && _MyRig.angularVelocity.magnitude > MaximumAngularSpeed)
        {
            //angular speed check
            RotateResult = Vector3.zero;
        }
        return RotateResult;
    }
}
