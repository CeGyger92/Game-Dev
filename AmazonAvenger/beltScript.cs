using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beltScript : MonoBehaviour
{
    void OnCollisionStay2D(Collision2D col)
    {
        //Debug.Log(col.transform.name);
        Rigidbody2D rigid = col.gameObject.GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(-200f, 0f));
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
