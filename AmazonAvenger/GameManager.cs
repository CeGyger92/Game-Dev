using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int keycards = 0;
    public Text keycardText;
    public GameObject battery;
    public int battStage = 4;
    Image[] bars;
    public Button OK;
    public Button RS;
    public GameObject cage;
    public GameObject Player;
    public GameObject cameraP;
    bool win = false;
    GameObject UIPanel;
    public GameObject bezos;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        UIPanel = OK.transform.parent.gameObject;
        Button okButton = OK.GetComponent<Button>();
        okButton.onClick.AddListener(clearButton);
        RS.GetComponent<Button>().onClick.AddListener(Restart);
        bars = battery.GetComponentsInChildren<Image>();
        StartCoroutine("checkBatt");
    }
    
    IEnumerator checkBatt()
    {
        yield return new WaitForSeconds(7f);
        if (battStage >= 0)
        {
            battStage--;
        }
        updateBattery();
        StartCoroutine("checkBatt");
    }

    public void updateBattery()
    {
        int battColor = 0;
        if (battStage < 4)
        {
            bars[4].transform.gameObject.SetActive(false);
        }
        else
        {
            bars[4].transform.gameObject.SetActive(true);
        }
        if (battStage < 3)
        {
            bars[3].transform.gameObject.SetActive(false);
            battColor = 1;
        }
        else
        {
            bars[3].transform.gameObject.SetActive(true);
        }
        if (battStage < 2)
        {
            bars[2].transform.gameObject.SetActive(false);
            battColor = 2;
        }
        else
        {
            bars[2].transform.gameObject.SetActive(true);
        }
        if (battStage < 1)
        {
            bars[1].transform.gameObject.SetActive(false);
            StartCoroutine("animateBar");
        }
        else
        {
            bars[1].transform.gameObject.SetActive(true);
        }
        if (battStage < 0)
        {
            GameOver(1);
        }

        for(int i = 0; i <5; i++)
        {
            if(battColor == 0)
            {
                bars[i].color = new Color(0.4791972f, 0.8679245f, 0.4053043f, .8f);
            }else if(battColor == 1)
            {
                bars[i].color = new Color(0.9433962f, 0.6917431f, 0.2358491f, .8f);
            }
            else
            {
                bars[i].color = new Color(1f, 0f, 0f, .8f);
            }
        }
    }

    IEnumerator animateBar()
    {
        for(int i = 0; i < 10 && battStage < 1 && !win; i++)
        {
            yield return new WaitForSeconds(.5f);
            if (bars[0].transform.gameObject.activeSelf)
            {
                bars[0].transform.gameObject.SetActive(false);
            }
            else
            {
                bars[0].transform.gameObject.SetActive(true);
                battery.transform.parent.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        bars[0].gameObject.SetActive(true);
        battery.transform.parent.gameObject.GetComponent<AudioSource>().Play();
    }

    public void updateKeycards()
    {
        keycardText.text = keycards + "/5";
    }

    public void GameOver(int endcode)
    {   
        if (endcode == 0)
        {
            Time.timeScale = 0;
            UIPanel.GetComponentsInChildren<Text>()[1].text = ("Some unexpecting Prime Member just got a securoBot with their shipment and you failed to stop Bezos!!!");
            OK.gameObject.SetActive(false);
            RS.gameObject.SetActive(true);
            UIPanel.SetActive(true);
        }
        if(endcode == 1 && !UIPanel.activeSelf && !win)
        {
            Time.timeScale = 0;
            UIPanel.GetComponentsInChildren<Text>()[1].text = ("SecuroBot's battery died and Bezos got away!! \nYou have to keep the securoBot charged! Hit the green powerstations with the bot to charge up!");
            OK.gameObject.SetActive(false);
            RS.gameObject.SetActive(true);
            UIPanel.SetActive(true);
        }
        if (endcode == 2)
        {
            Player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            win = true;
            StartCoroutine("Win");
        }
        if (endcode == 3)
        {
            Time.timeScale = 0;
            UIPanel.GetComponentsInChildren<Text>()[1].color = new Color(1f, 0f, 0f);
            UIPanel.GetComponentsInChildren<Text>()[1].text = ("ACCESS DENIED\n\n You need all 5 keycards to enter");
            OK.gameObject.SetActive(true);
            RS.gameObject.SetActive(false);
            UIPanel.SetActive(true);
        }
    }

    public void clearButton()
    {
        OK.transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1;
        OK.transform.parent.gameObject.GetComponentsInChildren<Text>()[1].color = new Color(1f, 1f, 1f);
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public IEnumerator Win()
    {
        while (cameraP.transform.position.x < 258)
        {
            cameraP.GetComponent<CameraFollow>().offset += new Vector3(.05f, 0f, 0f);
            yield return new WaitForSeconds(.01f);
        }
        bezos.GetComponents<AudioSource>()[0].Play();
        yield return new WaitForSeconds(1.5f);
        cage.SetActive(true);
        yield return new WaitForSeconds(2f);
        bezos.GetComponents<AudioSource>()[1].Play();
        Time.timeScale = 0;
        UIPanel.GetComponentsInChildren<Text>()[1].color = new Color(1f, 1f, 1f);
        UIPanel.GetComponentsInChildren<Text>()[1].text = ("YOU DID IT!\n\n\n You stopped Bezos from taking away 2-day shipping!\nCops are on their way to take care of this trespasser!");
        UIPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            battStage = 4;
            updateBattery();
        }
    }
}
