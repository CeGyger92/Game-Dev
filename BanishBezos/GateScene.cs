using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateScene : MonoBehaviour
{
    public GameObject player;
    public Text panel;
    public Text cont;
    string[] dialogue = { "Grunfeld (Dwarf):\n\n\nOh Yeah?\n\nYou might wanna double check the guest list for.... MY AXE!", "Minotaur:\n\n\nOkay, one second...\n\nHow do you spell the last name?", "Grunfeld (Dwarf):\n\n\nNo.. Uh... That was a...\n\nAh forget it, COMING THROUGH!!" };
    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        StartCoroutine("Dialogue");
        player.GetComponent<PlayerMovement>().frozen = true;
    }

    IEnumerator Dialogue()
    {
        yield return waitForKeyPress(KeyCode.Space);
        panel.text = dialogue[0];
        yield return waitForKeyPress(KeyCode.Space);
        panel.text = dialogue[1];
        yield return waitForKeyPress(KeyCode.Space);
        panel.text = dialogue[2];
        yield return waitForKeyPress(KeyCode.Space);
        panel.transform.parent.transform.gameObject.SetActive(false);
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<PlayerMovement>().frozen = false;
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
