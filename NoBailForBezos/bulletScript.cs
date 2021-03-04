using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float speed = 5f;
    public float counterAngle = 2f;
    Rigidbody2D rigid;
    bool isMoving = true;
    public GameManager gm;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GameManager>();
        if (gm.Player.GetComponent<PlayerMovement>().facing == false)
        {
            speed = -5f;
        }
        else
        {
            speed = 5f;
        }
       
        rigid.velocity = new Vector2(speed, counterAngle);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        isMoving = false;
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        if(collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponent<enemyScript>().dead = true;
            collision.transform.GetComponent<enemyScript>().die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && rigid.constraints == RigidbodyConstraints2D.FreezeAll)
        {
            if(gm.ammoCount < 4)
            {
                gm.AmmoUpdate(1);
            }
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            float angle = Mathf.Atan2(rigid.velocity.y, rigid.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
