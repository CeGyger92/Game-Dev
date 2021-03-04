using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blasterScript : MonoBehaviour
{
    public GameObject tapePrefab;
    public float delay = 2f;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        float random = Random.Range(0f, 5f);
        Quaternion rotation = tapePrefab.transform.rotation;
        Vector2 spawnPos;
        if(random <= 2)
        {
            spawnPos = transform.position + new Vector3(0f, .65f, 0f);
        }
        else
        {
            spawnPos = transform.position + new Vector3(0f, -.2f, 0f);
        }
        GetComponent<AudioSource>().Play();
        Instantiate(tapePrefab, spawnPos, rotation);

        Invoke("Spawn", delay);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
