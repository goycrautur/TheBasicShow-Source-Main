using UnityEngine;

public class BlockageRandomizer : MonoBehaviour
{
    private GameObject lastRandomSprite;

    public void OnEnable() 
    {
    GameObject lastRandomSprite = Instantiate(ZerullClassic.Instance.RandomBlockages[UnityEngine.Random.Range(0, ZerullClassic.Instance.RandomBlockages.Length)], transform);
    }
    public void OnDisable() => Destroy(lastRandomSprite);
}