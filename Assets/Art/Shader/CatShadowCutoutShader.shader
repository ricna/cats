Shader "Custom/CatShadowCutoutShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Sample the main texture
                half4 texColor = tex2D(_MainTex, i.uv);

                // Fetch light occlusion information (shadow)
                half shadowValue = tex2D(_CameraOpaqueTexture, i.screenPos.xy / i.screenPos.w).r;

                // If shadowValue is > 0, that means light is hitting, so render normally
                // If shadowValue is <= 0, that means it's in shadow, so we discard the pixel
                if (shadowValue <= 0)
                {
                    discard; // Discard the pixel (makes it invisible in shadow)
                }

                return texColor;
            }
            ENDCG
        }
    }
}
