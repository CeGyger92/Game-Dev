using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemies : MonoBehaviour
{
    public int health = 100;
    public Transform target;
    public float speed = 200f;
    public float nextWaypoint = 1f;
    public Transform home;

    public Transform attackPnt;
    public float attackRange = 1f;
    public LayerMask player;
    public float attackDelay = 2f;
    float nextAttack = 0f;
    Path path;
    int currentWaypoint = 0;
    bool reachedPath = false;
    public bool dieing = false;
    public int attckDamage = 20;

    public GameObject beam;
    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = home;
        //StartCoroutine("SeekEnemy");
    }

    IEnumerator SeekEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            UpdatePath();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.transform;
            StartCoroutine("SeekEnemy");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
        if(target == home)
        {
            StopCoroutine("SeekEnemy");
        }
    }

    void Attack()
    {
        if (beam != null) beamAttack();
        GetComponent<Animator>().SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPnt.position, attackRange, player);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<PlayerMovement>().TakeDamage(attckDamage);
        }

    }

    public void beamAttack()
    {
        beam.SetActive(true);
    }


    private void OnDrawGizmos()
    {
        if (attackPnt == null) return;
        Gizmos.DrawWireSphere(attackPnt.position, attackRange);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        GetComponent<Animator>().SetTrigger("Hurt");

        if(health <= 0)
        {
            Die();
        }
        
    }

    public void Die()
    {
        dieing = true;
        GetComponent<Animator>().SetTrigger("Death");
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
               
    }

    public void Remove()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.time >= nextAttack)
        {
            if (Vector3.Distance(this.transform.position, target.position) <= attackRange && !dieing && target.CompareTag("Player") && !target.GetComponent<PlayerMovement>().dieing)
            {
                Attack();
                nextAttack = Time.time + 1f / attackDelay;
            }
        }
        if (Mathf.Abs(transform.GetComponent<Rigidbody2D>().velocity.x) > .05 || Mathf.Abs(transform.GetComponent<Rigidbody2D>().velocity.y) > .05)
        {
            GetComponent<Animator>().SetBool("Moving", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("Moving", false);
        }

        if(Vector3.Distance(transform.position, home.position) > 5)
        {
            target = home;
        }
    }

    void FixedUpdate()
    {
        if(path == null)
        {
            return;
        }
        else
        {
            if(currentWaypoint >= path.vectorPath.Count)
            {
                reachedPath = true;
                return;
            }
            else
            {
                reachedPath = false;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if(distance < nextWaypoint)
            {
                currentWaypoint++;
            }
        }

        if(transform.position.x < target.transform.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

    }
}
