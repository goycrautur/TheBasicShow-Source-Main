using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class BackdropScroller : MonoBehaviour
{
    public float speed; //Speed Of Scroll
    public Vector2 direction; //Normalized Direction Of Scrol
    public RawImage img;
    public bool UnscaledTime;
    private void Update()
    {
        float timeval = UnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        img.uvRect = new Rect(img.uvRect.position + direction * timeval * speed,img.uvRect.size);
    }
}
