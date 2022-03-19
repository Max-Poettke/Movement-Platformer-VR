using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class kiteScript : MonoBehaviour
{
    List<InputDevice> inputDevices = new List <InputDevice>();
    float distanceBetweenHands = 0;
    //trying to figure out a kite kind of flight system
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        bool triggerValue;
        if (XRNode.RightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue){
            Debug.Log("Trigger button is pressed.");
        }
        */
    }
}
