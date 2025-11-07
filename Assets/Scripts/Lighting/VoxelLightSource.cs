using UnityEngine;

public class VoxelLightSource : MonoBehaviour
{
    public VoxelLightingMain vlm;

    [Range(1f, 128f)] public float radius = 8f;

    public Color lightColor = Color.black;

    public bool invertColor;

    public bool flicker = false;

    public float flickerIntensity = 0.2f;

    public float flickerSpeed = 5f;

    private float baseRadius;

    private Color baseColor;

    private void Start()
    {
        baseRadius = radius;
        baseColor = lightColor;
    }

    private void OnDisable()
    {
        if (vlm == null) return;

        vlm.RemoveLightFromArray(this);
    }

    private void OnEnable()
    {
        if (vlm == null) return;

        vlm.AddLightToArray(this);
    }

    private void OnWillRenderObject()
    {
        if (!flicker) return;

        float flickerOffset = Mathf.PerlinNoise(Time.time * flickerSpeed, transform.position.x + transform.position.y) - 0.5f;
        radius = baseRadius + flickerOffset * flickerIntensity;
        float brightnessOffset = flickerOffset * 0.1f;
        lightColor = baseColor * (1f + brightnessOffset);
    }

}