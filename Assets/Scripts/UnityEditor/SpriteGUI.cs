#if UNITY_EDITOR
using UnityEditor;

public class SpriteGUI : ShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
        MaterialProperty color = FindProperty("_Color", props);
        MaterialProperty mainTex = FindProperty("_MainTex", props);

        MaterialProperty lightMap = FindProperty("_LightMap", props);
        MaterialProperty useLightmap = FindProperty("_UseLightmap", props);
        MaterialProperty useSmoothTransition = FindProperty("_UseSmoothTransition", props);
        MaterialProperty transitionThreshold = FindProperty("_TransitionThreshold", props);

        MaterialProperty useOverlay = FindProperty("_UseOverlay", props);
        MaterialProperty blendFactor = FindProperty("_BlendFactor", props);

        MaterialProperty useBobbing = FindProperty("_UseBobbing", props);
        MaterialProperty bobAmount = FindProperty("_BobAmount", props);
        MaterialProperty bobSpeed = FindProperty("_BobSpeed", props);

        MaterialProperty useGlitch = FindProperty("_UseGlitch", props);
        MaterialProperty glitchX = FindProperty("_GlitchValueX", props);
        MaterialProperty glitchY = FindProperty("_GlitchValueY", props);
        MaterialProperty glitchZ = FindProperty("_GlitchValueZ", props);

        materialEditor.ShaderProperty(color, color.displayName);
        materialEditor.ShaderProperty(mainTex, mainTex.displayName);

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Lightmap Settings", EditorStyles.boldLabel);
        materialEditor.ShaderProperty(useLightmap, useLightmap.displayName);
        if (useLightmap.floatValue > 0.5f)
        {
            materialEditor.ShaderProperty(lightMap, lightMap.displayName);
            materialEditor.ShaderProperty(useSmoothTransition, useSmoothTransition.displayName);
            materialEditor.ShaderProperty(transitionThreshold, transitionThreshold.displayName);
        }

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Overlay Settings", EditorStyles.boldLabel);
        materialEditor.ShaderProperty(useOverlay, useOverlay.displayName);
        if (useOverlay.floatValue > 0.5f)
        {
            materialEditor.ShaderProperty(blendFactor, blendFactor.displayName);
        }

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Bobbing Settings", EditorStyles.boldLabel);
        materialEditor.ShaderProperty(useBobbing, useBobbing.displayName);
        if (useBobbing.floatValue > 0.5f)
        {
            materialEditor.ShaderProperty(bobAmount, bobAmount.displayName);
            materialEditor.ShaderProperty(bobSpeed, bobSpeed.displayName);
        }

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Glitch Settings", EditorStyles.boldLabel);
        materialEditor.ShaderProperty(useGlitch, useGlitch.displayName);
        if (useGlitch.floatValue > 0.5f)
        {
            materialEditor.ShaderProperty(glitchX, glitchX.displayName);
            materialEditor.ShaderProperty(glitchY, glitchY.displayName);
            materialEditor.ShaderProperty(glitchZ, glitchZ.displayName);
        }
    }
}
#endif