using UnityEngine;

public class BlockageRandomizer : MonoBehaviour
{
    private GameObject lastRandomSprite;
    [SerializeField] private Vector3 size = new Vector3 (1f,1f,1f);

    public void OnEnable() 
    {
    GameObject lastRandomSprite = Instantiate(ZerullClassic.Instance.RandomBlockages[UnityEngine.Random.Range(0, ZerullClassic.Instance.RandomBlockages.Length)], transform);
    lastRandomSprite.transform.localScale = size;
    }
    public void OnDisable() => Destroy(lastRandomSprite);
}