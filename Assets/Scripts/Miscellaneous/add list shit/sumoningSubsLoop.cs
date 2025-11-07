using UnityEngine;

public class sumoningSubsLoop : MonoBehaviour
{
    private void OnEnable()
    {
        GameControllerScript.Instance.SubsManager.summonLeSingleSubtitle(sub.subtitleOption, sub, 0f, GetComponent<AudioSource>());
    }
    [SerializeField] private subsScriptableObject sub;
}
