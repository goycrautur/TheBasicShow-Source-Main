using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateObjectConstantlyWhyIsThisAThingBroKms : MonoBehaviour
{
    public void Update()
    {
        transform.rotation = transform.rotation * rotationThing;
    }
    public Quaternion rotationThing;
}
