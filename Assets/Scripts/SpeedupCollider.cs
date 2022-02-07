using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedupCollider : MonoBehaviour
{
    private Rigidbody rb;
    private bool speedingUp = false;

    public void Start(){
        rb = GetComponent<Rigidbody>();
    }

    public void Update(){
        if(speedingUp){
            rb.velocity = new Vector3(rb.velocity.normalized.x + rb.velocity.x, rb.velocity.y, rb.velocity.normalized.z + rb.velocity.z);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "speedCollider"){
            speedingUp = true;
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "speedCollider"){
            speedingUp = false;
        }
    }
}
