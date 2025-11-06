using TMPro;
using UnityEngine;

public class DetentionTextScript : MonoBehaviour
{
    private void Awake() => spriteRenderers = new SpriteRenderer[] {hoursTensRenderer, hoursOnesRenderer, ColonRenderer, minutesTensRenderer, minutesOnesRenderer, ColonRenderer2, secondsTensRenderer, secondsOnesRenderer};

    private void Update()
    {
        bool useNewTimer = AdditionalGameCustomizer.Instance?.OldDetentionTimer == false;
        bool hasTime = door.lockTime > 0f;

        if (useNewTimer)
        {
            float displayTime = hasTime ? door.lockTime : 0f;
            UpdateDisplay(displayTime, hasTime ? numberColor : numberOutColor);
        }
        else
        {
            TimerText.text = hasTime ? $"You have detention! \n{Mathf.CeilToInt(door.lockTime)} seconds remain!" : string.Empty;
        }
    }

    private void UpdateDisplay(float time, Color color)
    {
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes2 = Mathf.FloorToInt(time / 1440);
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        UpdateDigitDisplay(hours / 10, hoursTensRenderer);
        UpdateDigitDisplay(hours % 10, hoursOnesRenderer);
        UpdateDigitDisplay(minutes2 / 10, minutesTensRenderer);
        UpdateDigitDisplay(minutes % 10, minutesOnesRenderer);
        UpdateDigitDisplay(seconds / 10, secondsTensRenderer);
        UpdateDigitDisplay(seconds % 10, secondsOnesRenderer);

        SetSpriteColors(color);
    }

    private void UpdateDigitDisplay(int digit, SpriteRenderer renderer)
    {
        if (digit >= 0 && digit < numberSprites.Length)
        {
            renderer.sprite = numberSprites[digit];
        }
    }

    private void SetSpriteColors(Color color)
    {
        foreach (var renderer in spriteRenderers)
        {
            renderer.color = color;
        }
    }

    [Header("References")]
    [SerializeField] private DoorScript door;
    [SerializeField] private SpriteRenderer hoursTensRenderer, hoursOnesRenderer, ColonRenderer, minutesTensRenderer, minutesOnesRenderer, ColonRenderer2, secondsTensRenderer, secondsOnesRenderer;

    [Header("Appearance Settings")]
    [SerializeField] private Sprite[] numberSprites;
    [SerializeField] private Color numberColor, numberOutColor;
    [SerializeField] private TMP_Text TimerText;

    private SpriteRenderer[] spriteRenderers;

}