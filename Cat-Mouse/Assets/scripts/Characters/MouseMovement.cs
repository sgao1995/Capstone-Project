using UnityEngine;
using System.Collections;

public class MouseMovement : MonoBehaviour {
    public float speed = 10.0F;
    public float gravity = 20.0F;
    private Vector3 direction = Vector3.zero;
    // Use this for initialization
    public Rigidbody cs;
	void Start () {
        cs = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	//if (cs.isGrounded) //checks for if mouse is on the ground.
      //  {
            direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            direction = transform.TransformDirection(direction);
            direction = direction * speed;
       // }
        cs.AddForce(direction * Time.deltaTime);
	}
}
