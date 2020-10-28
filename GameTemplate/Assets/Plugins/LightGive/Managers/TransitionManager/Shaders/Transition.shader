// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "LightGive/Unlit/TransitionShader"
{
	Properties
	{
		_MainTex ("Image (RGB)", 2D) = "white" {}
		_Gradation ("Gradation (A8)", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_Cutoff ("Alpha Cut Off", Range(0,1)) = 0.5
		[Toggle] _Invert ("Inverted (Boolean)", Float) = 0
	}
	
	SubShader
	{
		Tags
		{
			"Queue"="AlphaTest"
			"IgnoreProjector"="True"
			"RenderType"="TransparentCutout"
			"PreviewType"="Plane"
		}
		
		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				float4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
			};

			sampler2D _MainTex;
			sampler2D _Gradation;
			fixed4 _Color;
			fixed _Cutoff;
			fixed _Invert;

			v2f vert (appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				return OUT;
			}
			
			fixed4 frag (v2f IN) : SV_Target
			{
				fixed4 col = tex2D(_Gradation, IN.texcoord) -0.01;
				if(_Invert == 0) col = 1 - col;
				fixed4 tex = tex2D(_MainTex, IN.texcoord) * IN.color;
				clip(_Cutoff - col.a);
				return tex;
			}
			
			ENDCG
		}
	}
}
