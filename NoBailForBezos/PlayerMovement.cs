using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public Transform bulletSpawn;
    public GameObject bullet;
    public float thrust = 25f;
    public bool facing = true;
    public GameManager gm;

    float hMove = 0f;
    public float runSpeed = 40f;
    bool isFlying = false;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    IEnumerator Thrust()
    {
        yield return new WaitForSeconds(.5f);
        if (!GetComponents<AudioSource>()[1].isPlaying)
        {
            GetComponents<AudioSource>()[1].Play();
        }
    }
    // Update is called once per frame
    void Update()
    {
        hMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(hMove));

        if(Input.GetAxisRaw("Horizontal") < 0)
        {
            facing = false;
        }else if(Input.GetAxisRaw("Horizontal") > 0)
        {
            facing = true;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (gm.ammoCount > 0)
            {
                animator.SetTrigger("toShoot");
                GetComponents<AudioSource>()[0].Play();
                Instantiate(bullet, bulletSpawn.position, Quaternion.identity);
                gm.AmmoUpdate(-1);
            }
        }

        if (Input.GetKey("w"))
        {
            isFlying = true;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, thrust));
            animator.SetBool("isFlying", true);
            if (!GetComponents<AudioSource>()[1].isPlaying)
            {
                StartCoroutine("Thrust");
            }
            
        }
        else
        {
            isFlying = false;
            animator.SetBool("isFlying", false);
            StopCoroutine("Thrust");
            GetComponents<AudioSource>()[1].Stop();
        }

    }

    

    private void FixedUpdate()
    {
        controller.Move(hMove * Time.fixedDeltaTime, false, false);
        
    }
}
