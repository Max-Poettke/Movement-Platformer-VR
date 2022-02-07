using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCollider : MonoBehaviour
{
   private Rigidbody rb;

    public void Start(){
        rb = GetComponent<Rigidbody>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "gravityCollider"){
            rb.useGravity = false;    
        }
    }
    public void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "gravityCollider"){
            rb.useGravity = true;    
        }
    }
}
