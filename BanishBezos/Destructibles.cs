using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructibles : MonoBehaviour
{
    public GameObject loot;
    Animator anim;
    int timesHit = 0;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int x)
    {
        if(x > 0)
        {
            anim.SetTrigger("Hurt");
            timesHit++;
        }
        if(timesHit >= 2)
        {
            anim.SetTrigger("Destroy");
        }
    }

    public void Loot()
    {
        Instantiate(loot, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
