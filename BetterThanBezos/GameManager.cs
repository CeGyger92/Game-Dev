using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text cash;
    public Text rDisplay;
    public int cashRate = 1;
    public int residual = 0;
    public GameObject Coin;
    public Text GOText;
    public GameObject GOPanel;
    public Button GOButton;

    public int currentCash = 0;
    public int lifetimeCash = 0;

    public void addScore()
    {

        currentCash += 1 * cashRate;
        lifetimeCash += 1 * cashRate;
        cash.text = String.Concat("$", currentCash.ToString(), " USD");
        
    }

    public void IncreaseResidual(int x)
    {
        residual += x;
        rDisplay.text = String.Concat("Residual: $", residual.ToString(), " /s");
    }

    public void Spawn()
    {
        for (int i = 0; i < 2; i++)
        {
            Vector3 spawnPos = new Vector3(0f, 0f, 0f);
            Instantiate(Coin, spawnPos, Quaternion.identity);
        }
    }

    public void gameOver()
    {
        Time.timeScale = 0;
        GameObject.Find("assetPanel").SetActive(false);
        GOPanel.SetActive(true);
        GOText.enabled = true;
        GOButton.interactable = true;
        AudioSource audio = this.GetComponent<AudioSource>();
        audio.Play();
    }


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateScore", 1f, 1f);
    }

    void UpdateScore()
    {
        

        if(currentCash <= 2100000000)
        {
            currentCash += residual;
            lifetimeCash += residual;
            cash.text = String.Concat("$", currentCash.ToString(), " USD");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
