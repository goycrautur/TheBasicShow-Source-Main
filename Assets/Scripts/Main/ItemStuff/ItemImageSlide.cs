using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RawImage), typeof(RectTransform))]
public class ItemImageSlide : MonoBehaviour
{
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        mainImage = GetComponent<RawImage>();
        restPos = rect.anchoredPosition;
        baseColor = mainImage != null ? mainImage.color : Color.white;
        //slideDistance = 0;
    }

    public void ForceClear()
    {
        StopAllCoroutines();
        DestroyTempIfExists();

        if (mainImage != null)
        {
            mainImage.texture = null;
            mainImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
        }

        rect.anchoredPosition = restPos;
    }

    public void SlideIn(Texture newTexture)
    {
        StopAllCoroutines();
        DestroyTempIfExists();

        if (newTexture == null)
        {
            ForceClear();
            return;
        }

        mainImage.texture = newTexture;
        mainImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f);

        rect.anchoredPosition = restPos - new Vector2(slideDistance, upDistance);
        StartCoroutine(SlideAnchored(rect, restPos));
    }

    public void SlideOut()
    {
        StopAllCoroutines();
        DestroyTempIfExists();
        StartCoroutine(SlideOutAndClear());
    }

    private IEnumerator SlideOutAndClear()
    {
        Vector2 target = rect.anchoredPosition - new Vector2(slideDistance, downDistance);
        yield return SlideAnchored(rect, target);

        if (mainImage != null)
        {
            mainImage.texture = null;
            mainImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
        }
    }

    public void PlaySwapAnimation(Texture newTexture)
    {
        StopAllCoroutines();
        DestroyTempIfExists();

        if (newTexture == null)
        {
            StartCoroutine(SlideOutAndClear());
            return;
        }

        StartCoroutine(Swap(newTexture));
    }

    private IEnumerator Swap(Texture newTex)
    {
        rect.anchoredPosition = restPos;

        currentTempGO = new GameObject("TempIncoming", typeof(RectTransform), typeof(RawImage));
        currentTempGO.transform.SetParent(transform.parent, false);
        currentTempGO.transform.SetAsLastSibling();

        RectTransform tempRect = currentTempGO.GetComponent<RectTransform>();
        RawImage tempImg = currentTempGO.GetComponent<RawImage>();

        tempRect.anchorMin = rect.anchorMin;
        tempRect.anchorMax = rect.anchorMax;
        tempRect.pivot = rect.pivot;
        tempRect.sizeDelta = rect.sizeDelta;

        Vector2 tempStartPos = restPos - new Vector2(slideDistance, upDistance);
        tempRect.anchoredPosition = tempStartPos;

        tempImg.texture = newTex;
        tempImg.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f);

        Vector2 oldTargetPos = restPos - new Vector2(slideDistance, downDistance);
        Vector2 tempTargetPos = restPos;

        Coroutine oldSlide = StartCoroutine(SlideAnchored(rect, oldTargetPos));
        Coroutine newSlide = StartCoroutine(SlideAnchored(tempRect, tempTargetPos));

        yield return newSlide;
        yield return oldSlide;

        mainImage.texture = newTex;
        mainImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f);
        rect.anchoredPosition = restPos;

        DestroyTempIfExists();
    }

    private void DestroyTempIfExists()
    {
        if (currentTempGO != null)
        {
            Destroy(currentTempGO);
            currentTempGO = null;
        }
    }

    private IEnumerator SlideAnchored(RectTransform r, Vector2 target)
    {
        while (Vector2.Distance(r.anchoredPosition, target) > 1f)
        {
            spinVal += spinSpeed * Time.unscaledDeltaTime;
            r.rotation = Quaternion.Euler(0f, spinVal, 0f);
            r.anchoredPosition = Vector2.MoveTowards(r.anchoredPosition, target, speed * Time.unscaledDeltaTime);
            yield return null;
        }
        r.anchoredPosition = target;
        spinVal = 0f;
        r.rotation = Quaternion.Euler(0f,0f,0f);
    }

    [Header("Slide Settings")]
    public float speed = 567f;
    public float spinSpeed=100,slideDistance = 123f,upDistance,downDistance;
    public int tsId;

    private Vector2 restPos;
    private RawImage mainImage;
    private RectTransform rect;
    private GameObject currentTempGO;
    private Color baseColor;
    private float spinVal;
}