using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public GameObject firePrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CreateFlames");
        Physics2D.IgnoreLayerCollision(0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerMovement>().TakeDamage(15);
        }
        Destroy(this.gameObject);
    }

    IEnumerator CreateFlames()
    {
        while (true) {
            yield return new WaitForSeconds(1f);
            float random = Random.Range(0f, 1f);
            if (random <= .25)
            {
                Instantiate(firePrefab, transform.position, Quaternion.identity);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
