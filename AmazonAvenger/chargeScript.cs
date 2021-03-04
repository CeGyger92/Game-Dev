using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chargeScript : MonoBehaviour
{
    public GameManager gm;
    bool used = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!used && gm.battStage < 4)
        {
            GetComponent<AudioSource>().Play();
            gm.battStage = 4;
            gm.updateBattery();
            GetComponent<SpriteRenderer>().color = new Color(0.4791972f, 0.8679245f, 0.4053043f, .4f);
            used = true;
            Invoke("ResetThis", 10f);
        }
    }
    public void ResetThis()
    {
        GetComponent<SpriteRenderer>().color = new Color(0.4791972f, 0.8679245f, 0.4053043f, .4f);
        used = true;
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
