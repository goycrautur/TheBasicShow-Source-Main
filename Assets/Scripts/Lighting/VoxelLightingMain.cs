using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelLightingMain : MonoBehaviour
{
    public void addToLightFU(SpriteRenderer spr,bool deleteOrAdd = true)
    {
        if (deleteOrAdd)
        {
            WhiteListShit.Add(spr);
        }
        else
        {
            WhiteListShit.Remove(spr);
        }
    }
    [Header("Lighting Settings")]
    public List<VoxelLightSource> lightSources = new();
    public List<SpriteRenderer> WhiteListShit = new();
    public Color ambientLight = Color.black;

    [Header("Performance Settings")]
    public float lightUpdateInterval = 0.25f;
    public int tilesPerFrame = 10;

    [Header("Preview Settings")]
    public bool previewEnabled = true;

    private MaterialPropertyBlock propertyBlock;
    private Dictionary<Vector3Int, List<LightTile>> tiles = new();
    private Dictionary<Vector3Int, Color> originalTileColors = new();

    private Color previousAmbientLight;
    private List<Vector3> previousLightPositions = new();
    private List<Color> previousLightColors = new();
    private List<float> previousLightRadii = new();

    private bool lightingDirty = true;

    private void Awake()
    {
        if (propertyBlock == null)
            propertyBlock = new MaterialPropertyBlock();
        InitializeTiles();
    }

    private void Start()
    {
        if (Application.isPlaying)
            StartCoroutine(LightingUpdateRoutine());
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var lightSource in lightSources)
        {
            if (lightSource == null) continue;
            Gizmos.color = lightSource.lightColor;
            Gizmos.DrawWireSphere(lightSource.transform.position, lightSource.radius);
        }
    }

    private void InitializeTiles()
    {
        tiles.Clear();
        originalTileColors.Clear();

        var tileObjects = Resources.FindObjectsOfTypeAll<Renderer>();
        foreach (var tileObj in tileObjects)
        {
            Vector3Int pos = Vector3Int.RoundToInt(tileObj.transform.position);

            Renderer rend = tileObj.GetComponent<Renderer>();
            SpriteRenderer spriteRend = tileObj.GetComponent<SpriteRenderer>();

            if (rend == null && spriteRend == null) continue;

            if (!tiles.ContainsKey(pos))
                tiles[pos] = new List<LightTile>();

            tiles[pos].Add(new LightTile
            {
                position = pos,
                renderer = rend != null ? rend : spriteRend,
                lightColor = Color.black
            });

            originalTileColors[pos] = Color.white;
        }
    }

    private IEnumerator LightingUpdateRoutine()
    {
        while (true)
        {
            if (lightingDirty || HasLightingChanged())
            {
                lightingDirty = false;
                CacheLightingState();
                yield return RecalculateLightingAsync();
            }

            yield return new WaitForSeconds(lightUpdateInterval);
        }
    }

    private IEnumerator RecalculateLightingAsync()
    {
        Dictionary<Vector3Int, Color> tileColors = new();

        foreach (var kvp in tiles)
            tileColors[kvp.Key] = ambientLight;

        foreach (var light in lightSources)
        {
            if (light == null || light.radius <= 0f) continue;
            Vector3Int center = Vector3Int.RoundToInt(light.transform.position);
            float radiusSqr = light.radius * light.radius;

            foreach (var kvp in tiles)
            {
                Vector3Int tilePos = kvp.Key;
                float distSqr = (tilePos - center).sqrMagnitude;
                if (distSqr > radiusSqr) continue;

                float attenuation = Mathf.Clamp01(1f - Mathf.Sqrt(distSqr) / light.radius);
                Color contribution = light.lightColor * attenuation;

                if (!light.invertColor)
                {
                    tileColors[tilePos] += contribution;
                }
                else
                {
                    tileColors[tilePos] -= contribution;
                }
            }
            yield return null;
        }

        int count = 0;
        foreach (var kvp in tiles)
        {
            foreach (var tile in kvp.Value)
            {
                tile.lightColor = tileColors[kvp.Key];
                ApplyColorsToTile(tile);
                if (++count >= tilesPerFrame)
                {
                    count = 0;
                    yield return null;
                }
            }
        }
    }

    private bool HasLightingChanged()
    {
        if (ambientLight != previousAmbientLight)
            return true;

        if (lightSources.Count != previousLightPositions.Count)
            return true;

        for (int i = 0; i < lightSources.Count; i++)
        {
            var light = lightSources[i];
            if (light == null) continue;
            if (i >= previousLightPositions.Count) return true;

            if (light.transform.position != previousLightPositions[i] ||
                light.lightColor != previousLightColors[i] ||
                Mathf.Abs(light.radius - previousLightRadii[i]) > 0.001f)
                return true;
        }

        return false;
    }

    private void CacheLightingState()
    {
        previousAmbientLight = ambientLight;
        previousLightPositions.Clear();
        previousLightColors.Clear();
        previousLightRadii.Clear();

        foreach (var light in lightSources)
        {
            if (light == null) continue;
            previousLightPositions.Add(light.transform.position);
            previousLightColors.Add(light.lightColor);
            previousLightRadii.Add(light.radius);
        }
    }

    private void ApplyColorsToTile(LightTile tile)
    {
        if (tile.renderer == null) return;

        if (tile.renderer is SpriteRenderer spriteRenderer)
        {
            foreach (var whiteli in WhiteListShit)
            {
                if (spriteRenderer == whiteli)
                {
                    spriteRenderer.color = tile.lightColor;
                }
            }
        }
        else
        {
            propertyBlock.SetColor("_Color", tile.lightColor);
            propertyBlock.SetColor("_Color0", tile.lightColor);
            tile.renderer.SetPropertyBlock(propertyBlock);
        }
    }

    public void MarkLightingDirty() => lightingDirty = true;

    #region PUBLIC_SECTION
    public void AddLightToArray(VoxelLightSource light)
    {
        StopAllCoroutines();
        tiles.Clear();
        lightSources.Add(light);
        InitializeTiles();
        if (Application.isPlaying)
        {
            StartCoroutine(LightingUpdateRoutine());
        }
    }

    public void RemoveLightFromArray(VoxelLightSource light)
    {
        StopAllCoroutines();
        tiles.Clear();
        lightSources.Remove(light);
        InitializeTiles();
        if (Application.isPlaying)
        {
            StartCoroutine(LightingUpdateRoutine());
        }
    }
    #endregion
}