Shader "Unlit/UnlitMask"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_AlphaTex("Alpha Mask", 2D) = "white" {}
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100

		zwrite off
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			
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
			sampler2D _AlphaTex;
			
			float4 _MainTex_ST;
			float4 _AlphaTex_ST;
			
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
				fixed4 alpha = tex2D(_AlphaTex, i.uv);
				col.a = alpha.a;
				return col;
			}
			ENDCG
		}
	}
}
