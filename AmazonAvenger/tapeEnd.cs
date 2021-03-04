using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tapeEnd : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag != "Player")
        {
            Destroy(collision.transform.gameObject);
        }
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
