using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    [SerializeField] Animator dooranimator_1;
    [SerializeField] Animator dooranimator_2;
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
            dooranimator_1.SetBool("begin", true);
            dooranimator_2.SetBool("begin", true);
        }
    }
}
