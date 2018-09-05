﻿Shader "Custom/FadeProxim" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_godPowerActive("godBool", Int) = 0
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
		Pass
	{
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM

#pragma vertex VS_NormalMapping 
#pragma fragment PS_NormalMapping
#pragma target 3.0

#include "UnityCG.cginc"

		// vertex shader input that allows position and texture coordinates as well as TBN
	struct VSInput
	{
		float4 pos: POSITION;
		float4 tang: TANGENT;
		float3 nor: NORMAL;
		float2 tex: TEXCOORD0;
	};

	// vertex shader output structure
	struct VSOutput
	{
		float4 pos: SV_POSITION;
		float4 col: COLOR;
		//float3 worldPos;
	};

	int _godPowerActive;
	sampler2D _MainTex;

	// normal mapping vertex shader
	VSOutput VS_NormalMapping(VSInput a_Input)
	{
		VSOutput output;
		float3 worldPos = mul(unity_ObjectToWorld, a_Input.pos);
		// calculate homogenous position
		float4 color = float4(0.5 + sin(10* worldPos.x) / 2, 1, 1, 1);
		//float4 color = float4(0.5 + sin(10 * (sqrt(worldPos.x * worldPos.x + worldPos.z * worldPos.z))), 0.5 + sin(10 * sqrt(worldPos.x * worldPos.x + worldPos.z * worldPos.z)), 0.5 + sin(10 * sqrt(worldPos.x * worldPos.x + worldPos.z * worldPos.z)), 1);
		output.col = color;
		output.col.a = _godPowerActive * (1 + (_godPowerActive * (2 * sin(_Time.y * 10) / 2.2) * 0.5 + sin(30 * sqrt(worldPos.x * worldPos.x + worldPos.z * worldPos.z))));
		output.pos = UnityObjectToClipPos(a_Input.pos);

		return output;
	}

	// normal mapping pixel shader
	float4 PS_NormalMapping(VSOutput a_Input) : COLOR
	{
		// index into textures
		//float4 colour = a_Input.col * float4(1, 1, 1, _godPowerActive * (2 * sin(_Time.y * 5)/2.2));
		float4 colour = a_Input.col;
		// return texture colour modified by diffuse component
		return colour;
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
