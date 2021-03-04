using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flames : MonoBehaviour
{
    float delay = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (Time.time > delay)
            {
                collision.GetComponent<PlayerMovement>().TakeDamage(5);
                delay = Time.time + 1.5f;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
