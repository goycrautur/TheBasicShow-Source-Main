using UnityEngine;

[ExecuteAlways]
public class SpriteController : MonoBehaviour
{
    private void OnEnable()
    {
        Initialize();
        Apply();
    }

    private void OnValidate() => Initialize();
    
    private void Initialize()
    {
        if (block == null)
        {
            block = new MaterialPropertyBlock();
        }

        if (!targetRenderer)
        {
            targetRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (targetRenderer)
        {
            lastSprite = targetRenderer.sprite;
            lastColor = targetRenderer.color;
        }
    }

    private void Update()
    {
        SyncFromRenderer();
        Apply();
    }

    private void SyncFromRenderer()
    {
        if (targetRenderer.sprite != lastSprite)
        {
            lastSprite = targetRenderer.sprite;

            if (lastSprite != null)
            {
                mainTex = lastSprite.texture;
            }
        }

        if (targetRenderer.color != lastColor)
        {
            lastColor = targetRenderer.color;
            color = lastColor;
        }
    }

    private void Apply()
    {
        if (!targetRenderer)
        {
            return;
        }

        targetRenderer.GetPropertyBlock(block);
        block.SetColor("_Color", color * externalLightColor);
        Texture textureToUse = mainTex;
        if (textureToUse == null && targetRenderer.sprite != null)
        {
            textureToUse = targetRenderer.sprite.texture;
        }
        if (textureToUse != null)
        {
            block.SetTexture("_MainTex", textureToUse);
        }
        block.SetFloat("_BillboardMode", billboardMode);

        block.SetFloat("_UseLightmap", useLightmap ? 1f : 0f);
        if (lightMap)
        {
            block.SetTexture("_LightMap", lightMap);
        }

        block.SetFloat("_UseSmoothTransition", useSmoothTransition ? 1f : 0f);
        block.SetFloat("_TransitionThreshold", transitionThreshold);

        block.SetFloat("_UseColorMask", useColorMask ? 1f : 0f);
        if (colorMask)
        {
            block.SetTexture("_ColorMask", colorMask);
        }

        block.SetColor("_MaskColor", maskColor);
        block.SetFloat("_MaskBrightness", maskBrightness);

        block.SetFloat("_UseOverlay", useOverlay ? 1f : 0f);
        block.SetColor("_Color1", OverlayColor * externalLightColor2);
        block.SetFloat("_BlendFactor", blendFactor);

        block.SetFloat("_UseBobbing", useBobbing ? 1f : 0f);
        block.SetVector("_BobAmount", bobAmount);
        block.SetVector("_BobSpeed", bobSpeed);

        block.SetFloat("_UseShaking", useShaking ? 1f : 0f);
        block.SetFloat("_ShakeAmount", shakeAmount);
        block.SetFloat("_ShakeRotationAmount", shakeRotationAmount);
        block.SetFloat("_ShakeSpeed", shakeSpeed);

        block.SetFloat("_UseRandomZRotation", useRandomZrotation ? 1f : 0f);
        block.SetFloat("_RandomZRotationAmount", rotationAmount);

        block.SetFloat("_UseTransparency", useTransparency ? 1f : 0f);
        block.SetFloat("_Cutoff", cutoffOffset);

        block.SetInt("_Contrast", ontrast);

        block.SetInt("_ValueX", valueX);
        block.SetInt("_ValueY", valueY);
        block.SetInt("_ValueZ", valueZ);
        

        targetRenderer.SetPropertyBlock(block);
    }

    [Header("Base")]
    public Color color = Color.white;
    public Texture mainTex;

    [Header("Billboard")]
    [Range(0, 2)] public int billboardMode = 2;

    [Header("Dithered Transparency")]
    public bool useTransparency = false;
    [Range(-1f, 1f)] public float cutoffOffset = 0f;

    [Header("Lightmap")]
    public bool useLightmap = false;
    public Texture lightMap;
    public bool useSmoothTransition = true;
    [Range(0.01f, 1f)] public float transitionThreshold = 0.5f;

    [Header("Color Mask")]
    public bool useColorMask = false;
    public Texture colorMask;
    public Color maskColor = Color.red;
    [Range(0, 5)] public float maskBrightness = 2f;

    [Header("Overlay")]
    public bool useOverlay = false;
    public Color OverlayColor = Color.white;
    [Range(0, 1)] public float blendFactor = 0f;

    [Header("Bobbing")]
    public bool useBobbing = false;
    public Vector3 bobAmount = new Vector3(0f, 0.42f, 0f), bobSpeed = new Vector3(1, 2, 1);

    [Header("Raldi Shaking")]
    public bool useShaking = false;
    public float shakeAmount = 0.5f, shakeRotationAmount = 0.5f, shakeSpeed = 24f;

    [Header("Z Rotation")]
    public bool useRandomZrotation = false;
    public float rotationAmount;

    [Header("Constrat and Glitch color stuff")]
    [Range(0, 4)] public int ontrast = 1;
    [Range(0, 256)] public int valueX,valueY,valueZ;

    public SpriteRenderer targetRenderer;
    public MaterialPropertyBlock block;
    public Sprite lastSprite;
    public Color lastColor;
    [HideInInspector] public Color externalLightColor = Color.white,externalLightColor2 = Color.white;
}