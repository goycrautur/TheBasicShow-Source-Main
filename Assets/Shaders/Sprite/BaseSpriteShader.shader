Shader "Legacy Shaders/Simplified/BaseSpriteRenderer"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
        [NoScaleOffset] [MainTexture] _MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
        [NoScaleOffset] _LightMap("Lightmap (Greyscale)", 2D) = "white" {}

        [Enum(Off,0,Full Billboard,1,Lock Y Axis,2)] _BillboardMode("Billboard Mode", Float) = 0

        [Toggle] _UseLightmap("Enable Lightmap", Float) = 0
        [Toggle] _UseSmoothTransition("Use Smooth Transition", Float) = 1
        _TransitionThreshold("Lightmap Transition Threshold", Range(0.01, 1)) = 0.5

        _MaskColor ("Color Mask Tint", Color) = (1,0,0,1)
        _MaskBrightness ("Color Mask Brightness", Range(0, 5)) = 2.0
        _ColorMask ("Color Mask (B&W)", 2D) = "black" {}
        [Toggle] _UseColorMask ("Use Color Mask", Float) = 0

        [Toggle] _UseOverlay("Enable Overlay", Float) = 0
        _Color1("Overlay Color", Color) = (1,1,1,1)
        _BlendFactor("Overlay Blend Factor", Range(0, 1)) = 0

        [Toggle] _UseBobbing("Enable Bobbing", Float) = 0
        _BobAmount("Bobbing Amount (XYZ)", Vector) = (0, 0, 0)
        _BobSpeed("Bobbing Speed (XYZ)", Vector) = (1, 1, 1)

        [Toggle] _UseShaking("Enable Shaking", Float) = 0
        _ShakeAmount("Shake Position Amount", Float) = 0.5
        _ShakeRotationAmount("Shake Rotation Amount", Float) = 0.5
        _ShakeSpeed("Shake Speed (FPS)", Float) = 24.0

        [Toggle] _UseRandomZRotation("Enable Random Z Rotation", Float) = 0
        _RandomZRotationAmount("Random Z Rotation Max", Float) = 0.3

        [Toggle] _UseTransparency("Use Dither Transparency", Float) = 0
        _Cutoff("Alpha Cutoff Offset", Range(-1,1)) = 0

        _Contrast("Contrast", Range(0,4)) = 1

        _ValueX("ValueX", Range(0,256)) = 1
        _ValueY("ValueY", Range(0,256)) = 1
        _ValueZ("ValueZ", Range(0,256)) = 1
    }

    CustomEditor "SpriteGUI"

    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        LOD 200
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade vertex:vert addshadow
        #pragma target 3.0

        fixed4 _Color, _Color1, _MaskColor;
        float3 _BobAmount, _BobSpeed;
        sampler2D _MainTex, _LightMap, _ColorMask;
        
        float _BillboardMode, _TransitionThreshold, _MaskBrightness;
        float _UseLightmap, _UseSmoothTransition, _UseOverlay, _UseColorMask, _UseBobbing, _UseShaking, _UseRandomZRotation;
        float _ShakeAmount, _ShakeRotationAmount, _ShakeSpeed, _RandomZRotationAmount, _UseTransparency, _Cutoff;
        float _Contrast;
        int _ValueX;
        int _ValueY;
        int _ValueZ;

        struct Input
        {
            float2 uv_MainTex, uv_LightMap;
            fixed4 color : COLOR;
            float4 screenPos;
        };

        float GetBayerDither(float2 screenPos)
        {
            int x = int(fmod(screenPos.x, 4.0)); int y = int(fmod(screenPos.y, 4.0));
            float4x4 bayer = float4x4(1.0/17.0, 9.0/17.0, 3.0/17.0, 11.0/17.0, 13.0/17.0, 5.0/17.0, 15.0/17.0, 7.0/17.0, 4.0/17.0, 12.0/17.0, 2.0/17.0, 10.0/17.0, 16.0/17.0, 8.0/17.0, 14.0/17.0, 6.0/17.0);
            return bayer[y][x]; 
        }

        fixed3 ApplyXOR(fixed3 col)
        {
            int x = (int)(col.r * 256);
            x ^= _ValueX;
            col.r = x / 256.0;

            int y = (int)(col.g * 256);
            y ^= _ValueY;
            col.g = y / 256.0;

            int z = (int)(col.b * 256);
            z ^= _ValueZ;
            col.b = z / 256.0;

            return col;
        }

        void vert(inout appdata_full v)
        {
            float totalZAngle = 0;

            if (_UseRandomZRotation > 0.5)
            {
                float3 stableBasis = float3(length(unity_ObjectToWorld._m00_m10_m20), length(unity_ObjectToWorld._m01_m11_m21), length(unity_ObjectToWorld._m02_m12_m22));
                float randomSeed = stableBasis.x * 12.3 + stableBasis.y * 45.7 + stableBasis.z * 91.1;
                float baseRandomZ = (frac(sin(randomSeed) * 43758.5453) - 0.5) * _RandomZRotationAmount;
                totalZAngle += baseRandomZ;
            }

            float shakeTime = 0;
            if (_UseShaking > 0.5)
            {
                shakeTime = floor(_Time.y * _ShakeSpeed);
                float shakeAngle = (frac(sin(shakeTime * 51.2141) * 43758.5453) - 0.5) * _ShakeRotationAmount;
                totalZAngle += shakeAngle;
            }

            if (abs(totalZAngle) > 0.0001)
            {
                float s = sin(totalZAngle);
                float c = cos(totalZAngle);
                float2 rotatedXY = float2(v.vertex.x * c - v.vertex.y * s, v.vertex.x * s + v.vertex.y * c);
                v.vertex.xy = rotatedXY;
            }

            float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

            if (_BillboardMode > 0.5)
            {
                float3 center = unity_ObjectToWorld._m03_m13_m23;
                float3 right = UNITY_MATRIX_V[0].xyz;
                float3 up    = UNITY_MATRIX_V[1].xyz;
                float3 fwd   = UNITY_MATRIX_V[2].xyz;

                if (_BillboardMode == 2)
                {
                    up = float3(0, 1, 0);
                    right = normalize(float3(UNITY_MATRIX_V[0].x, 0, UNITY_MATRIX_V[0].z));
                    fwd = normalize(cross(right, up));
                }

                float3 scale = float3(length(unity_ObjectToWorld._m00_m10_m20), length(unity_ObjectToWorld._m01_m11_m21), length(unity_ObjectToWorld._m02_m12_m22));
                float3 localVertex = v.vertex.xyz * scale;
                worldPos.xyz = center + right * localVertex.x + up * localVertex.y + fwd * localVertex.z;
            }

            if (_UseBobbing > 0.5)
            {
                if (any(_BobAmount != 0))
                {
                    half time = _Time.y;
                    float3 bobbingPosition = float3(sin(time * _BobSpeed.x), sin(time * _BobSpeed.y), sin(time * _BobSpeed.z)) * _BobAmount;
                    worldPos.xyz += bobbingPosition;
                }
            }

            if (_UseShaking > 0.5)
            {
                float shakeX = (frac(sin(shakeTime * 12.9898) * 43758.5453) - 0.5) * 2.0;
                float shakeY = (frac(sin(shakeTime * 78.2330) * 43758.5453) - 0.5) * 2.0;
                float shakeZ = (frac(sin(shakeTime * 37.7190) * 43758.5453) - 0.5) * 2.0;
                worldPos.xyz += float3(shakeX, shakeY, shakeZ) * _ShakeAmount;
            }

            v.vertex = mul(unity_WorldToObject, worldPos);
        }

        fixed3 ApplyContrast(fixed3 color, float contrast)
        {
            return ((color - 0.5) * contrast) + 0.5;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex);
            texColor.rgb = ApplyXOR(texColor.rgb);
            fixed4 baseColor = texColor * _Color * IN.color;

            baseColor.rgb = ApplyContrast(
                baseColor.rgb,
                _Contrast
            );

            baseColor.rgb = saturate(baseColor.rgb);

            if (_UseTransparency > 0.5)
            {
                float2 pixelPos = IN.screenPos.xy / IN.screenPos.w * _ScreenParams.xy;
                clip(baseColor.a - GetBayerDither(pixelPos) - _Cutoff); 
            }

            if (_UseColorMask > 0.5)
            {
                fixed maskVal = tex2D(_ColorMask, IN.uv_MainTex).r;
                baseColor.rgb = lerp(baseColor.rgb, baseColor.rgb * _MaskColor.rgb * _MaskBrightness, maskVal);
            }

            if (_UseLightmap > 0.5)
            {
                fixed lightIntensity = tex2D(_LightMap, IN.uv_LightMap).r;
                fixed3 ambient = _Color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb;

                float blendFactor = 0;
                if (_UseSmoothTransition > 0.5)
                {
                    blendFactor = smoothstep(0.0, _TransitionThreshold, lightIntensity);
                }
                else
                {
                    blendFactor = step(_TransitionThreshold, lightIntensity);
                }

                fixed3 unlitColor = (texColor * IN.color).rgb;
                if (_UseColorMask > 0.5)
                {
                    fixed maskVal = tex2D(_ColorMask, IN.uv_MainTex).r;
                    unlitColor = lerp(unlitColor, unlitColor * _MaskColor.rgb * _MaskBrightness, maskVal);
                }

                fixed3 blendedLight = lerp(ambient, fixed3(lightIntensity, lightIntensity, lightIntensity), blendFactor);
                o.Emission = unlitColor * max(blendedLight - ambient, 0);
            }
            else
            {
                o.Emission = 0;
            }

            o.Albedo = baseColor.rgb;
            o.Alpha = baseColor.a;
        }
        ENDCG

        Pass
        {
            Name "Overlay"
            Tags { "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert addshadow
            #pragma fragment frag
            #pragma target 3.0
            
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            fixed4 _Color, _Color1, _MaskColor;
            float3 _BobAmount, _BobSpeed;
            sampler2D _MainTex, _LightMap, _ColorMask;
            
            float _BillboardMode, _BlendFactor, _TransitionThreshold, _MaskBrightness;
            float _UseLightmap, _UseSmoothTransition, _UseOverlay, _UseColorMask, _UseBobbing, _UseShaking, _UseRandomZRotation;
            float _ShakeAmount, _ShakeRotationAmount, _ShakeSpeed, _RandomZRotationAmount,_UseTransparency, _Cutoff;
            float _Contrast;
            int _ValueX;
            int _ValueY;
            int _ValueZ;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_FOG_COORDS(1)
            };

            float GetBayerDither(float2 screenPos)
            {
                int x = int(fmod(screenPos.x, 4.0)); int y = int(fmod(screenPos.y, 4.0));
                float4x4 bayer = float4x4(1.0/17.0, 9.0/17.0, 3.0/17.0, 11.0/17.0, 13.0/17.0, 5.0/17.0, 15.0/17.0, 7.0/17.0, 4.0/17.0, 12.0/17.0, 2.0/17.0, 10.0/17.0, 16.0/17.0, 8.0/17.0, 14.0/17.0, 6.0/17.0);
                return bayer[y][x]; 
            }

            fixed3 ApplyXOR(fixed3 col)
            {
                int x = (int)(col.r * 256);
                x ^= _ValueX;
                col.r = x / 256.0;

                int y = (int)(col.g * 256);
                y ^= _ValueY;
                col.g = y / 256.0;

                int z = (int)(col.b * 256);
                z ^= _ValueZ;
                col.b = z / 256.0;

                return col;
            }

            v2f vert(appdata_t v)
            {
                v2f o;
                float totalZAngle = 0;

                if (_UseRandomZRotation > 0.5)
                {
                    float3 stableBasis = float3(length(unity_ObjectToWorld._m00_m10_m20), length(unity_ObjectToWorld._m01_m11_m21), length(unity_ObjectToWorld._m02_m12_m22));
                    float randomSeed = stableBasis.x * 12.3 + stableBasis.y * 45.7 + stableBasis.z * 91.1;
                    float baseRandomZ = (frac(sin(randomSeed) * 43758.5453) - 0.5) * _RandomZRotationAmount;
                    totalZAngle += baseRandomZ;
                }

                float shakeTime = 0;
                if (_UseShaking > 0.5)
                {
                    shakeTime = floor(_Time.y * _ShakeSpeed);
                    float shakeAngle = (frac(sin(shakeTime * 51.2141) * 43758.5453) - 0.5) * _ShakeRotationAmount;
                    totalZAngle += shakeAngle;
                }

                if (abs(totalZAngle) > 0.0001)
                {
                    float s = sin(totalZAngle);
                    float c = cos(totalZAngle);
                    float2 rotatedXY = float2(v.vertex.x * c - v.vertex.y * s, v.vertex.x * s + v.vertex.y * c);
                    v.vertex.xy = rotatedXY;
                }

                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

                if (_BillboardMode > 0.5)
                {
                    float3 center = unity_ObjectToWorld._m03_m13_m23;
                    float3 right = UNITY_MATRIX_V[0].xyz;
                    float3 up    = UNITY_MATRIX_V[1].xyz;
                    float3 fwd   = UNITY_MATRIX_V[2].xyz;

                    if (_BillboardMode == 2)
                    {
                        up = float3(0, 1, 0);
                        right = normalize(float3(UNITY_MATRIX_V[0].x, 0, UNITY_MATRIX_V[0].z));
                        fwd = normalize(cross(right, up));
                    }
 
                    float3 scale = float3(length(unity_ObjectToWorld._m00_m10_m20), length(unity_ObjectToWorld._m01_m11_m21), length(unity_ObjectToWorld._m02_m12_m22));
                    float3 localVertex = v.vertex.xyz * scale;
                    worldPos.xyz = center + right * localVertex.x + up * localVertex.y + fwd * localVertex.z;
                }  

                if (_UseBobbing > 0.5)
                {
                    if (any(_BobAmount != 0))
                    {
                        half time = _Time.y;
                        float3 bobbingPosition = float3(sin(time * _BobSpeed.x), sin(time * _BobSpeed.y), sin(time * _BobSpeed.z) ) * _BobAmount;
                        worldPos.xyz += bobbingPosition;
                    }
                }

                if (_UseShaking > 0.5)
                {
                    float shakeX = (frac(sin(shakeTime * 12.9898) * 43758.5453) - 0.5) * 2.0;
                    float shakeY = (frac(sin(shakeTime * 78.2330) * 43758.5453) - 0.5) * 2.0;
                    float shakeZ = (frac(sin(shakeTime * 37.7190) * 43758.5453) - 0.5) * 2.0;
                    worldPos.xyz += float3(shakeX, shakeY, shakeZ) * _ShakeAmount;
                }

                o.pos = mul(UNITY_MATRIX_VP, worldPos);
                o.uv = v.uv;
                o.color = v.color;
                
                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }
            
            fixed3 ApplyContrast(fixed3 color, float contrast)
            {
                return ((color - 0.5) * contrast) + 0.5;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                if (_UseOverlay < 0.5)
                {
                    discard;
                }

                fixed4 texColor = tex2D(_MainTex, i.uv);
                texColor.rgb = ApplyXOR(texColor.rgb); 
                fixed4 baseColor = texColor * _Color * i.color;

                baseColor.rgb = ApplyContrast(
                    baseColor.rgb,
                    _Contrast
                );

                baseColor.rgb = saturate(baseColor.rgb);

                if (_UseTransparency > 0.5)
                {
                    clip(baseColor.a - GetBayerDither(i.pos.xy) - _Cutoff);
                }

                if (_UseColorMask > 0.5)
                {
                    fixed maskVal = tex2D(_ColorMask, i.uv).r;
                    baseColor.rgb = lerp(baseColor.rgb, baseColor.rgb * _MaskColor.rgb * _MaskBrightness, maskVal);
                }

                fixed3 sceneAmbient = UNITY_LIGHTMODEL_AMBIENT.rgb;
                fixed3 litColor = baseColor.rgb * sceneAmbient;

                if (_UseLightmap > 0.5)
                {
                    fixed lightIntensity = tex2D(_LightMap, i.uv).r;
                    float blendFactor = 0;
                    
                    if (_UseSmoothTransition > 0.5)
                    {
                        blendFactor = smoothstep(0.0, _TransitionThreshold, lightIntensity);
                    }
                    else
                    {
                        blendFactor = step(_TransitionThreshold, lightIntensity);
                    }
                     
                    fixed3 unlitColor = (texColor * i.color).rgb;
                    if (_UseColorMask > 0.5)
                    {
                        fixed maskVal = tex2D(_ColorMask, i.uv).r;
                        unlitColor = lerp(unlitColor, unlitColor * _MaskColor.rgb * _MaskBrightness, maskVal);
                    }

                    fixed3 ambientTint = _Color.rgb * sceneAmbient;
                    fixed3 blendedLight = lerp(ambientTint, fixed3(lightIntensity, lightIntensity, lightIntensity), blendFactor);
                    litColor += unlitColor * max(blendedLight - ambientTint, 0);
                }

                fixed4 litBase = fixed4(litColor, baseColor.a);
                fixed4 overlayColor = _Color1 * i.color;
                fixed4 overlay = fixed4(overlayColor.rgb * overlayColor.a * sceneAmbient, baseColor.a);
                fixed4 finalColor = lerp(litBase, overlay, _BlendFactor);

                finalColor.rgb = saturate(finalColor.rgb);
                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                return finalColor;
            }
            ENDCG
        }
    }

    FallBack "Unlit/Transparent"
}