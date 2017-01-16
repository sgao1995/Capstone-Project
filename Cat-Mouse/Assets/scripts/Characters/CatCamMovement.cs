using UnityEngine;
using System.Collections;

public class CatCamMovement : MonoBehaviour {
    Vector3 mouseMov; //vector that keeps track of mouse movement
    Vector3 smoothnessV;//vector to smooth mouse movement
    Vector3 CamPos; //starting position of the camera
    public float sensitivity = 3.0f; //mouse sensitivity
    public float smoothness = 2.0f; //smoothness value for camera movement

    //variables for camera collision 
    public float minDist = 0f;
    public float maxDist = 4f;
    public float smoothnessCol = 5.0f; //smoothness value for collision

    Vector3 camDir;
    float distance;

    GameObject character;
    void Start()
    {
        camDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
        character = this.transform.parent.gameObject; //set the character as the parent of the camera
    }
    void Update()
    {
        CamControls();
        CameraCollision();
    }
    void CamControls()
    {
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
    void CameraCollision() //used to check if camera is near an object, if so then we will move the camera forward
    {
        Vector3 camPos = transform.TransformPoint(camDir * maxDist);
        Vector3 charPos = transform.parent.position;

        Debug.DrawLine(charPos, camPos, Color.cyan); //used to debug, to check the line cast

        RaycastHit hitInfo;
        if (Physics.Linecast(charPos, camPos, out hitInfo)) //if there is an object between the target and character, then we set distance to distance of there the line hit (distance is clamped to be within min and max dist values)
        {
            distance = Mathf.Clamp(hitInfo.distance, minDist, maxDist); 
        }
        else //if nothing is hit, we set distance to maxDist
        {
            distance = maxDist;
        }
        //change position of cam based on collision
        transform.localPosition = Vector3.Lerp(transform.localPosition, camDir*distance, Time.deltaTime*smoothnessCol);
    }
}
