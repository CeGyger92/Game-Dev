using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;


namespace AirGods.OmegaI.Com
{
    public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public float currentPlayers;
        public Camera pOne;
        public Camera pTwo;
        public int bScore = 0;
        public int yScore = 0;
        public Text yellowScore;
        public Text blueScore;
        public bool gameOver = false;
        public GameObject puck;
        public GameObject gamePuck;
        public Text winText;
        public GameObject WinPanel;
        public GameObject ExitPanel;
        public GameObject ScorePanel;
        public Text[] nicknames;
        AudioSource [] SFX;
        int startingTrack;

        public Transform puckPos;
        

        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting == true)
            {
                stream.SendNext(currentPlayers);
            }
            else
            {
                currentPlayers = (float)stream.ReceiveNext();
            }
        }

        #endregion

        #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }


        #endregion

        #region Public Fields

        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        #endregion

        #region Public Methods


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void updateScore()
        {
            yellowScore.text = (yScore).ToString();
            blueScore.text = (bScore).ToString();
            if(bScore >= 3)
            {
                GameOver(1);
            }else if(yScore >= 3)
            {
                GameOver(0);
            }
        }

        public void placePuck()
        {
            gamePuck = GameObject.Find("Puck(Clone)");
            gamePuck.SetActive(false);
            gamePuck.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gamePuck.transform.position = puckPos.position;
            gamePuck.SetActive(true);
        }


        public void GameOver(int winner)
        {
            WinPanel.SetActive(true);
            gameOver = true;
            Destroy(gamePuck.gameObject, 2);
            if (winner == 1)
            {
                SFX[4].Play();
                winText.text = "BLUE IS THE VICTOR!!";
            }
            else
            {
                SFX[3].Play();
                winText.text = "YELLOW IS THE VICTOR!!";
            }
        }

        [PunRPC]
        void startMatch(int track)
        {
            startingTrack = track;
            SFX[startingTrack].Play();
            StartCoroutine("DelayInput");
        }

        IEnumerator DelayInput()
        {
            yield return new WaitWhile(() => SFX[startingTrack].isPlaying);
            gameOver = false;
        }

        #endregion

        #region Private Methods


        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", 2);
            PhotonNetwork.LoadLevel("Room for " + "2");
        }


        #endregion

        #region Photon Callbacks


        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
            currentPlayers++;
            nicknames[1].text = other.NickName;
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                //LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }


        #endregion

        private void Start()
        {
            Physics.IgnoreLayerCollision(8, 9);
            gameOver = true;
            currentPlayers++;
            SFX = GetComponents<AudioSource>();
            
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                if(PhotonNetwork.IsMasterClient)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    GameObject P1  = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(-36f, 6f, 10f), Quaternion.Euler(new Vector3(-90f, 0f, 0f)), 0);
                    nicknames[0].text = P1.GetComponent<PhotonView>().Owner.NickName;
                }
                else
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    GameObject P2 = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(16f, 6f, 10f), Quaternion.Euler(new Vector3(-90f, 0f, 0f)), 0);
                    nicknames[1].text = P2.GetComponent<PhotonView>().Owner.NickName;
                    gamePuck = PhotonNetwork.Instantiate(this.puck.name, new Vector3(-12f, 6f, 10f), Quaternion.Euler(new Vector3(-90f, 0f, 0f)), 0);
                    pOne.gameObject.SetActive(false);
                    pTwo.gameObject.SetActive(true);
                    int i = 0;
                    foreach (Player p in PhotonNetwork.PlayerList)
                    {
                        if(i == 0)
                        {
                            nicknames[i].text = p.NickName;
                        }
                        i++;
                    }
                    int rand = 0; //Random.Range(0, 3);
                    PhotonView.Get(this).RPC("startMatch", RpcTarget.All, rand);
                }
                
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (ExitPanel.activeSelf)
                {
                    ScorePanel.SetActive(true);
                    ExitPanel.SetActive(false);
                }
                else
                {
                    ScorePanel.SetActive(false);
                    ExitPanel.SetActive(true);
                }
            }
        }
    }
}
