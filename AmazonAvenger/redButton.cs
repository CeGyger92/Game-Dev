using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redButton : MonoBehaviour
{
    public GameManager gm;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<AudioSource>().Play();
        gm.GameOver(2);
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
