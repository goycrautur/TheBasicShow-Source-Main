using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baloonPopParticleScript : MonoBehaviour
{
    public void Start() //LMFAO
    {
        GameControllerScript.Instance.SubsManager.summonLeSubtitle(GameControllerScript.Instance.subtitlesScriptableObject[7].subtitleOption, GameControllerScript.Instance.subtitlesScriptableObject[7], 0f, GetComponent<AudioSource>());
    }
}
