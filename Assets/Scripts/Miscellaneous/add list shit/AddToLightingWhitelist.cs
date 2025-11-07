using UnityEngine;

public class AddToLightingWhitelist : MonoBehaviour
{
    //boo this guy cant code a blacklist so he resulted to using whitelist instead booo booo
    private SpriteRenderer sprRen;

    private void Start()
    {
        sprRen = GetComponent<SpriteRenderer>();
        if (sprRen != null)
        {
            GameControllerScript.Instance.voxLight.addToLightFU(sprRen);
        }
    }
    private void OnEnable()
    {
        sprRen = GetComponent<SpriteRenderer>();
        if (sprRen != null)
        {
            GameControllerScript.Instance.voxLight.addToLightFU(sprRen);
        }
    }
    private void OnDisable()
    {
        sprRen = GetComponent<SpriteRenderer>();
        if (sprRen != null)
        {
            GameControllerScript.Instance.voxLight.addToLightFU(sprRen,false);
        }
    }
    private void OnDestroy()
    {
        sprRen = GetComponent<SpriteRenderer>();
        if (sprRen != null)
        {
            GameControllerScript.Instance.voxLight.addToLightFU(sprRen,false);
        }
    }
}
