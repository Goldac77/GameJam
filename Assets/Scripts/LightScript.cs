using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    [HideInInspector] public bool attacking;
    Light lightsource;

    [SerializeField] LayerMask rayLayerMask;

    float timer;
    [SerializeField] int lightTimer;
    [SerializeField] float offDuration;
    int counter;
    // Start is called before the first frame update
    void Start()
    {
        attacking = false;
        lightsource = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 1f)
        {
            counter++;
            timer = 0;
        }

        if(counter >= lightTimer)
        {
            lightsource.enabled = false;
            counter = 0;
            StartCoroutine(ToggleLight());
        }

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

    IEnumerator ToggleLight()
    {
        if (!lightsource.enabled)
        {
            lightsource.enabled = false;
            yield return new WaitForSeconds(offDuration);
        }
    }
}
