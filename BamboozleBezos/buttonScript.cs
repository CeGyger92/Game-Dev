using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonScript : MonoBehaviour
{
    public Button OK;
    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        OK.GetComponent<Button>().onClick.AddListener(Toggle);
    }

    void Toggle()
    {
        panel.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
