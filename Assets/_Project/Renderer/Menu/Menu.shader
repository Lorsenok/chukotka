Shader "Unlit/Menu"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Speed ("Speed", Float) = 1
        _Wavy ("Wavy Effect", Float) = 0.01
        _CA ("Chromatic Aberration", Float) = 0.005
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

            fixed4 _Color;
            float _CA;
            
            float _Speed;
            float _Wavy;

            float rand2D(float2 pos)
            {
                //dot(vec1.xy, vec2.xy) = vec1.x * vec2.x + vec1.y * vec2.y
                return frac(sin(dot(pos,
                    float2(12.9898,78.233))) * 43758.5453123);
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
                i.uv.xy += sin(i.uv.yx * 12.56 + _Time.y * 1) * _Wavy;
                
                float parts = 100;

                float speedSet = round(i.uv.y / (1 / parts)) + parts;
                float speed = abs(sin(speedSet)) * 1000;
                float dir = sign(sin(speedSet));
                speed = pow(speed, 0.5);
                
                float colr = round(rand2D(
                floor((i.uv.x + _CA) * 64 + _Time.y * speed * _Speed * dir) / 64
                ));
                float colg = round(rand2D(
                floor(i.uv.x * 64 + _Time.y * speed * _Speed * dir) / 64
                ));
                float colb = round(rand2D(
                floor((i.uv.x - _CA) * 64 + _Time.y * speed * _Speed * dir) / 64
                ));

                fixed3 col = float3(colr, colg, colb);
                
                if (speedSet % 2 == 0) col = 1;
                
                return (1 - fixed4(col, 1)) * _Color;
            }
            ENDCG
        }
    }
}
