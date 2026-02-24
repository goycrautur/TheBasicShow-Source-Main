Shader "Legacy Shaders/Simplified/BaseMaterial"
{
    Properties
    {
        _Color0 ("Main Color", Color) = (1,1,1,1)
        _Color1 ("Secondary Color", Color) = (1,1,1,1)
        [MainTexture] _MainTex ("Base (RGB)", 2D) = "white" {}
        _SecondTex ("Secondary (RGB)", 2D) = "white" {}
        _SecondaryDiffrent ("Secondary ALT (RGB)", 2D) = "white" {}
        _Mask ("Mask", 2D) = "black" {}
        _LightMap ("Lightmap (Greyscale)", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
        _TransitionThreshold ("Lightmap Transition Threshold", Range(0.01, 0.2)) = 0.05
        _ValueX("Color Glitch X", Range(0,256)) = 0
        _ValueY("Color Glitch Y", Range(0,256)) = 0
        _ValueZ("Color Glitch Z", Range(0,256)) = 0
        _VertexGlitchSeed("Vertex Glitch Seed", Float) = 4
        _VertexGlitchIntensity("Vertex Glitch Intensity", Range(-2,2)) = 0

        [ToggleUI] _Swap ("Switch Secondary", Int) = 0
        [ToggleUI] _UseMask ("Use Mask", Int) = 0
        [ToggleUI] _UseSecondTex ("Use Secondary Tex", Int) = 0
        [ToggleUI] _UseTransparency ("Use Transparency", Int) = 0
        [ToggleUI] _UseLightmap ("Use Lightmap", Int) = 0
        [ToggleUI] _UseSmoothTransition ("Use Smooth Transition", Int) = 1
        [ToggleUI] _UseGlitch ("Use Glitch", Int) = 0
    }

    CustomEditor "MaterialGUI"

    SubShader
    {
        Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alphatest:_Cutoff fullforwardshadows vertex:vert
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _SecondTex;
        sampler2D _SecondaryDiffrent;
        sampler2D _Mask;
        sampler2D _LightMap;

        fixed4 _Color0;
        fixed4 _Color1;

        int _ValueX;
        int _ValueY;
        int _ValueZ;

        float _Swap;
        float _UseMask;
        float _UseSecondTex;
        float _UseTransparency;
        float _UseLightmap;
        float _UseSmoothTransition;
        float _TransitionThreshold;
        float _UseGlitch;
        float _VertexGlitchSeed;
        float _VertexGlitchIntensity;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_LightMap : TEXCOORD1;
            float3 worldNormal;
        };

        float hash(float n) { return frac(sin(n) * 43758.5453); }

        float noise3D(float3 x)
        {
            float3 p = floor(x);
            float3 f = frac(x);
            f = f * f * (3.0 - 2.0 * f);
            float n = p.x + p.y * 57.0 + 113.0 * p.z;
            return lerp(lerp(lerp(hash(n + 0.0), hash(n + 1.0), f.x), lerp(hash(n + 57.0), hash(n + 58.0), f.x), f.y), lerp(lerp(hash(n + 113.0), hash(n + 114.0), f.x), lerp(hash(n + 170.0), hash(n + 171.0), f.x), f.y), f.z);
        }

        void vert(inout appdata_full v)
        {
            if (_UseGlitch > 0.5)
            {
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Static noise — not time-based
                float n = noise3D(worldPos * 2.0 + _VertexGlitchSeed * 10.0);
                n = round(n * 3.0) / 3.0;
                n = saturate((n - 0.5) * 2.0 + 0.5);

                v.vertex.xyz += v.normal * n * _VertexGlitchIntensity * 0.5;
            }
        }

        fixed3 ApplyGlitch(fixed3 colIn, int vX, int vY, int vZ)
        {
            int r = colIn.r * 256;
            r = r ^ vX;
            colIn.r = r / 256.0;

            int g = colIn.g * 256;
            g = g ^ vY;
            colIn.g = g / 256.0;

            int b = colIn.b * 256;
            b = b ^ vZ;
            colIn.b = b / 256.0;
            
            return colIn;
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 baseColor = tex2D(_MainTex, IN.uv_MainTex) * _Color0;

            if (_UseGlitch > 0.5)
            {
                baseColor.rgb = ApplyGlitch(baseColor.rgb, _ValueX, _ValueY, _ValueZ);
            }

            o.Albedo = baseColor.rgb;
            o.Alpha = 1.0;
            o.Emission = fixed3(0, 0, 0);

            if (_UseSecondTex > 0.5 || _UseLightmap > 0.5 || _UseTransparency > 0.5)
            {
                fixed4 wall = baseColor;
                fixed4 door = fixed4(0, 0, 0, 0);

                if (_UseSecondTex > 0.5)
                {
                    if (_Swap > 0.5)
                    {
                        door = tex2D(_SecondaryDiffrent, IN.uv_MainTex) * _Color1;
                    }
                    else
                    {
                        door = tex2D(_SecondTex, IN.uv_MainTex) * _Color1;
                    }

                    if (_UseGlitch > 0.5)
                    {
                        door.rgb = ApplyGlitch(door.rgb, _ValueX, _ValueY, _ValueZ);
                    }
                }

                fixed4 mask = (_UseMask > 0.5) ? tex2D(_Mask, IN.uv_MainTex) : fixed4(1,1,1,1);
                fixed3 finalColor = lerp(wall.rgb, door.rgb, (_UseSecondTex > 0.5) ? door.a : 0);
                o.Albedo = finalColor;

                if (_UseLightmap > 0.5)
                {
                    fixed4 lightmap = tex2D(_LightMap, IN.uv_LightMap);
                    fixed3 ambient = ShadeSH9(float4(normalize(IN.worldNormal), 1.0));

                    fixed3 diffFromWhite = 1.0 - ambient.rgb;
                    float maxDiff = max(max(diffFromWhite.r, diffFromWhite.g), diffFromWhite.b);
                    float blendFactor = (_UseSmoothTransition > 0.5) ? smoothstep(0.0, _TransitionThreshold, maxDiff) : step(_TransitionThreshold, maxDiff);

                    o.Emission = finalColor * lightmap.rgb * blendFactor;
                }

                if (_UseTransparency > 0.5)
                {
                    o.Alpha = wall.a * mask.a * mask.rgb + door.a * (1 - mask.a);
                }
                else
                {
                    o.Alpha = 1.0;
                }
            }
        }
        ENDCG
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert fullforwardshadows vertex:vert
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _SecondTex;
        sampler2D _SecondaryDiffrent;
        sampler2D _Mask;
        sampler2D _LightMap;

        fixed4 _Color0;
        fixed4 _Color1;

        int _ValueX;
        int _ValueY;
        int _ValueZ;

        float _Swap;
        float _UseMask;
        float _UseSecondTex;
        float _UseTransparency;
        float _UseLightmap;
        float _UseSmoothTransition;
        float _TransitionThreshold;
        float _UseGlitch;
        float _VertexGlitchSeed;
        float _VertexGlitchIntensity;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_LightMap : TEXCOORD1;
            float3 worldNormal;
        };

        float hash(float n) { return frac(sin(n) * 43758.5453); }

        float noise3D(float3 x)
        {
            float3 p = floor(x);
            float3 f = frac(x);
            f = f * f * (3.0 - 2.0 * f);
            float n = p.x + p.y * 57.0 + 113.0 * p.z;
            return lerp(lerp(lerp(hash(n + 0.0), hash(n + 1.0), f.x), lerp(hash(n + 57.0), hash(n + 58.0), f.x), f.y), lerp(lerp(hash(n + 113.0), hash(n + 114.0), f.x), lerp(hash(n + 170.0), hash(n + 171.0), f.x), f.y), f.z);
        }

        void vert(inout appdata_full v)
        {
            if (_UseGlitch > 0.5)
            {
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                float n = noise3D(worldPos * 2.0 + _VertexGlitchSeed * 10.0);
                n = round(n * 3.0) / 3.0;
                n = saturate((n - 0.5) * 2.0 + 0.5);

                v.vertex.xyz += v.normal * n * _VertexGlitchIntensity * 0.5;
            }
        }

        fixed3 ApplyGlitch(fixed3 colIn, int vX, int vY, int vZ)
        {
            int r = colIn.r * 256;
            r = r ^ vX;
            colIn.r = r / 256.0;

            int g = colIn.g * 256;
            g = g ^ vY;
            colIn.g = g / 256.0;

            int b = colIn.b * 256;
            b = b ^ vZ;
            colIn.b = b / 256.0;
            
            return colIn;
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 baseColor = tex2D(_MainTex, IN.uv_MainTex) * _Color0;

            if (_UseGlitch > 0.5)
            {
                baseColor.rgb = ApplyGlitch(baseColor.rgb, _ValueX, _ValueY, _ValueZ);
            }

            o.Albedo = baseColor.rgb;
            o.Alpha = 1.0;
            o.Emission = fixed3(0, 0, 0);

            if (_UseSecondTex > 0.5 || _UseLightmap > 0.5)
            {
                fixed4 wall = baseColor;
                fixed4 door = fixed4(0, 0, 0, 0);

                if (_UseSecondTex > 0.5)
                {
                    if (_Swap > 0.5)
                    {
                        door = tex2D(_SecondaryDiffrent, IN.uv_MainTex) * _Color1;
                    }
                    else
                    {
                        door = tex2D(_SecondTex, IN.uv_MainTex) * _Color1;
                    }

                    if (_UseGlitch > 0.5)
                    {
                        door.rgb = ApplyGlitch(door.rgb, _ValueX, _ValueY, _ValueZ);
                    }
                }

                fixed3 finalColor = lerp(wall.rgb, door.rgb, (_UseSecondTex > 0.5) ? door.a : 0);
                o.Albedo = finalColor;

                if (_UseLightmap > 0.5)
                {
                    fixed4 lightmap = tex2D(_LightMap, IN.uv_LightMap);
                    fixed3 ambient = ShadeSH9(float4(normalize(IN.worldNormal), 1.0));

                    fixed3 diffFromWhite = 1.0 - ambient.rgb;
                    float maxDiff = max(max(diffFromWhite.r, diffFromWhite.g), diffFromWhite.b);
                    float blendFactor = (_UseSmoothTransition > 0.5) ? smoothstep(0.0, _TransitionThreshold, maxDiff) : step(_TransitionThreshold, maxDiff);

                    o.Emission = finalColor * lightmap.rgb * blendFactor;
                }
            }
        }
        ENDCG
    }

    FallBack "Diffuse"
}