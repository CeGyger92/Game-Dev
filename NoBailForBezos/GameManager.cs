using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public int ammoCount = 4;
    public GameObject ammo;
    Transform[] slots;
    public GameObject PoliceStation;
    public float fireRate = 2f;
    public GameObject enemyBullet;
    public GameObject healthBar;
    public Image playerHealth;
    Image[] hearts;
    int timesHit = 0;
    public float bulletDamage = .02f;
    public float minSpawn = .5f;
    public float maxSpawn = 2f;
    public int maxEnemies = 3;
    public int enemiesRound = 0;
    float roundTime = 40f;
    int round = 1;
    public Text TimeClock;
    public Text GameStage;
    public GameObject chopper1;
    public GameObject chopper2;
    float currentRound;
    public int enemiesInScene = 0;
    public GameObject MessagePanel;
    Button[] btns;
    Text bodyText;
    public GameObject cTruck;

    // Start is called before the first frame update
    void Start()
    {
        slots = ammo.GetComponentsInChildren<Transform>();
        Physics2D.IgnoreLayerCollision(10, 8);
        hearts = playerHealth.GetComponentsInChildren<Image>();
        btns = MessagePanel.GetComponentsInChildren<Button>();
        btns[0].onClick.AddListener(okButton);
        btns[1].onClick.AddListener(rsButton);
        btns[1].gameObject.SetActive(false);
        bodyText = MessagePanel.transform.GetChild(1).GetComponent<Text>();
        Time.timeScale = 0;
        StartCoroutine("roundTracker");
        //StartCoroutine("Win");
    }

    void okButton()
    {
        MessagePanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    void rsButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator roundTracker()
    {
        currentRound = roundTime;
        while (currentRound > 0)
        {
            yield return new WaitForSeconds(1f);
            currentRound--;
            if(currentRound <= 3)
            {
                GetComponents<AudioSource>()[0].Play();
            }
            TimeClock.text = currentRound.ToString();
            if (currentRound == 0)
            {
                if (round < 4)
                {
                    round++;
                    nextRound();
                }
                else
                {
                    GameOver(2);
                }
            }
        }
    }

    public void nextRound()
    {
        StopCoroutine("roundTracker");
        GameStage.text = ("Wave " + round.ToString() + "/4");
        roundTime += 20f;
        TimeClock.text =roundTime.ToString();
        maxEnemies += 2;
        enemiesRound = 0;
        fireRate *= .90f;
        if (timesHit > 0)
        {
            GetComponents<AudioSource>()[3].Play();
            timesHit--;
            hearts[timesHit + 1].color = new Color(1f, 1f, 1f, 1f);
        }
        chopper1.GetComponent<chopperScript>().moving = true;
        chopper2.GetComponent<chopperScript>().moving = true;
        StartCoroutine("roundTracker");
    }

    public void AmmoUpdate(int n)
    {
        if(n > 0)GetComponents<AudioSource>()[1].Play();
        ammoCount += n;
        for(int i = 0; i < 4; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < ammoCount; i++)
        {
            slots[i].gameObject.SetActive(true);
        }
    }

    public void lowerHealth()
    {
        healthBar.transform.localScale -= new Vector3(bulletDamage, 0f);
        
        if(healthBar.transform.localScale.x <= .01f)
        {
            GameOver(1);
        }
        
    }

    public void hitPlayer()
    {
        GetComponents<AudioSource>()[2].Play();
        if (timesHit < 3)
        {
            hearts[timesHit+1].color = new Color(1f, 1f, 1f, .1f);
            timesHit++;
        }
        
        if(timesHit+1 == 4)
        {
            GameOver(0);
        }
    }

    public void GameOver(int endCode)
    {
        if(endCode == 0)
        {
            Time.timeScale = 0;
            bodyText.fontSize = 20;
            bodyText.text = "The securoBot has taken one too many hits!\n\nBezos' Thugs will be free to break him out without resistance!\n\nRIP securoBot";
            MessagePanel.SetActive(true);
            btns[0].gameObject.SetActive(false);
            btns[1].gameObject.SetActive(true);
        }else if(endCode == 1)
        {
            Time.timeScale = 0;
            bodyText.fontSize = 20;
            bodyText.text = "The assault on the Police Station was too much!\n\nThe cops had to hand Bezos over to his allies and now he's escaped!!";
            MessagePanel.SetActive(true);
            btns[0].gameObject.SetActive(false);
            btns[1].gameObject.SetActive(true);
        }
        else
        {
            StartCoroutine("Win");
        }
    }

    IEnumerator Win()
    {
        while(cTruck.transform.position.x > 0)
        {
            yield return new WaitForEndOfFrame();
            cTruck.transform.localPosition += new Vector3(-.1f, 0f, 0f) * Time.deltaTime * 20f;
        }
        cTruck.transform.GetChild(0).GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 5.0f);
        yield return new WaitForSeconds(.5f);
        Destroy(cTruck.transform.GetChild(0).gameObject);
        cTruck.transform.GetChild(1).transform.gameObject.SetActive(true);
        cTruck.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0;
        bodyText.fontSize = 20;
        bodyText.text = "That should keep the Police Station safe and ensure that Bezos remains  behind bars.\n\nCyberTruck for the win!";
        MessagePanel.SetActive(true);
        btns[0].gameObject.SetActive(false);
        btns[1].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log((!chopper1.GetComponent<chopperScript>().moving) + " " + currentRound);
        if ((!chopper1.GetComponent<chopperScript>().moving) && (!chopper2.GetComponent<chopperScript>().moving) && currentRound >= 30f && maxEnemies > enemiesRound)
        {
            chopper1.GetComponent<chopperScript>().moving = true;
            chopper2.GetComponent<chopperScript>().moving = true;
        }
        if(enemiesInScene == 0 && currentRound < 30f && currentRound > 10)
        {
            currentRound = 10f;
        }
    }
}
