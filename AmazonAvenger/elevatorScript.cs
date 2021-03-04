using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevatorScript : MonoBehaviour
{
    float speed = 4f;
    bool moveUp = true;
    public float peak = 15;
    public float low = -15;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.parent = this.gameObject.transform;
        //collision.transform.GetComponent<Rigidbody2D>().gravityScale = 10;
        //collision.transform.gameObject.GetComponent<>()
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.parent = null;
        //collision.transform.GetComponent<Rigidbody2D>().gravityScale = 2;
    }
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine("Animate");
    }

    //IEnumerator Animate()
    //{
    //    if(this.transform.position.y < -20f)
    //    {
    //        while (this.transform.position.y < 20f)
    //        {
    //            this.transform.position += new Vector3(0f, .1f);
    //            yield return new WaitForSeconds(.01f);
    //        }
    //    }
    //    if (this.transform.position.y > 20f)
    //    {
    //        while (this.transform.position.y > -20f)
    //        {
    //            this.transform.position -= new Vector3(0f, .1f);
    //            yield return new WaitForSeconds(.01f);
    //        }
    //    }
    //    StartCoroutine("Animate");
    //}

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y > peak)
        {
            moveUp = false;
        }
        if (transform.position.y < low)
        {
            moveUp = true;
        }

        if (moveUp)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
        }
    }
}
