using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;

public class InfiniteFruitAudio : MonoBehaviour
{
    public AudioSource movement;
    public AudioSource collide;
    private float velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        
        velocity = GetComponent<Rigidbody2D>().velocity.x;
        
        if(velocity == 0.0f)
        {
            movement.Stop();
            return;
        }
        else
        {
            movement.pitch = Math.Abs(velocity/6);
            if(!movement.isPlaying)
            {
                //Debug.Log("AudioSource is not playing");
                movement.Play();
            }
        }
        
        
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            collide.volume = Math.Abs(velocity / 90);
            collide.Play(); 
        }
    }
}
