Shader "Legacy Shaders/Simplified/BaseSpriteRenderer"
{
    Properties
    {
        _Color("Main Color / Overlay Color", Color) = (1,1,1,1)
        [NoScaleOffset] [MainTexture] _MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
        [NoScaleOffset] _LightMap("Lightmap (Greyscale)", 2D) = "white" {}

        [Toggle] _UseLightmap("Enable Lightmap", Float) = 0
        [Toggle] _UseSmoothTransition("Use Smooth Transition", Float) = 1
        _TransitionThreshold("Lightmap Transition Threshold", Range(0.01, 1)) = 0.5

        [Toggle] _UseOverlay("Enable Overlay", Float) = 0
        _BlendFactor("Overlay Blend Factor", Range(0, 1)) = 0

        [Toggle] _UseBobbing("Enable Bobbing", Float) = 0
        _BobAmount("Bobbing Amount (XYZ)", Vector) = (0, 0, 0)
        _BobSpeed("Bobbing Speed (XYZ)", Vector) = (1, 1, 1)

        [Toggle] _UseGlitch("Enable Glitch", Float) = 0
        _GlitchValueX("Glitch Value X", Range(0, 256)) = 1
        _GlitchValueY("Glitch Value Y", Range(0, 256)) = 1
        _GlitchValueZ("Glitch Value Z", Range(0, 256)) = 1
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
        #pragma surface surf Lambert alpha:fade vertex:vert
        #pragma target 3.0

        #pragma shader_feature _USELIGHTMAP_ON
        #pragma shader_feature _USESMOOTHTRANSITION_ON
        #pragma shader_feature _USEOVERLAY_ON
        #pragma shader_feature _USEBOBBING_ON
        #pragma shader_feature _USEGLITCH_ON

        sampler2D _MainTex;
        sampler2D _LightMap;
        fixed4 _Color;

        float _TransitionThreshold;
        float3 _BobAmount;
        float3 _BobSpeed;
        
        int _GlitchValueX;
        int _GlitchValueY;
        int _GlitchValueZ;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_LightMap;
            fixed4 color : COLOR;
        };

        void vert(inout appdata_full v)
        {
            float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

            #if defined(_USEBOBBING_ON)
                if (any(_BobAmount != 0))
                {
                    half time = _Time.y;
                    float3 bobbingPosition = float3(sin(time * _BobSpeed.x), sin(time * _BobSpeed.y), sin(time * _BobSpeed.z)) * _BobAmount;
                    worldPos.xyz += bobbingPosition;
                }
            #endif

            v.vertex = mul(unity_WorldToObject, worldPos);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex);
            
            #ifdef _USEGLITCH_ON
                int gx = texColor.x * 256;
                gx = gx ^ _GlitchValueX;
                texColor.x = gx / 256.0;

                int gy = texColor.y * 256;
                gy = gy ^ _GlitchValueY;
                texColor.y = gy / 256.0;

                int gz = texColor.z * 256;
                gz = gz ^ _GlitchValueZ;
                texColor.z = gz / 256.0;
            #endif

            fixed4 baseColor = texColor * _Color * IN.color;

            #ifdef _USELIGHTMAP_ON
                fixed lightIntensity = tex2D(_LightMap, IN.uv_LightMap).r;
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;

                float blendFactor = 0;
                #ifdef _USESMOOTHTRANSITION_ON
                    blendFactor = smoothstep(0.0, _TransitionThreshold, lightIntensity);
                #else
                    blendFactor = step(_TransitionThreshold, lightIntensity);
                #endif

                fixed3 blendedLight = lerp(ambient, fixed3(lightIntensity, lightIntensity, lightIntensity), blendFactor);
                o.Emission = baseColor.rgb * max(blendedLight - ambient, 0);
            #else
                o.Emission = 0;
            #endif

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
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            
            #pragma shader_feature _USEOVERLAY_ON
            #pragma shader_feature _USELIGHTMAP_ON
            #pragma shader_feature _USESMOOTHTRANSITION_ON
            #pragma shader_feature _USEBOBBING_ON
            #pragma shader_feature _USEGLITCH_ON

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _LightMap;
            fixed4 _Color;

            float _BlendFactor;
            float _TransitionThreshold;
            float3 _BobAmount;
            float3 _BobSpeed;

            int _GlitchValueX;
            int _GlitchValueY;
            int _GlitchValueZ;

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
            };

            v2f vert(appdata_t v)
            {
                v2f o;

                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

                #if defined(_USEBOBBING_ON)
                    if (any(_BobAmount != 0))
                    {
                        half time = _Time.y;
                        float3 bobbingPosition = float3(sin(time * _BobSpeed.x), sin(time * _BobSpeed.y), sin(time * _BobSpeed.z) ) * _BobAmount;
                        worldPos.xyz += bobbingPosition;
                    }
                #endif

                o.pos = mul(UNITY_MATRIX_VP, worldPos);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                #ifndef _USEOVERLAY_ON
                    discard;
                #endif

                fixed4 texColor = tex2D(_MainTex, i.uv);

                #ifdef _USEGLITCH_ON
                    int gx = texColor.x * 256;
                    gx = gx ^ _GlitchValueX;
                    texColor.x = gx / 256.0;

                    int gy = texColor.y * 256;
                    gy = gy ^ _GlitchValueY;
                    texColor.y = gy / 256.0;

                    int gz = texColor.z * 256;
                    gz = gz ^ _GlitchValueZ;
                    texColor.z = gz / 256.0;
                #endif

                fixed4 baseColor = texColor * _Color * i.color;

                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;
                fixed3 litColor = baseColor.rgb * ambient;

                #ifdef _USELIGHTMAP_ON
                    fixed lightIntensity = tex2D(_LightMap, i.uv).r;
                    float blendFactor = 0;
                    #ifdef _USESMOOTHTRANSITION_ON
                        blendFactor = smoothstep(0.0, _TransitionThreshold, lightIntensity);
                    #else
                        blendFactor = step(_TransitionThreshold, lightIntensity);
                    #endif
                    fixed3 blendedLight = lerp(ambient, fixed3(lightIntensity, lightIntensity, lightIntensity), blendFactor);
                    litColor += baseColor.rgb * max(blendedLight - ambient, 0);
                #endif

                fixed4 litBase = fixed4(litColor, baseColor.a);
                fixed4 overlayColor = _Color * i.color;
                fixed4 overlay = fixed4(overlayColor.rgb * overlayColor.a, baseColor.a);
                fixed4 finalColor = lerp(litBase, overlay, _BlendFactor);
                finalColor.rgb = saturate(finalColor.rgb);

                return finalColor;
            }
            ENDCG
        }
    }

    FallBack "Unlit/Transparent"
}