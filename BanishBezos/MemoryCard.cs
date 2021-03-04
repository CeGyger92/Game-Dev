using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MemoryCard : MonoBehaviour
{
    public GameObject player;
    public int gold;
    public int health;
    PlayerMovement settings;
   
    private static MemoryCard mem;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (mem == null)
        {
            mem = this;
        }
        else
        {
            Destroy(mem.gameObject);
        }
    }


    void Start()
    {
        settings = player.GetComponent<PlayerMovement>();
    }



    public void saveState()
    {
        gold = settings.gold;
        health = settings.health;        
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
