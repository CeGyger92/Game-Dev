using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deletePortal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void deleteMe(float n)
    {
        if(n ==1)Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
