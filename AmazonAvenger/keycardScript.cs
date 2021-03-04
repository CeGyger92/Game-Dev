using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keycardScript : MonoBehaviour
{
    public GameManager gm;
    public GameObject chime;
    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(col.transform.name);
        if (col.transform.tag == "Player")
        {
            //Debug.Log("HIT IT");
            chime.GetComponent<AudioSource>().Play();
            gm.keycards++;
            gm.updateKeycards();
            this.gameObject.SetActive(false);
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
