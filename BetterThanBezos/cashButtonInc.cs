using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cashButtonInc : MonoBehaviour
{
    public GameManager gm;
    
    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.gameObject.GetComponent<Button>();
        btn.onClick.AddListener(Increase);


    }

    void Increase()
    {
        gm.addScore();
        gm.Spawn();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
