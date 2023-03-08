// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "unlit/multiply2D"
{
Properties
{
    _MainTex ("Texture", 2D) = "white" {}
}
SubShader
{
    Tags { "RenderType"="Opaque"
    "Queue" = "Geometry" }

    Pass
    {
        //AlphaTest Greater 0.5 //Only render pixels whose alpha is greater than AlphaValue.
        Stencil{
        Ref 2
        Comp Always//Never - never pass the stencil test, Always - always pass the stencil test
        Pass Replace //write Ref value into the stencil buufer
        }

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

        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            return o;
        }

        fixed4 frag (v2f i) : SV_Target
        {
            // sample the texture
            fixed4 col = tex2D(_MainTex, i.uv);
            //if (col.a < 0.9) discard; //discard a pixel if it's transparent and don't draw it into a buffer
            return col;
        }
        ENDCG
    }
}
}