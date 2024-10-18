using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNear : MonoBehaviour
{
    [HideInInspector] public bool playerNear;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.name == "PlayerController")
        {
            playerNear = true;
            StartCoroutine(ReturnState());
        }
    }

    IEnumerator ReturnState()
    {
        playerNear = false;
        yield return new WaitForSeconds(3);
    }
}
