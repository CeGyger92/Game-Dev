using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freeBoxX : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D col)
    {
        col.transform.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
