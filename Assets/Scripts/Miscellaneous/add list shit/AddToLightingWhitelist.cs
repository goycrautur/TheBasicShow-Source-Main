using UnityEngine;

public class AddToLightingWhitelist : MonoBehaviour
{
    //boo this guy cant code a blacklist so he resulted to using whitelist instead booo booo
    [SerializeField] private SpriteRenderer sprRen;

    private void Start()
    {
        return; 
        sprRen = GetComponent<SpriteRenderer>();
        if (sprRen != null)
        {
            GameControllerScript.Instance.voxLight.addToLightFU(sprRen);
        }
    }
    private void OnEnable()
    {
        return; 
        sprRen = GetComponent<SpriteRenderer>();
        if (sprRen != null)
        {
            GameControllerScript.Instance.voxLight.addToLightFU(sprRen);
        }
    }
    private void OnDisable()
    {
        return; 
        sprRen = GetComponent<SpriteRenderer>();
        if (sprRen != null)
        {
            GameControllerScript.Instance.voxLight.addToLightFU(sprRen,false);
        }
    }
    private void OnDestroy()
    {
        return; 
        sprRen = GetComponent<SpriteRenderer>();
        if (sprRen != null)
        {
            GameControllerScript.Instance.voxLight.addToLightFU(sprRen,false);
        }
    }
}
