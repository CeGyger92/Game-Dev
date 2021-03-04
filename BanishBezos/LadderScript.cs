using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LadderScript : MonoBehaviour
{
    public GameObject Minotaur;
    public GameObject mem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Minotaur.GetComponent<Enemies>().dieing)
        {
            mem = GameObject.Find("MemoryCard");
            mem.GetComponent<MemoryCard>().saveState();
            SceneManager.LoadScene("Dungeon");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
