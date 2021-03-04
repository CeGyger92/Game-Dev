using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxBanger : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log(col.transform.name);
        if (col.transform.tag == "Player")
        {
            Rigidbody2D rigid = col.gameObject.GetComponent<Rigidbody2D>();
            rigid.AddForce(new Vector2(-1000f, 0f));
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
