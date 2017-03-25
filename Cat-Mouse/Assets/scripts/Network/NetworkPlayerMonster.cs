using UnityEngine;
using System.Collections;

public class NetworkPlayerMonster : Photon.MonoBehaviour {
    Vector3 rPosition = new Vector3(0, 0, 0);
    Quaternion rRotation = Quaternion.identity;
  //  private Animator animator;
    // Use this for initialization
    void Start()
    {
      //  animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {

        }
        else
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
            stream.SendNext(GetComponent<Animator>().GetBool("Death"));
            stream.SendNext(GetComponent<Animator>().GetBool("WalkForward"));

        }
        else
        {
            this.rPosition = (Vector3)stream.ReceiveNext();
            this.rRotation = (Quaternion)stream.ReceiveNext();
            GetComponent<Animator>().SetBool("Death", (bool)stream.ReceiveNext());
            GetComponent<Animator>().SetBool("WalkForward", (bool)stream.ReceiveNext());

        }
    }
}
