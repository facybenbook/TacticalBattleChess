﻿

Shader "Custom/ContentShader" {
	Properties{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		//Mark
		[Toggle] _MarkActive("Mark Active",float) = 0
		_MarkOffSet("y offset",float) = 0.008

		//Hover
		[Toggle]	_OutlineActive("Range Active", float) = 0
		_Outline("size of Outline",Range(0,0.1)) = 0.02
		_OutlineColor("Color for Outline",color) = (1,1,1,1)
	}

	SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 100

		ZWrite Off
		Cull Off //Else sprites Vanish after Y 180 rotation!
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"

			struct appdata {
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f {
			float4 position : SV_POSITION;
			float2 uv : TEXCOORD0;
		};

		sampler2D _MainTex;
		float _MarkOffSet;
		bool _MarkActive;
		bool _OutlineActive;
		float _Outline;
		float4 _OutlineColor;
		//Vertex
		v2f vert(appdata v)
		{
			v2f o;
			o.position = UnityObjectToClipPos(v.vertex);
			if (_MarkActive) {
				o.position.y += _MarkOffSet;
			}
			o.uv = v.uv;
			return o;
		}
		//Fragment
		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 col = tex2D(_MainTex, i.uv);
			if (_OutlineActive) {

				if (col.a == 0) {
					if (col.b > 0 && col.b < _Outline) {
						return _OutlineColor;
					}
				}
			}
			return col;
		}
		ENDCG

	}
	}
}