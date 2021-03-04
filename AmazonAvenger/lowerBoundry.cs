using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lowerBoundry : MonoBehaviour
{
    public GameManager gm;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.gameObject.tag == "Player")
        {
            gm.Player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<AudioSource>().Play();
            StartCoroutine("Fall");
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(3f);
        gm.GameOver(0);
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
