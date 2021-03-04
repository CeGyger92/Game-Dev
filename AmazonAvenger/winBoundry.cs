using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class winBoundry : MonoBehaviour
{
    public GameManager gm;
    String temp;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(gm.keycards == 5)
        {
            GetComponent<Collider2D>().gameObject.SetActive(false);
        }
        else
        {
            gm.GameOver(3);
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
