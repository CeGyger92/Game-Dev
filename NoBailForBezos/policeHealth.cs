using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class policeHealth : MonoBehaviour
{
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "bullet")
        {
            gm.lowerHealth();
            Destroy(collision.transform.gameObject);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
