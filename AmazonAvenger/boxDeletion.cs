using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxDeletion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Hazard")
        {
            GetComponent<AudioSource>().Play();
            Destroy(other.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
