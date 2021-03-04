using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearScript : MonoBehaviour
{
    public GameObject red;
    public GameObject blue;
    public GameObject green;
    public GameObject doors;

    public bool gemsPlaced = false;
    public bool doorsClosed = true;
    AudioSource [] clips;
    // Start is called before the first frame update
    void Start()
    {
        clips = GetComponents<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if(doorsClosed)clips[0].Play();
            if (collision.transform.GetComponent<PlayerMovement>().redGem)
            {
                placeGem(red);
                if(!gemsPlaced)clips[1].Play();
            }
            if (collision.transform.GetComponent<PlayerMovement>().greenGem)
            {
                placeGem(green);
                if (!gemsPlaced) clips[1].Play();
            }
            if (collision.transform.GetComponent<PlayerMovement>().blueGem)
            {
                placeGem(blue);
                if (!gemsPlaced) clips[1].Play();
            }

            if (collision.transform.GetComponent<PlayerMovement>().redGem && collision.transform.GetComponent<PlayerMovement>().greenGem && collision.transform.GetComponent<PlayerMovement>().blueGem)
            {
                gemsPlaced = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        clips[0].Stop();
    }

    void placeGem(GameObject gem)
    {
        float addition = 1 - gem.GetComponent<SpriteRenderer>().color.a;
        gem.GetComponent<SpriteRenderer>().color += new Color(0f, 0f, 0f, addition);
    }

    void openDoor()
    {
        doors.GetComponent<Animator>().SetTrigger("Open");
        clips[2].Play();
        doors.GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.rotation.eulerAngles.z % 360 < 204 && transform.rotation.eulerAngles.z % 360 > 202 && gemsPlaced && doorsClosed)
        {
            Debug.Log("Here");
            doorsClosed = false;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            openDoor();
        }
    }
}
