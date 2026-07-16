using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerPos : MonoBehaviour
{
    public GameObject thing,player;

    private void Update()
    {
        thing.transform.position = player.transform.position;
    }
}
