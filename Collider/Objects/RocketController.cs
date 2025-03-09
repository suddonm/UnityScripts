using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class RocketController : MonoBehaviour
{
    public float Delay = 3f;

    public float Thrust = 5f;

    float countdown = 0f;
    bool hasExploded = false;

    public GameObject ExplosionEffect;
    public GameObject RocketTrailEffect;

    public GameObject RocketNozzle;

    // Start is called before the first frame update
    void Start()
    {
        countdown = Delay;

        Instantiate(RocketTrailEffect, RocketNozzle.transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        //if (!hasExploded)
        //{
        //    RocketTrailEffect.transform.position = this.transform.position;
        //}


        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    private void FixedUpdate()
    {
        if (!hasExploded)
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * Thrust);
        }
    }

    void Explode()
    {
        Instantiate(ExplosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
