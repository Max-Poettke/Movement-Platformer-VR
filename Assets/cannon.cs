using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannon : MonoBehaviour
{
    public Transform target;
    public Rigidbody rb;
    public float speed;

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "MainCamera"){
            rb = other.gameObject.transform.parent.transform.parent.GetComponent<Rigidbody>();
            Vector3 direction = new Vector3(target.position.x - transform.position.x, target.position.y - transform.position.y, target.position.z - transform.position.z).normalized;
            rb.velocity = direction * speed;
        }
    }
}
