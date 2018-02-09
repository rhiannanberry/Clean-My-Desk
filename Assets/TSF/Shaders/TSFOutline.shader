// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TSF/BaseOutline1"
{
    Properties 
    {

		[MaterialToggle(_TEX_ON)] _DetailTex ("Enable Detail texture", Float) = 0 	//1
		_MainTex ("Detail", 2D) = "white" {}        								//2
		_ToonShade ("Shade", 2D) = "white" {}  										//3
		[MaterialToggle(_COLOR_ON)] _TintColor ("Enable Color Tint", Float) = 0 	//4
		_Color ("Base Color", Color) = (1,1,1,1)									//5	
		[MaterialToggle(_VCOLOR_ON)] _VertexColor ("Enable Vertex Color", Float) = 0//6        
		_Brightness ("Brightness 1 = neutral", Float) = 1.0							//7	
		_OutlineColor ("Outline Color", Color) = (0.5,0.5,0.5,1.0)					//10
		_Outline ("Outline width", Float) = 0.01									//11
		_OutlineDamp ("Outline Damp", Float) = 0.01
		_OutlineNoise ("Outline noise", 2D) = "white" {}

    }
 
    SubShader
    {
        Tags { "RenderType"="Opaque" }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 250 
        Lighting Off
        Fog { Mode Off }
        
        UsePass "TSF/Base1/BASE"
        	
        Pass
        {
            Cull Front
            ZWrite On
            CGPROGRAM
			#include "UnityCG.cginc"
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma glsl_no_auto_normalization
            #pragma vertex vert
 			#pragma fragment frag
			
            struct appdata_t 
            {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 uv : TEXCOORD0;
			};

			struct v2f 
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
		
			};
			float _OutlineDamp;
            fixed _Outline;
			sampler2D _OutlineNoise;
            
            v2f vert (appdata_t v) 
            {
                v2f o;
			    o.pos = v.vertex;
				o.uv = v.uv;
				float3 affectedVerts = normalize(v.normal.xyz);
				float sinoffset = sin(dot(affectedVerts, float3(1, 1, 1) )* 7 % 11);
				float offset = tex2Dlod(_OutlineNoise, float4(o.pos.xy, 0, 0)).r;
				//also i might change this again bc i want contour lines to render ON TOP of model
				//the sketchy edge w straigt line looks bad
				//adjust this to normalize width between models
				o.pos.xyz += affectedVerts *(_Outline)*0.01 + sinoffset*_OutlineDamp*affectedVerts;
			    o.pos = UnityObjectToClipPos(o.pos);
			    return o;
            }
            
            fixed4 _OutlineColor;
            
            fixed4 frag(v2f i) : SV_Target//:COLOR 
			{
				float offset = tex2D(_OutlineNoise, i.uv).a;
				_OutlineColor.a = saturate(offset+0.1);
		    	return _OutlineColor;
			}
            
            ENDCG
        }
    }
Fallback "Legacy Shaders/Diffuse"
}