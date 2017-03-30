using UnityEngine;
using System.Collections;

public class MouseCamMovement : MonoBehaviour {
    Vector3 mouseMov; //vector that keeps track of mouse movement
    Vector3 smoothnessV;//vector to smooth mouse movement
    Vector3 CamPos; //starting position of the camera
    public float sensitivity = 3.0f; //mouse sensitivity
    public float smoothness = 4.0f; //smoothness value for camera movement

    GameObject character;
    private Vector3 camPosition;
    public float distBehind;
    public float distOntop;


    //variables for camera collision 
    int layerMask = ~(1 << 10); //used for collision, set to layer 10 so that anything on that layer is ignored by the linecast 


    void Start()
    {
        character = GameObject.FindWithTag("Mouse");
    }

    void LateUpdate()
    {

        CamControls();
        Vector3 offset = new Vector3(0, 0.5f, 0);
        Vector3 target = transform.parent.position + offset;
        CameraCollision(target, ref camPosition);
        transform.position = camPosition;

    }
    void CamControls()
    {
        camPosition = character.transform.position + Vector3.up * distOntop - character.transform.forward * distBehind; //set camera position
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        mx = mx * sensitivity * smoothness; //multiply by smooth and sensitivity value
        my = my * sensitivity * smoothness;
        Vector3 mousePos = new Vector3(mx, my); //get value of position of mouse, so the x and y coord
        //lerp to make movement smooth, so it isn't instant
        smoothnessV.x = Mathf.Lerp(smoothnessV.x, mousePos.x, 1f / smoothness);
        smoothnessV.y = Mathf.Lerp(smoothnessV.y, mousePos.y, 1f / smoothness);
        mouseMov = mouseMov + smoothnessV;
        //clamp to limit movement of y axis
        mouseMov.y = Mathf.Clamp(mouseMov.y, -30f, 60f);
        transform.localRotation = Quaternion.AngleAxis(-mouseMov.y, Vector3.right); //inverted rotate up and down with camera
        character.transform.localRotation = Quaternion.AngleAxis(mouseMov.x, character.transform.up); //rotate the character left/right

    }
    void CameraCollision(Vector3 target, ref Vector3 camPos) //used to check if camera is near an object, if so then we will move the camera forward
    {


        Debug.DrawLine(target, camPos, Color.cyan); //used to debug, to check the line cast

        RaycastHit hitInfo;
        if (Physics.Linecast(target, camPos, out hitInfo, layerMask)) //if there is an object between the target and camera, then we set distance to distance of where the line hit (distance is clamped to be within min and max dist values)
        {
            Debug.DrawRay(hitInfo.point, -Vector3.forward, Color.red); //used to debug, check where we hit
            camPos = new Vector3(hitInfo.point.x, camPos.y, hitInfo.point.z);

        }
    }
}
