using UnityEngine;
using System.Collections;

public class CatMovement : MonoBehaviour
{
    public float speed = 15.0f; //speed value
    private Vector3 moveV; //vector to store movement
    public Rigidbody catrb;
    private int jumpForce = 3;//amount of jump force
    private bool isJumping; //flag to check if user is already jumping or not
    void Start()
    {
        catrb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; //cursor is gone from screen
    }

    void FixedUpdate()
    {
        moveV.Set(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")); //set vector3 with wasd

        moveV = moveV.normalized * speed * Time.deltaTime; //multiply the normalized vector with speed and delta time
     
        transform.Translate(moveV); //translate cat obj based on moveV vector
        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None; //if we press esc, cursor appears on screen
        }
        //jump control
        if (Input.GetKeyDown(KeyCode.Space) && isJumping == false)//if user press space and is not already jumping
        {
            catrb.velocity = new Vector3(0, jumpForce, 0);
        }
        isJumping = true;

    }
    void OnCollisionStay()
    {
        isJumping = false;   
    }
}