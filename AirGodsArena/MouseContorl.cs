using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;




namespace AirGods.OmegaI.Com
{
    public class MouseContorl : MonoBehaviourPun
    {
        public float speed = 5f;
        Rigidbody m_RigidBody;
        float hMove;
        float vMove;
        GameManager gm;
        AudioSource hit;
        // Use this for initialization1
        void Start()
        {
            gm = FindObjectOfType<GameManager>();
            m_RigidBody = GetComponent<Rigidbody>();
            hit = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if ((photonView.IsMine == false && PhotonNetwork.IsConnected == true) || gm.gameOver)
            {
                return;
            }
            hMove = Input.GetAxis("Horizontal") * speed;
            vMove = Input.GetAxis("Vertical") * speed;
            if (this.transform.position.x > -10)
            {
                hMove *= -1;
                vMove *= -1;
            }
        }

        private void FixedUpdate()
        {
            m_RigidBody.velocity += new Vector3(vMove, 0f, hMove * -1);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            if (collision.transform.CompareTag("Puck"))
            {
                hit.Play();
            }
        }

    }
}