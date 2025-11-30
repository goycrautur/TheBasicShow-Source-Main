using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class sumoningSubs : MonoBehaviour
{
    private void OnEnable()
    {
        if (!DelaySpawning)
        {
            if (type == "3d")
            {
            GameControllerScript.Instance.SubsManager.summonLeSingleSubtitle(sub.subtitleOption, sub, 0f, audisourc);
            }
            if (type == "2d")
            {
            GameControllerScript.Instance.SubsManager.summonLeSubtitle2D(sub.subtitleOption,sub,0f,new Vector3(0f,-170.5f,0f),audisourc);
            }
        }
        if (DelaySpawning)
        {
            StartCoroutine(SpawnDelayerkys());
        }
    }
    private IEnumerator SpawnDelayerkys()
    {
        yield return new WaitForSeconds(spawnDelay);
        if (type == "3d")
        {
        GameControllerScript.Instance.SubsManager.summonLeSingleSubtitle(sub.subtitleOption, sub, 0f, audisourc);
        }
        if (type == "2d")
        {
        GameControllerScript.Instance.SubsManager.summonLeSubtitle2D(sub.subtitleOption,sub,0f,new Vector3(0f,-170.5f,0f),audisourc);
        }
        yield break;
    }
    [SerializeField] private AudioSource audisourc;
    [SerializeField] private subsScriptableObject sub;
    [SerializeField] private string type = "3d";
    [SerializeField] private bool DelaySpawning;
    [SerializeField] private float spawnDelay;
}
