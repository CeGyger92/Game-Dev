using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class assetPurchase : MonoBehaviour
{
    public GameManager gm;
    public int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.gameObject.GetComponent<Button>();
        btn.onClick.AddListener(Purchase);
    }

    public int returnCount()
    {
        return count;
    }

    void Purchase()
    {
        Text [] textArray = this.gameObject.GetComponent<Button>().GetComponentsInChildren<Text>();
        //Debug.Log(textArray[1].text);
        int price = int.Parse(textArray[1].text.ToString());
        int rate = int.Parse(textArray[3].text.ToString());
        if(gm.currentCash >= price)
        {
            AudioSource audio = this.GetComponent<AudioSource>();
            audio.Play();
            gm.currentCash -= price;
            gm.IncreaseResidual(rate);
            count++;
            textArray[4].text = String.Concat("Count: ", count.ToString());
            if(price == 2100000000)
            {
                gm.gameOver();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
