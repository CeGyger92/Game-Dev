using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AirGods.OmegaI.Com
{ 
    public class TitleScreen : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }
        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}