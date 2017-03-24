using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {
    Vector3 rPosition = new Vector3(0,0,0);
    Quaternion rRotation = Quaternion.identity;
    private Animator animator;
    int useThisMove=-1;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        if (photonView.isMine)
        {

        }else
        {
            transform.position = Vector3.Lerp(transform.position, this.rPosition, 10f * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.rRotation, 10f * Time.deltaTime);
        }
     
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(animator.GetBool("Death"));
            stream.SendNext(animator.GetBool("WalkForward"));

        }
        else
        {
            this.rPosition = (Vector3)stream.ReceiveNext();
            this.rRotation = (Quaternion)stream.ReceiveNext();
            animator.SetBool("Death", (bool)stream.ReceiveNext());
            animator.SetBool("WalkForward", (bool)stream.ReceiveNext());

        }
    }
}
