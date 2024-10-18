using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [HideInInspector] public bool stayedTooLong;
    int idleTimer;
    float timer;
    [SerializeField] int idleThreshold;

    Vector3 lastPosition;
    [HideInInspector] public Vector3 destination;

    [SerializeField] PlayerNear playerNear;
    AudioSource audioSource;

    [HideInInspector] public bool playerWon;
    int aliveDuration;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        idleTimer = 0;
        stayedTooLong = false;
        lastPosition = transform.position;
        playerWon = false;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNear.playerNear)
        {
            audioSource.Play();
        }

        timer += Time.deltaTime;
        if(lastPosition == transform.position)
        {
            if (timer >= 1f)
            {
                idleTimer++;
                aliveDuration++;
                timer = 0;
            }
        } else
        {
            if(idleTimer != 0)
            {
                idleTimer = 0;
            }
            lastPosition = transform.position;
            stayedTooLong = false;
        }

        if(idleTimer >= idleThreshold)
        {
            stayedTooLong = true;
            destination = transform.position;
        }

        if(aliveDuration >= 300)
        {
            playerWon = true;
        }
    }
}
