using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chopperScript : MonoBehaviour
{
    Vector3 startingPos;
    bool right;
    public bool moving;
    GameManager gm;
    public GameObject enemy;
    bool halfway = false;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        startingPos = transform.position;
        moving = true;
        if (startingPos.x < 0)
        {
            right = true;
        }
        else
        {
            right = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.transform.tag == "PlaySpace")
        {
            //Debug.Log("entered");
            StartCoroutine("spawnEnemies");
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("exited");
        if (collision.transform.tag == "PlaySpace")
        {
            StartCoroutine("returnHome");
        }
    }

    IEnumerator returnHome()
    {
        yield return new WaitForSeconds(5f);
        moving = false;
        transform.position = startingPos;
        GetComponent<AudioSource>().Stop();
    }

    IEnumerator spawnEnemies()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            float random = Random.Range(gm.minSpawn, gm.maxSpawn);
            yield return new WaitForSeconds(random);
            GameObject clone = Instantiate(enemy, transform.position, Quaternion.identity);
            gm.enemiesInScene++;
            if (!right)
            {
                clone.transform.localScale = new Vector3(-1f, 1f);
            }
            gm.enemiesRound++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
            if (right)
            {
                transform.localPosition += new Vector3(.1f, 0f, 0f) * Time.deltaTime * 7f;
                if (transform.localPosition.x >= 0) StopCoroutine("spawnEnemies");
            }
            else
            {
                transform.localPosition += new Vector3(-.1f, 0f, 0f) * Time.deltaTime * 7f;
                if (transform.localPosition.x <= 0) StopCoroutine("spawnEnemies");
            }
            if (gm.enemiesRound >= gm.maxEnemies) StopCoroutine("spawnEnemies");
            
        }
    }
}
