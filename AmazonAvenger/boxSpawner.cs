using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxSpawner : MonoBehaviour
{
    public GameObject primeBox;
    public float minSpawn = 1f;
    public float maxSpawn = 3f;
    
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        Instantiate(primeBox, transform.position, Quaternion.identity);
        Invoke("Spawn", Random.Range(minSpawn, maxSpawn));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
