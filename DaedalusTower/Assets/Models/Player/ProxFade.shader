Shader "Custom/ProxFade" {
		Properties
		{
			_Color("Color", Color) = (1,1,1,1)
			_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
			_Metallic("Metallic", Range(0,1)) = 0.0
		}
			SubShader
		{
			Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
			LOD 200

			CGPROGRAM
#pragma vertex vert
#pragma surface surf Standard fullforwardshadows alpha:fade
#pragma target 3.0

#include "UnityCG.cginc"
			sampler2D _MainTex;
		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			float4 vertex : SV_POSITION;
			float alpha : ALPHA;
		};
		struct Input {
			float2 uv_MainTex;
			v2f alpha;
		};


		

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		v2f vert(inout appdata v)
		{
			v2f o;
			o.alpha = length(ObjSpaceViewDir(v.vertex));
			return o;
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
		}
			FallBack "Standard"
	}