#if UNITY_EDITOR
using UnityEditor;

public class SpriteGUI : ShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
        MaterialProperty color = FindProperty("_Color", props);
        MaterialProperty mainTex = FindProperty("_MainTex", props);

        MaterialProperty billboardMode = FindProperty("_BillboardMode", props);

        MaterialProperty lightMap = FindProperty("_LightMap", props);
        MaterialProperty useLightmap = FindProperty("_UseLightmap", props);
        MaterialProperty useSmoothTransition = FindProperty("_UseSmoothTransition", props);
        MaterialProperty transitionThreshold = FindProperty("_TransitionThreshold", props);

        MaterialProperty useColorMask = FindProperty("_UseColorMask", props);
        MaterialProperty maskColor = FindProperty("_MaskColor", props);
        MaterialProperty maskBrightness = FindProperty("_MaskBrightness", props);
        MaterialProperty colorMask = FindProperty("_ColorMask", props);

        MaterialProperty useOverlay = FindProperty("_UseOverlay", props);
        MaterialProperty blendFactor = FindProperty("_BlendFactor", props);

        MaterialProperty useBobbing = FindProperty("_UseBobbing", props);
        MaterialProperty bobAmount = FindProperty("_BobAmount", props);
        MaterialProperty bobSpeed = FindProperty("_BobSpeed", props);

        MaterialProperty startShaking = FindProperty("_UseShaking", props);
        MaterialProperty shakeAmount = FindProperty("_ShakeAmount", props);
        MaterialProperty shakeRotationAmount = FindProperty("_ShakeRotationAmount", props);
        MaterialProperty shakeSpeed = FindProperty("_ShakeSpeed", props);

        MaterialProperty useRandomZrotation = FindProperty("_UseRandomZRotation", props);
        MaterialProperty zRotationAmount = FindProperty("_RandomZRotationAmount", props);

        materialEditor.ShaderProperty(color, color.displayName);
        materialEditor.ShaderProperty(mainTex, mainTex.displayName);

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Billboard Settings", EditorStyles.boldLabel);
        materialEditor.ShaderProperty(billboardMode, billboardMode.displayName);

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

        EditorGUILayout.LabelField("Color Mask Settings", EditorStyles.boldLabel);
        materialEditor.ShaderProperty(useColorMask, useColorMask.displayName);
        if (useColorMask.floatValue > 0.5f)
        {
            materialEditor.ShaderProperty(maskColor, maskColor.displayName);
            materialEditor.ShaderProperty(maskBrightness, maskBrightness.displayName);
            materialEditor.ShaderProperty(colorMask, colorMask.displayName);
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

        EditorGUILayout.LabelField("Raldi Settings", EditorStyles.boldLabel);
        materialEditor.ShaderProperty(startShaking, startShaking.displayName);
        if (startShaking.floatValue > 0.5f)
        {
            materialEditor.ShaderProperty(shakeAmount, shakeAmount.displayName);
            materialEditor.ShaderProperty(shakeRotationAmount, shakeRotationAmount.displayName);
            materialEditor.ShaderProperty(shakeSpeed, shakeSpeed.displayName);
        }
        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Random Z Rotation", EditorStyles.boldLabel);
        materialEditor.ShaderProperty(useRandomZrotation, useRandomZrotation.displayName);
        if (useRandomZrotation.floatValue > 0.5f)
        {
            materialEditor.ShaderProperty(zRotationAmount, zRotationAmount.displayName);
        }
    }
}
#endif