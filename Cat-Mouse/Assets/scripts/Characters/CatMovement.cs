using UnityEngine;
using System.Collections;

public class CatMovement : MonoBehaviour {
    public float speed = 10.0F;
    // Use this for initialization
    public Rigidbody cs;
    void Start()
    {
        cs = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalA = Input.GetAxis("Horizontal");
        float verticalA = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontalA, 0, verticalA);
        move = move * speed * Time.deltaTime;
        cs.MovePosition(transform.position + move);
    }
}
