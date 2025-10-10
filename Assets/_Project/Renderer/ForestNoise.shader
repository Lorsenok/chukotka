Shader "Unlit/ForestNoise"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Octaves ("Octaves", Float) = 6
        _Pixelization ("Pixelization", Float) = 16
        _Simplification ("Simplification", FLoat) = 8
        _Direction ("Direction", Vector) = (1, 1, 0, 0)
        _Color ("Color", Color) = (1, 1, 1, 1)
        _TimeMultiplier ("Appear Speed", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Octaves;
            float _Pixelization;
            float _Simplification;
            float2 _Direction;
            float4 _Color;
            float _TimeMultiplier;

            float2 rand2D(float2 pos)
            {
                return float2(
                    frac(sin(dot(pos, float2(12.9898, 78.233))) * 43758.5453),
                    frac(sin(dot(pos, float2(39.3468, 11.135))) * 96321.9754)
                );
            }

            float noise(float2 uv)
            {
                float2 i = floor(uv); 
                float2 f = smoothstep(0.0, 1.0, frac(uv)); 

                float a = rand2D(i);
                float b = rand2D(i + float2(1.0, 0));
                float c = rand2D(i + float2(0, 1.0));
                float d = rand2D(i + float2(1.0, 1.0));

                return lerp(
                    lerp(a, b, f.x),
                    lerp(c, d, f.x),
                    f.y
                );
            }

            float fbm (float2 uv)
            {
                float col = 0.0;
                float amplitude = 0.5;

                for (float i = 0; i < _Octaves; i++)
                {
                    col += noise(uv) * amplitude;
                    uv *= 2;
                    amplitude *= 0.5;
                }
                return col;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.uv += _Time.y * _Direction;
                i.uv = floor(i.uv * _Pixelization) / _Pixelization;
                float noise = fbm( i.uv + _Time.y + fbm( i.uv + fbm( i.uv ) ) );
                noise = floor(noise * _Simplification) / _Simplification;
                fixed4 finalColor = float4(1, 1, 1, noise) * _Color;
                finalColor.a += _Color.a * sin(_Time.y * _TimeMultiplier);
                return finalColor;
            }
            ENDCG
        }
    }
}
