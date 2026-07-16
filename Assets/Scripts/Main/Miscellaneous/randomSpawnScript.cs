using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomSpawnScript : MonoBehaviour
{
    private AILocationSelectorScript wanderer;
    private Transform location;
    public void Start()
    {
        wanderer = FindObjectOfType<AILocationSelectorScript>();

        GameObject Set = GameObject.Find("AI_LocationSelector");
        location = Set.transform;

        location.position = wanderer.SetNewTargetForAgent(null, "present");
        transform.position = location.position + Vector3.up * 4f;
    }
}
