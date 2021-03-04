using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Bezos : MonoBehaviour
{
    public int health = 200;
    public Transform target;
    public float speed = 200f;
    public float nextWaypoint = 1f;

    public Transform attackPnt;
    public float attackRange = 1f;
    public LayerMask player;
    public GameObject dwarf;
    public float attackDelay = 2f;
    float nextAttack = 0f;
    Path path;
    int currentWaypoint = 0;
    bool reachedPath = false;
    public bool dieing = false;
    public int attckDamage = 20;
    public GameObject fireBalls;
    public GameObject fireCircle;
    public GameObject scene;
    public float fballSpeed = 5f;
    public Transform[] BezosWaypoints;
    bool firstStage = true;
    bool finalStage = false;
    int currentTransform = 3;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        
    }


    IEnumerator SeekEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            UpdatePath();
        }
    }

    IEnumerator FireBalls()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            GameObject fBall = Instantiate(fireBalls, attackPnt.position, Quaternion.identity);
            GetComponents<AudioSource>()[0].Play();
            fBall.GetComponent<Rigidbody2D>().velocity = (dwarf.transform.position - transform.position).normalized * fballSpeed;
            Vector2 v = fBall.GetComponent<Rigidbody2D>().velocity;
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            fBall.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    IEnumerator FinalStage()
    {
        fireCircle.SetActive(true);
        while (true)
        {
            yield return new WaitForSeconds(6f);
            fireCircle.SetActive(false);
            firstStage = true;
            yield return new WaitForSeconds(3f);
            firstStage = false;
            fireCircle.SetActive(true);
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
        if(currentTransform < 3 && target == BezosWaypoints[currentTransform])
        {
            StopCoroutine("SeekEnemy");
            target = dwarf.transform;
            StartCoroutine("FireBalls");
            if (finalStage)
            {
                StartCoroutine("FinalStage");
            }
        }
    }

    void Attack()
    {
        GetComponent<Animator>().SetTrigger("Attack");
        GetComponents<AudioSource>()[1].Play();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPnt.position, attackRange, player);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<PlayerMovement>().TakeDamage(attckDamage);
        }

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
        GetComponents<AudioSource>()[2].Play();

        if (health <= 150 && health > 125)
        {
            firstStage = false;
            selectDestination();
        }
        if(health <= 125 && health > 75)
        {
            StopCoroutine("FireBalls");
            selectDestination();
            StartCoroutine("SeekEnemy");
        }
        if (health <= 75 && health > 0)
        {
            StopCoroutine("FireBalls");
            target = BezosWaypoints[0];
            currentTransform = 0;
            finalStage = true;
            StartCoroutine("SeekEnemy");
        }
        if (health <= 0)
        {
            Die();
        }

    }

    private void selectDestination()
    {
        int rand = Random.Range(0, 3);
        while (rand == currentTransform)
        {
            rand = Random.Range(0, 3);
        }
        target = BezosWaypoints[rand];
        currentTransform = rand;
    }

    public void Die()
    {
        StopAllCoroutines();
        GetComponents<AudioSource>()[3].Play();
        dieing = true;
        GetComponent<Animator>().SetTrigger("Death");
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        scene.GetComponent<DungeonScene>().StartCoroutine("Dialogue2");
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
            if (Vector3.Distance(this.transform.position, target.position) <= .5f && !dieing && target.CompareTag("Player") && !target.GetComponent<PlayerMovement>().dieing && firstStage)
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

    }

    void FixedUpdate()
    {
        if (transform.position.x < target.transform.position.x && !dieing)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if(transform.position.x > target.transform.position.x && !dieing)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        if (path == null)
        {
            return;
        }
        else
        {
            if (currentWaypoint >= path.vectorPath.Count)
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

            if (distance < nextWaypoint)
            {
                currentWaypoint++;
            }
        }

    }
}
