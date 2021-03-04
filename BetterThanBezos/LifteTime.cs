using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LifteTime : MonoBehaviour

{
    public Text LifeTime;
    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LifeTime.text = gm.lifetimeCash.ToString();
    }
}
