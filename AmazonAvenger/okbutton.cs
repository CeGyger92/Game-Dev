using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class okbutton : MonoBehaviour
{
    public Button btn;
    // Start is called before the first frame update
    void Start()
    {
        Button okButton = this.GetComponent<Button>();
        btn.onClick.AddListener(Dismiss);
    }

    void Dismiss()
    {
        btn.transform.parent.transform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
