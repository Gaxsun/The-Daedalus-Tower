Shader "Custom/FadeProxim" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
	}

		SubShader{
			Tags { "RenderType" = "Transparent" "Queue"="Transparent"}
			LOD 200
		Pass{
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha
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
		float alpha : DISTANCE;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.alpha = length(ObjSpaceViewDir(v.vertex));
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		return o;
	}

	float4 _Color;
	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = tex2D(_MainTex, i.uv) * _Color;
		col.a = saturate(i.alpha / 2.5f);
		return col;
	}
		ENDCG
	}
			CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard alpha //finalcolor:mycolor

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		
		struct Input {
			float2 uv_MainTex;
			fixed4 color : COLOR;
		};

		/*
		void mycolor(Input IN, SurfaceOutputStandard o, inout fixed4 color)
		{
			color;
		}
		*/

		sampler2D _MainTex;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
		}
		ENDCG
	}


		FallBack "Diffuse/Transparent"
}
