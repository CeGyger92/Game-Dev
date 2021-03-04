using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class upgradePurchase : MonoBehaviour
{
    public GameObject assetPanel;
    public GameManager gm;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.gameObject.GetComponent<Button>();
        btn.onClick.AddListener(Purchase);
    }

    void Purchase()
    {
        Text[] textArray = this.gameObject.GetComponent<Button>().GetComponentsInChildren<Text>();
        //Debug.Log(textArray[1].text);
        int price = int.Parse(textArray[1].text.ToString());
        int rate = int.Parse(textArray[3].text.ToString());
        Button[] buttons = assetPanel.GetComponentsInChildren<Button>();
        if (gm.currentCash >= price)
        {
            AudioSource audio = this.GetComponent<AudioSource>();
            audio.Play();
            gm.currentCash -= price;
            if(rate == 1)
            {
                gm.cashRate *= 2;
            }
            else
            {
               
                //Debug.Log(buttons[rate - 2].GetComponentsInChildren<Text>()[3].text.ToString());
                int selectedRate = int.Parse(buttons[rate - 2].GetComponentsInChildren<Text>()[3].text.ToString());
                gm.IncreaseResidual(selectedRate);
                buttons[rate - 2].GetComponentsInChildren<Text>()[3].text = (selectedRate * 2).ToString();
            }
            count++;
            textArray[4].text = String.Concat("Count: ", count.ToString());
            if(count == 3 && rate != 1)
            {
                this.GetComponent<Button>().interactable = false;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}