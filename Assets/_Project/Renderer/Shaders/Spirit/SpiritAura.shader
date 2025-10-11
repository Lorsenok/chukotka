Shader "Unlit/SpiritAura"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Mask ("Mask Texture", 2D) = "white" {}
        _Centre ("Centre", Vector) = (0.5, 0.5, 0, 0)
        _Edge ("Edge", Float) = 0.05
        _Intro ("Intro", Vector) = (0, 0.05, 0, 0)
        _Outro ("Outro", Vector) = (0.1, 0.3, 0, 0)
        _CurAnimTime ("Animation", Float) = 0
        _CA ("Chromatic Aberration Intensity", Float) = 0.02
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

            sampler2D _Mask;

            float2 _Centre;
            float _Edge;

            float2 _Intro;
            float2 _Outro;
            float _CurAnimTime;

            float _CA;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float getOffsetStrength(float t, float2 dir)
            {
                float2 aspect = float2(1, _ScreenParams.x / _ScreenParams.y);
                float d = length(dir / aspect) - t; // SDF of a circle with radius being substracted from length
                
                //if distance > _second argument_ returns 1, otherwise returns 0
                //it makes an edge for sdf figure
                d *= 1 - smoothstep(0, _Edge, abs(d));

                //Any easing animations can be used here :P
                d *= smoothstep(_Intro.x, _Intro.y, t); //Smooth intro, for first 1% of an animation circle will slowly appear
                d *= 1 - smoothstep(_Outro.x, _Outro.y, t); //Smooth outro, same here

                return d;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 dir = _Centre - i.uv; //direction from the center of the effect to current pixel

                float rD = getOffsetStrength(_CurAnimTime + _CA, dir);
                float gD = getOffsetStrength(_CurAnimTime, dir);
                float bD = getOffsetStrength(_CurAnimTime - _CA, dir);

                float r = tex2D(_MainTex, i.uv + normalize(dir) * rD).r;
                float g = tex2D(_MainTex, i.uv + normalize(dir) * gD).g;
                float b = tex2D(_MainTex, i.uv + normalize(dir) * bD).b;
                
                return fixed4(r, g, b, tex2D(_Mask, i.uv).a);
            }
            ENDCG
        }
    }
}
