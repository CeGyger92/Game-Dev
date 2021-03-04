using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonScene : MonoBehaviour
{
    public bool triggered = false;
    public Camera cam;
    public GameObject player;
    public GameObject wizard;
    public GameObject portal;
    public Text panel;
    public Text cont;
    GameObject mem;
    bool moveBezos;
    string[] dialogue = { "Grunfeld:\n\n\nMr.Wizard I'm gonna have to ask you to leave and go back where you came from, you're not welcome here!", "Bezos:\n\n\nBAH!\n\nWelcome or not this is now my domain, prepare to meet your end, dwarf!!"
    , "Bezos:\n\n\nNo, wait, this can't be happening!!" , "Grunfeld:\n\n\nTime to go bye bye Mr.Wizard\n\n*Grunfeld reads the words from the magical scroll*" , "Bezos:\n\n\nNOOOOOOOOO!!"};
    // Start is called before the first frame update
    void Start()
    {
        mem = GameObject.Find("MemoryCard"); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;
        triggered = true;
        cam.gameObject.GetComponent<CameraScript>().panZoomOut();
        GetComponents<AudioSource>()[2].Stop();
        GetComponents<AudioSource>()[3].Play();
        StartCoroutine("Dialogue");
    }

    IEnumerator Dialogue()
    {
        yield return new WaitForSeconds(2f);
        panel.transform.parent.transform.gameObject.SetActive(true);
        yield return waitForKeyPress(KeyCode.Space);
        panel.text = dialogue[0];
        yield return waitForKeyPress(KeyCode.Space);
        panel.text = dialogue[1];
        yield return waitForKeyPress(KeyCode.Space);
        panel.transform.parent.transform.gameObject.SetActive(false);
        cam.gameObject.GetComponent<CameraScript>().panZoomIn();
        wizard.GetComponent<Bezos>().target = player.transform;
        wizard.GetComponent<Bezos>().StartCoroutine("SeekEnemy");
    }

    IEnumerator Dialogue2()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<PlayerMovement>().frozen = true;
        yield return new WaitForSeconds(2f);
        panel.transform.parent.transform.gameObject.SetActive(true);
        panel.text = dialogue[2];
        yield return waitForKeyPress(KeyCode.Space);
        panel.text = dialogue[3];
        GetComponents<AudioSource>()[0].Play();
        for (int i = 0; i < 10; i++)
        {
            wizard.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 1f, .5f);
            while (wizard.GetComponent<SpriteRenderer>().color.r < 1f)
            {
                wizard.GetComponent<SpriteRenderer>().color += new Color(.1f, 0f, -.1f);
                yield return new WaitForSeconds(.01f);
            }
            while (wizard.GetComponent<SpriteRenderer>().color.r > 0f)
            {
                wizard.GetComponent<SpriteRenderer>().color += new Color(-.1f, 0f, .1f);
                yield return new WaitForSeconds(.01f);
            }
        }
        wizard.GetComponent<SpriteRenderer>().color = Color.white;
        GetComponents<AudioSource>()[0].Stop();
        wizard.transform.localScale = new Vector3(.6f, .6f, 1f);
        wizard.GetComponent<Animator>().SetTrigger("Transform");
        wizard.GetComponent<Bezos>().target = portal.transform;
        
        yield return waitForKeyPress(KeyCode.Space);
        portal.SetActive(true);
        GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(1f);
        wizard.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation; 
        moveBezos = true;
        panel.text = dialogue[4];
        yield return waitForKeyPress(KeyCode.Space);
        player.GetComponent<PlayerMovement>().GameOver(0);
        panel.transform.parent.transform.gameObject.SetActive(false);
    }


    private IEnumerator waitForKeyPress(KeyCode key)
    {
        bool done = false;
        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            if (Input.GetKeyDown(key))
            {
                done = true; // breaks the loop
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }

        // now this function returns
    }
    // Update is called once per frame
    void Update()
    {
        if (moveBezos)
        {
            Debug.Log("trying");
            if (wizard.transform.position.x > portal.transform.position.x)
            {
                wizard.transform.localPosition += new Vector3(-.01f, 0f, 0f) * Time.deltaTime* 125f;
            }
            if (wizard.transform.position.y > portal.transform.position.y)
            {
                wizard.transform.localPosition += new Vector3(0f, .01f, 0f) * Time.deltaTime * 50f;
            }
            if (wizard.transform.localScale.y >= .1f)
            {
                wizard.transform.localScale += new Vector3(0f, -.01f, 0f) * Time.deltaTime * 30f;
            }
            if (wizard.transform.localScale.x >= .1f)
            {
                wizard.transform.localScale += new Vector3(-.01f, 0f, 0f) * Time.deltaTime * 30f;
            }
            if (wizard.transform.localPosition.x < 4.55f)
            {
                portal.GetComponent<Animator>().SetTrigger("dissmiss");
                GetComponents<AudioSource>()[1].Stop();
                Destroy(wizard.gameObject);
                moveBezos = false;
            }
        }
    }
}
