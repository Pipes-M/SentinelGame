Shader "Unlit/LidarShader"
{
    Properties
    {
        _PointSize("Point Size", Float) = 5.0
        _Color("Point Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            StructuredBuffer<float3> _Points;
            float4 _Color;
            float _PointSize;

            struct appdata
            {
                uint vertexID : SV_VertexID;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 col : COLOR;
                float pointSize : PSIZE;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 worldPos = _Points[v.vertexID];
                o.pos = UnityObjectToClipPos(float4(worldPos,1));
                o.col = _Color;
                o.pointSize = _PointSize;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.col;
            }
            ENDCG
        }
    }
}
