using UnityEngine;

public class sumoningSubs : MonoBehaviour
{
    private void OnEnable()
    {
        GameControllerScript.Instance.SubsManager.summonLeSingleSubtitle(sub.subtitleOption, sub, 0f, audisourc);
    }
    [SerializeField] private AudioSource audisourc;
    [SerializeField] private subsScriptableObject sub;
}
