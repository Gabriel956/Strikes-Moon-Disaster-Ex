using System;
using UnityEngine;

public class MeteorLogic : MonoBehaviour
{
    public float gravityMultiplier = 2f;
    public float maxFallSpeed = 50f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.down * 10f;
    }

    void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
        if (rb.linearVelocity.y < -maxFallSpeed)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -maxFallSpeed, rb.linearVelocity.z);
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        GameObject hitObject = collision.gameObject;

        if(hitObject.CompareTag("Player"))
        {
            Debug.Log("UFO hit!");
        }
        if (hitObject != null)
        {
            Destroy(gameObject);
            Debug.Log(hitObject.name);
            
        }
        //if(collision == null)
        //Destroy(gameObject);
    }
}
