using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinMover : MonoBehaviour
{
    public float maxSpeed = 2f;
    public float minSpeed = -2f;

    Rigidbody2D rbody;
    // Start is called before the first frame update
    void Start()
    {
        float speedOne = 0;
        float speedTwo = 0;
        while(speedOne == 0 && speedTwo == 0)
        {
            speedOne = Random.Range(minSpeed, maxSpeed);
            speedTwo = Random.Range(minSpeed, maxSpeed);
        }
        rbody = GetComponent<Rigidbody2D>();
        rbody.velocity = new Vector2(speedOne, speedTwo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
