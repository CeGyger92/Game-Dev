using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Arrival : MonoBehaviour
{
    public GameObject prisoner;
    public GameObject wiz;
    public GameObject portal;
    public Text panel;
    public Image transition;
    public Text transitionText;
    public Text cont;
    string[] dialogue = { "Bezos:\n\nWhy does this world have such a low resolution?", "Bezos:\n\nWait, whats happening to me!?!?!?!", "Bezos:\n\nHmm...\n\nWell its better than the inside of a cell, time to see what this world has to offer..." };
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("AnimatePrisoner");
    }

    IEnumerator AnimatePrisoner()
    {
        yield return waitForKeyPress(KeyCode.Space);
        panel.text = dialogue[0];
        yield return waitForKeyPress(KeyCode.Space);
        panel.text = dialogue[1];
        portal.GetComponent<Animator>().SetTrigger("dissmiss");
        cont.gameObject.SetActive(false);
        GetComponents<AudioSource>()[1].Play();
        for (int i = 0; i < 10; i++)
        {
            prisoner.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 1f, .5f);
            while (prisoner.GetComponent<SpriteRenderer>().color.r < 1f)
            {
                prisoner.GetComponent<SpriteRenderer>().color += new Color(.1f, 0f, -.1f);
                yield return new WaitForSeconds(.01f);
            }
            while (prisoner.GetComponent<SpriteRenderer>().color.r > 0f)
            {
                prisoner.GetComponent<SpriteRenderer>().color += new Color(-.1f, 0f, .1f);
                yield return new WaitForSeconds(.01f);
            }
        }
        prisoner.SetActive(false);
        GetComponents<AudioSource>()[1].Stop();
        wiz.SetActive(true);
        //yield return new WaitForSeconds(.5f);
        panel.text = dialogue[2];
        cont.gameObject.SetActive(true);
        yield return waitForKeyPress(KeyCode.Space);
        while(transition.color.a < 1)
        {
            transition.color += new Color(0f, 0f, 0f, .1f);
            yield return new WaitForSeconds(.1f);
            Debug.Log(transition.color.a);
        }
        transitionText.transform.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("Dragon");
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
        
    }
}
