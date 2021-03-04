using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DragonScene : MonoBehaviour
{
    int dLog = 0;
    public Text panel;
    public Text cont;
    public GameObject dragon;
    public GameObject Vbub;
    public GameObject Cbub;
    public GameObject Adven;
    public GameObject AdvenF;
    public GameObject Glad;
    bool animating = false;
    string[] dialogue = {"Fang: \n\nBut first, before you embark on this quest...\n\nA story...." , "Carric:\n\nUgh, Is this gonna be a long story???", "Fang:\n\n FOOL!!", "Fang:\n\n Amongst my treasure hoard " +
            "is an item that will bring your allies back to life.\n\nTake this magic scroll, after you've weakened the wizard called 'Bezos' read it's words to banish him back to his home plane. Only then will I allow you to raise them up in my name to continue in my service.\n\n\n GO.. NOW!" };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator BreathAttack()
    {
        yield return new WaitForSeconds(4f);
        
        Cbub.gameObject.SetActive(false);
        Vbub.gameObject.SetActive(false);
        dragon.GetComponent<Animator>().SetTrigger("Breath");
        yield return new WaitForSeconds(1f);
        Adven.GetComponent<Animator>().SetTrigger("Death");
        AdvenF.GetComponent<Animator>().SetTrigger("Death");
        Glad.GetComponent<Animator>().SetTrigger("Death");
        animating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && !animating)
        {
            if (dLog > 3) SceneManager.LoadScene("GateKeeping");
            if(dLog < 4) panel.text = dialogue[dLog];
            if (dLog == 1)
            {
                Cbub.gameObject.SetActive(true);
                Vbub.gameObject.SetActive(false);
            }else if(dLog == 2)
            {
                Cbub.gameObject.SetActive(false);
                Vbub.gameObject.SetActive(true);
                dragon.GetComponent<Animator>().SetTrigger("Anger");
                GetComponents<AudioSource>()[1].Play();
                animating = true;
                StartCoroutine("BreathAttack");
            }
            else
            {
                Cbub.gameObject.SetActive(false);
                Vbub.gameObject.SetActive(true);
            }
            dLog++;
        }
    }
}
