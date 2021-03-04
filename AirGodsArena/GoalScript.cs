using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirGods.OmegaI.Com
{

    public class GoalScript : MonoBehaviour
    {
        public GameManager gm;
        AudioSource goal;
        // Start is called before the first frame update
        void Start()
        {
            goal = GetComponent<AudioSource>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Puck"))
            {
                goal.Play();
                if (this.CompareTag("YellowGoal"))
                {
                    gm.bScore++;
                }
                else if (this.CompareTag("BlueGoal"))
                {
                    gm.yScore++;
                }
                gm.updateScore();
                gm.placePuck();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
