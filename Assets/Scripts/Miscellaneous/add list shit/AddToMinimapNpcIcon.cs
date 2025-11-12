using UnityEngine;

public class AddToMinimapNpcIcon : MonoBehaviour
{
    //boo this guy cant code a blacklist so he resulted to using whitelist instead booo booo
    [SerializeField] private GameObject sprRen;

    private void Start()
    {
        if (sprRen != null)
        {
            GameControllerScript.Instance.NpcMinimapIcon.Add(sprRen);
        }
    }
    private void OnDestroy()
    {
        if (sprRen != null)
        {
            GameControllerScript.Instance.NpcMinimapIcon.Remove(sprRen);
        }
    }
}
