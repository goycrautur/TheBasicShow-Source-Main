using UnityEngine;

public class AddToMinimapNpcIconAndOtherThingies : MonoBehaviour
{
    //boo this guy cant code a blacklist so he resulted to using whitelist instead booo booo
    [SerializeField] private GameObject sprRen,chaosClone;

    private void Start()
    {
        if (sprRen != null)
        {
            GameControllerScript.Instance.NpcMinimapIcon.Add(sprRen);
        }
        if (chaosClone != null)
        {
            GameControllerScript.Instance.npcCloneList.Add(chaosClone);
        }
    }
    private void OnDestroy()
    {
        if (sprRen != null)
        {
            GameControllerScript.Instance.NpcMinimapIcon.Remove(sprRen);
        }
        if (chaosClone != null)
        {
            GameControllerScript.Instance.npcCloneList.Remove(chaosClone);
        }
    }
}
