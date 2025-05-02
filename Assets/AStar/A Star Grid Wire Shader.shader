Shader "Unlit/A Star Grid Wire Shader"
{
    Properties
    {
        _Color("Wire Color", Color) = (0, 0, 0, 1)
        _LineThickness("Thickness (World units)", Range(0.001, 0.5)) = 0.05
        _TileSize("Tile Size", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float _LineThickness;
            float _TileSize;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 pos = i.worldPos;

                // 타일 내부에서의 상대 좌표 (0~1)
                float2 localPos = fmod(pos.xz, _TileSize) / _TileSize;
                localPos = abs(localPos - 0.5); // 중심 기준

                float edgeDist = min(localPos.x, localPos.y);

                float wireLine = smoothstep(_LineThickness * 0.5, _LineThickness * 0.5 + 0.005, edgeDist);

                return _Color * (1.0 - wireLine);
            }
            ENDCG
        }
    }
}
