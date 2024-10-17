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
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        idleTimer = 0;
        stayedTooLong = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(lastPosition == transform.position)
        {
            if (timer >= 1f)
            {
                idleTimer++;
                timer = 0;
            }
        } else
        {
            if(idleTimer != 0)
            {
                idleTimer = 0;
            }
        }

        if(idleTimer >= idleThreshold)
        {
            stayedTooLong = true;
            destination = transform.position;
        }
    }
}
