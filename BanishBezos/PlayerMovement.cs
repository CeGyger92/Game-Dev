using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator anim;
    float hMove = 0;
    float vMove = 0;
    float moveSpeed = 40f;
    public int health = 100;

    public Transform attackPnt;
    public float attackRange;
    public float attackDelay = 2f;
    float nextAttack = 0f;
    public int attckDmg = 25;
    public LayerMask enemyLayers;
    public bool dieing = false;
    public bool frozen = false;

    public int gold = 0;
    bool hitItem = false;
    public bool redGem = false;
    public bool greenGem = false;
    public bool blueGem = false;

    public Text goldCount;
    public Image[] healthDisplay;
    GameObject mem;
    public GameObject button;
    public GameObject uI;
    AudioSource[] clips;

    // Start is called before the first frame update
    void Start()
    {
        healthDisplay = GameObject.Find("Health").GetComponentsInChildren<Image>();
        mem = GameObject.Find("MemoryCard");
        if(mem != null && SceneManager.GetActiveScene().name != "GateKeeping")
        {
            gold = mem.GetComponent<MemoryCard>().gold;
            goldCount.text = gold.ToString();
            health = mem.GetComponent<MemoryCard>().health;
            updateHealth();
        }
        clips = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        hMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
        vMove = Input.GetAxisRaw("Vertical") * moveSpeed/4;
        if ((Mathf.Abs(hMove) > .01 || Mathf.Abs(vMove) > .01) && !frozen)
        {
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", false);
        }
        if(Time.time >= nextAttack)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !frozen && !dieing)
            {
                Attack();
                nextAttack = Time.time + 1f / attackDelay;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if(!dieing)anim.SetTrigger("Hurt");
        clips[3].Play();
        health -= damage;
        updateHealth();
        if(health <= 0 && !dieing)
        {
            Die();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hitItem) return;
        
        
        if (collision.transform.CompareTag("Gold"))
        {
            clips[0].Play();
            hitItem = true;
            gold += 5;
            goldCount.text = gold.ToString();
            Destroy(collision.gameObject);
        }
        if (collision.transform.CompareTag("Chicken"))
        {
            clips[1].Play();
            hitItem = true;
            if ((health + 25) > 100)
            {
                health = 100;
            }
            else
            {
                health += 25;
            }
            updateHealth();
            Destroy(collision.gameObject);
        }
        if (collision.transform.CompareTag("BlueGem"))
        {
            clips[2].Play();
            blueGem = true;
            Destroy(collision.gameObject);
        }
        if (collision.transform.CompareTag("GreenGem"))
        {
            clips[2].Play();
            greenGem = true;
            Destroy(collision.gameObject);
        }
        if (collision.transform.CompareTag("RedGem"))
        {
            clips[2].Play();
            redGem = true;
            Destroy(collision.gameObject);
        }
        StartCoroutine("preventDouble");
    }

    IEnumerator preventDouble()
    {
        yield return new WaitForSeconds(1f);
        hitItem = false;
    }

    void updateHealth()
    {
        int damageTaken = 100 - health;

        for(int i = 1; i < 6 ;i++)
        {
            float scale = 1;
            while(scale > 0 && damageTaken > 0)
            {
                damageTaken -= 5;
                scale -= .25f;
            }
            healthDisplay[i].transform.localScale = new Vector3(scale, scale);
        }
    }

    void Die()
    {
        dieing = true;
        anim.SetTrigger("Death");
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        StartCoroutine("Wait");
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        GameOver(1);
    }

    public void GameOver(int endCode)
    {
        if (endCode == 1)
        {
            button.GetComponent<Button>().onClick.AddListener(Reload);
            button.transform.parent.transform.gameObject.SetActive(true);
        }
        else
        {
            uI.GetComponentsInChildren<Text>()[1].text = "You Did It!!\n\nYou defeated Bezos and shunted him back to his realm. Hopefully he was teleported directly back to his cell.\n\nHopefully...";
            button.transform.parent.transform.gameObject.SetActive(true);
            clips[6].Play();
            button.gameObject.SetActive(false);
        }
    }

    void Reload()
    {
        SceneManager.LoadScene("GateKeeping");
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPnt.position, attackRange, enemyLayers);
        int enemiesHit = 0;
        foreach (Collider2D enemy in hitEnemies)
        {
            if (!enemy.isTrigger)
            {
                enemiesHit++;
                if (enemy.CompareTag("Enemy") && enemy.name == "WizardBezos") enemy.GetComponent<Bezos>().TakeDamage(attckDmg);
                if (enemy.CompareTag("Enemy") && enemy.name != "WizardBezos") enemy.GetComponent<Enemies>().TakeDamage(attckDmg);
                if (enemy.CompareTag("Item")) enemy.GetComponent<Destructibles>().TakeDamage(attckDmg);
            }
        }
        if (enemiesHit == 0)
        {
            clips[4].Play();
        }
        else
        {
            clips[5].Play();
        }       
    }

    private void OnDrawGizmos()
    {
        if (attackPnt == null) return;
        Gizmos.DrawWireSphere(attackPnt.position, attackRange);
    }

    private void FixedUpdate()
    {
        if(!frozen && !dieing)controller.Move(hMove * Time.fixedDeltaTime, vMove * Time.fixedDeltaTime, false, false);
    }
}
