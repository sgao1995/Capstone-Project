using UnityEngine;
using System.Collections;

public class CatCamMovement : MonoBehaviour {
    Vector3 mouseMov; //vector that keeps track of mouse movement
    Vector3 smoothnessV;//vector to smooth mouse movement
    public float sensitivity = 3.0f; //mouse sensitivity
    public float smoothness = 2.0f; //smoothness value
    GameObject cat;
    void Start()
    {
        cat = this.transform.parent.gameObject; //set cat as the parent of the camera, which is the cube
    }
    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        mx = mx * sensitivity * smoothness; //multiply by smooth and sensitivity value
        my = my * sensitivity * smoothness;
        Vector3 mousePos = new Vector3(mx, my); //get value of position of mouse, so the x and y coord
        //lerp to make movement smooth, so it isn't instant
        smoothnessV.x = Mathf.Lerp(smoothnessV.x,mousePos.x,1f/smoothness);
        smoothnessV.y = Mathf.Lerp(smoothnessV.y,mousePos.y,1f/smoothness);
        mouseMov = mouseMov + smoothnessV;
        //clamp to limit movement of y axis
        mouseMov.y = Mathf.Clamp(mouseMov.y, -30f, 0f);
        transform.localRotation = Quaternion.AngleAxis(-mouseMov.y, Vector3.right); //inverted rotate up and down with camera
        cat.transform.localRotation = Quaternion.AngleAxis(mouseMov.x, cat.transform.up); //rotate the cat left/right
    }
}
