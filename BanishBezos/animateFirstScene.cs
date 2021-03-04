using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class animateFirstScene : MonoBehaviour
{
    public GameObject Bezos;
    public GameObject wand;
    public GameObject CW;
    public GameObject portal;
    public Transform cellDoor;
    public Button OK;
    bool moveWiz = false;
    bool moveWand = false;
    float doorPos = 7.06f;
    bool moveBezos = false;

    // Start is called before the first frame update
    void Start()
    {
        OK.GetComponent<Button>().onClick.AddListener(gameInit);   
    }

    void gameInit()
    {
        OK.transform.parent.transform.gameObject.SetActive(false);
        StartCoroutine("Animate");
    }

    IEnumerator Animate()
    {
        yield return new WaitForSeconds(1f);
        moveWiz = true;
        yield return new WaitForSeconds(5f);
        wand.SetActive(true);
        GetComponents<AudioSource>()[0].Play();
        moveWand = true;
        doorPos = 0f;
        yield return new WaitForSeconds(3.5f);
        wand.transform.parent = Bezos.transform;
        portal.SetActive(true);
        GetComponents<AudioSource>()[1].Play();
        GetComponents<AudioSource>()[1].loop = true;
        yield return new WaitForSeconds(1f);
        moveBezos = true;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Arrival");
    }


    // Update is called once per frame
    void Update()
    {
        if(moveWiz && CW.transform.position.x > doorPos)
        {
            CW.transform.localPosition += new Vector3(-.1f, 0f, 0f) * Time.deltaTime * 20f;
        }
        if (moveWand && wand.transform.position.x > -2.4f)
        {
            wand.transform.localPosition += new Vector3(-.1f, 0f, 0f) * Time.deltaTime * 20f;
            if(wand.transform.position.y > -3.6)
            {
                wand.transform.localPosition += new Vector3(0f, -.1f, 0f) * Time.deltaTime * 20f;
            }

        }
        if (moveWand && wand.transform.position.x < -2.4f && wand.transform.position.y < .45f)
        {
            wand.transform.localPosition += new Vector3(0f, .25f, 0f) * Time.deltaTime * 40f;
            if (wand.transform.position.x > -3.8)
            {
                wand.transform.localPosition += new Vector3(-.1f, 0f, 0f) * Time.deltaTime * 40f;
            }

        }
        if (moveBezos)
        {
            if(Bezos.transform.position.x < portal.transform.position.x)
            {
                Bezos.transform.localPosition += new Vector3(.1f, 0f, 0f) * Time.deltaTime * 40f;
            }
            if (Bezos.transform.position.y < portal.transform.position.y)
            {
                Bezos.transform.localPosition += new Vector3(0f, .1f, 0f) * Time.deltaTime * 40f;
            }
            if (Bezos.transform.localScale.y >= .1f)
            {
                Bezos.transform.localScale += new Vector3(0f, -.1f, 0f) * Time.deltaTime * 10f;
            }
            if (Bezos.transform.localScale.x >= .1f)
            {
                Bezos.transform.localScale += new Vector3(-.1f, 0f, 0f) * Time.deltaTime * 10f;
            }
            if(Bezos.transform.localScale.x < .1f || Bezos.transform.localScale.y < .1f)
            {
                portal.GetComponent<Animator>().SetTrigger("Dissmiss");
                GetComponents<AudioSource>()[1].Stop();
                Destroy(Bezos.gameObject);
                moveBezos = false;
                moveWand = false;
                moveWiz = false;
            }
        }
    }
}
// -2.4 -3.6