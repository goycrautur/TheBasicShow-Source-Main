#if UNITY_EDITOR
using UnityEditor;

public class MaterialGUI : ShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
        MaterialProperty color0 = FindProperty("_Color0", props);
        MaterialProperty color1 = FindProperty("_Color1", props);
        MaterialProperty mainTex = FindProperty("_MainTex", props);

        MaterialProperty secondTex = FindProperty("_SecondTex", props);
        MaterialProperty secondaryDiff = FindProperty("_SecondaryDiffrent", props);
        MaterialProperty mask = FindProperty("_Mask", props);
        MaterialProperty lightMap = FindProperty("_LightMap", props);

        MaterialProperty cutoff = FindProperty("_Cutoff", props);
        MaterialProperty transitionThreshold = FindProperty("_TransitionThreshold", props);

        MaterialProperty swap = FindProperty("_Swap", props);
        MaterialProperty useSecondTex = FindProperty("_UseSecondTex", props);
        MaterialProperty useMask = FindProperty("_UseMask", props);
        MaterialProperty useTransparency = FindProperty("_UseTransparency", props);
        MaterialProperty useLightmap = FindProperty("_UseLightmap", props);
        MaterialProperty useSmoothTransition = FindProperty("_UseSmoothTransition", props);

        MaterialProperty useGlitch = FindProperty("_UseGlitch", props);
        MaterialProperty valX = FindProperty("_ValueX", props);
        MaterialProperty valY = FindProperty("_ValueY", props);
        MaterialProperty valZ = FindProperty("_ValueZ", props);
        MaterialProperty vertSeed = FindProperty("_VertexGlitchSeed", props);
        MaterialProperty vertInt = FindProperty("_VertexGlitchIntensity", props);

        materialEditor.ShaderProperty(color0, color0.displayName);
        materialEditor.ShaderProperty(mainTex, mainTex.displayName);

        EditorGUILayout.Space();

        materialEditor.ShaderProperty(useSecondTex, useSecondTex.displayName);
        if (useSecondTex.floatValue > 0.5f)
        {
            materialEditor.ShaderProperty(color1, color1.displayName);
            materialEditor.ShaderProperty(secondTex, secondTex.displayName);
            materialEditor.ShaderProperty(secondaryDiff, secondaryDiff.displayName);
            materialEditor.ShaderProperty(swap, swap.displayName);
        }

        materialEditor.ShaderProperty(useMask, useMask.displayName);
        if (useMask.floatValue > 0.5f)
        {
            materialEditor.ShaderProperty(mask, mask.displayName);
        }

        materialEditor.ShaderProperty(useTransparency, useTransparency.displayName);
        if (useTransparency.floatValue > 0.5f)
        {
            materialEditor.ShaderProperty(cutoff, cutoff.displayName);
        }

        materialEditor.ShaderProperty(useLightmap, useLightmap.displayName);
        if (useLightmap.floatValue > 0.5f)
        {
            materialEditor.ShaderProperty(lightMap, lightMap.displayName);
            materialEditor.ShaderProperty(useSmoothTransition, useSmoothTransition.displayName);
            materialEditor.ShaderProperty(transitionThreshold, transitionThreshold.displayName);
        }

        materialEditor.ShaderProperty(useGlitch, useGlitch.displayName);
        if (useGlitch.floatValue > 0.5f)
        {
            materialEditor.ShaderProperty(valX, valX.displayName);
            materialEditor.ShaderProperty(valY, valY.displayName);
            materialEditor.ShaderProperty(valZ, valZ.displayName);
            materialEditor.ShaderProperty(vertSeed, vertSeed.displayName);
            materialEditor.ShaderProperty(vertInt, vertInt.displayName);
        }
    }
}
#endif