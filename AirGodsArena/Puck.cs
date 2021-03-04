using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace AirGods.OmegaI.Com
{

    public class Puck : MonoBehaviourPunCallbacks, IPunObservable
    {
        Rigidbody rbody;
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(rbody.position);
                stream.SendNext(rbody.rotation);
                stream.SendNext(rbody.velocity);
            }
            else
            {
                rbody.position = (Vector3)stream.ReceiveNext();
                rbody.rotation = (Quaternion)stream.ReceiveNext();
                rbody.velocity = (Vector3)stream.ReceiveNext();

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                rbody.position += rbody.velocity * lag;
            }
        }

        private void Awake()
        {
            rbody = GetComponent<Rigidbody>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
