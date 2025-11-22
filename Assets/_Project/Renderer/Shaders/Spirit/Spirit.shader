Shader "Unlit/Spirit"
{
    Properties
    {
        _MainTex ("Particle Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Distortion ("Distortion Power", Float) = 0.1
        _Octaves ("Octaves", Float) = 6
        _Animation ("Animation", Range(0, 1)) = 0
        _AnimationTiling ("Animation Tiling", Vector) = (1, 1, 0, 0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Color;
            float4 _MainTex_ST;

            float _Distortion;

            float _Octaves;

            float _Animation;
            float2 _AnimationTiling;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

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
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.uv.x += sin(i.pos.y * 6) * _Distortion;
                fixed4 col = tex2D(_MainTex, i.uv);
                float n = floor(fbm(i.uv) * 8) / 8;
                col *= float4(n, n, n, 1.0);
                
                float alpha = col.a * fbm(i.uv * _AnimationTiling);
                
                float alphaMain = 1 - smoothstep(alpha, 1, _Animation + 0.01); 
                alphaMain = floor(alphaMain);

                fixed4 mainCol = fixed4(col.rgb, alphaMain);
                
                return mainCol * alphaMain * i.color;
            }
            ENDCG
        }
    }
}
