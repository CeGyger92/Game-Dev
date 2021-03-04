using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tapeScript : MonoBehaviour
{
    public float speed = -2f;
    Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(speed, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
