using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    public GameManager gm;
    bool attacking = false;
    Vector3 target;
    Animator animator;
    bool turned = false;
    public bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
    }

    public void die()
    {
        StopCoroutine("Attack");
        attacking = false;
        animator.SetTrigger("killed");
        StartCoroutine("cleanUp");
        GetComponents<AudioSource>()[1].Play();
    }

    IEnumerator cleanUp()
    {
        yield return new WaitForSeconds(1.5f);
        gm.enemiesInScene--;
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!attacking && !dead)
        {
            attacking = true;
            StartCoroutine("Attack");
            //Debug.Log("here");
        }
    }


    public IEnumerator Attack()
    {       
        while (attacking)
        {
            animator.SetTrigger("shoot");
            yield return new WaitForSeconds(.2f);
            float rand = Random.Range(1f, 9f);
            if(rand <= 3)
            {
                target = gm.Player.transform.position;
                if(transform.localScale.x < 0 && target.x > transform.localPosition.x)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1f, 1);
                    turned = true;
                }
                if (transform.localScale.x > 0 && target.x < transform.localPosition.x)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1f, 1);
                    turned = true;
                }
            }
            else
            {
                target = gm.PoliceStation.transform.position;
            }
            if (!attacking) break;
            GameObject bullet = Instantiate(gm.enemyBullet, this.transform.GetChild(0).transform.position, Quaternion.identity);
            GetComponents<AudioSource>()[0].Play();
            bullet.GetComponent<Rigidbody2D>().velocity = (target - transform.position).normalized * 5f;
            yield return new WaitForSeconds(gm.fireRate);
            if (turned)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1f, 1);
                turned = false;
            }
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
