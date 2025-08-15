Shader "UI/CircleWipePixel"
{
    Properties
    {
        _Color    ("Overlay Color", Color) = (0,0,0,0.8)     // 바깥쪽 어둡게 할 색
        _Center   ("Center (Rect 0-1)", Vector) = (0.5,0.5,0,0) // 중심 (패널 Rect 기준 0~1)
        _Radius   ("Radius (norm by H)", Float) = 1.5         // 반경(패널 세로 픽셀로 정규화)
        _Feather  ("Feather (norm by H)", Float) = 0.2        // 페더(부드러움)
        _RectSize ("Rect Size (px)", Vector) = (1080,2160,0,0) // x=width, y=height
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "CanUseSpriㄴteAtlas"="True" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float4 _Center;
            float  _Radius;
            float  _Feather;
            float4 _RectSize; // (w,h)

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv     : TEXCOORD0;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; // Image가 꽉 채운 Rect 기준 0~1 UV
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Rect 기준 0~1 좌표계
                float2 uv = i.uv;
                float2 c  = _Center.xy;

                // 픽셀 단위로 맞춘 후 길이(세로 픽셀로 정규화)
                float2 p_px = float2((uv.x - c.x) * _RectSize.x,
                                     (uv.y - c.y) * _RectSize.y);
                float d = length(p_px) / max(_RectSize.y, 1.0);

                // 원 바깥을 어둡게: d > _Radius 일수록 불투명
                float edge = smoothstep(_Radius - _Feather, _Radius, d);

                fixed4 col = _Color;
                col.a *= edge;
                return col;
            }
            ENDCG
        }
    }
}
