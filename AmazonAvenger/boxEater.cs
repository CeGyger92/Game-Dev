using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxEater : MonoBehaviour
{
    public Stack<GameObject> toBeEaten;
    float timer = 0;
    bool waited = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        timer = 0;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(timer > 3.5f)
        {
            Destroy(collision.transform.gameObject);
        }
    }
    
    IEnumerator wait()
    {
        yield return new WaitForSeconds(20f);
        waited = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("wait");
    }

    // Update is called once per frame
    void Update()
    {
        if (waited)
        {
            timer += Time.deltaTime;
        }
    }
}
