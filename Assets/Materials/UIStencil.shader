Shader "Custom/windowTex"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [IntRange] _StencilID ("Stencil ID", Range(0, 255)) = 0
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "Queue"="Geometry+1"
            "RenderPipeline"="UniversalPipeline"
        }
        
        Pass
        {
            Blend Zero One
            ZWrite Off
            
            Stencil
            {
                Ref [_StencilID]
                Comp Always
                Pass Replace
                Fail Keep
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
                o.uv = 1 - TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // If the alpha value is less than a small threshold, discard the pixel
                // This will prevent it from writing to the stencil buffer.
                if (col.a < 0.01)
                    discard;

                return fixed4(0,0,0,0); // Actual color output is irrelevant, since ColorMask is 0.
            }
            ENDCG
        }
    }
}
