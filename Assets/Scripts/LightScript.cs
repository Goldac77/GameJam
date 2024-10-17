using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    [HideInInspector] public bool attacking;

    [SerializeField] LayerMask rayLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("enemy"))
            {
                attacking = true;
            } else
            {
                attacking = false;
            }
        } else
        {
            attacking = false;
        }
    }
}
