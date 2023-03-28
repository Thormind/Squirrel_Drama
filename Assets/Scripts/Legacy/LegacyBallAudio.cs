using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyBallAudio : MonoBehaviour
{
    public AudioSource movement;
    public AudioSource collide;
    private float velocity;


    // Start is called before the first frame update
    void Awake()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.gameListener = gameObject.GetComponent<AudioListener>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        velocity = GetComponent<Rigidbody2D>().velocity.x;
        velocity = Mathf.Abs(velocity);

        if (velocity < 0.01f)
        {
            movement.Stop();
            return;
        }
        else
        {
            movement.pitch = velocity/2;
            if (!movement.isPlaying && ScenesManager.gameState == GAME_STATE.ACTIVE)
            {
                movement.Play();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Border")
        { 
            collide.volume = velocity / 30;
            collide.Play();
        }
    }
}
