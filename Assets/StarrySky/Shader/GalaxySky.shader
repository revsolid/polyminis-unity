Shader "Starry Sky/Galaxy Sky" {
	Properties {
		_RandomTex ("Random Tex", 2D) = "black" {}
		_Center ("Center", Vector) = (0.5, 0.5, 0, 0)
		_Distort ("Distort", Range(0, 1)) = 0
		_Range ("Range", Range(0, 5)) = 2
		_Power ("Power", Range(1, 32)) = 10
		_ColorTex ("Color Tex", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		uniform sampler2D _RandomTex, _ColorTex;
		uniform float4 _Center, _Color;
		uniform float _Distort, _Range, _Power;
       	float4 frag (v2f_img i) : SV_TARGET
       	{
			float dist = distance(_Center.xy, i.uv);
			float rnd = tex2D(_RandomTex, i.uv + _Distort.xx).r;
			float dense = (1 - dist * _Range) * pow(rnd, dist * _Power);
			
			float angle = atan(((1 - i.uv.y) - 0.5) / (i.uv.x - 0.5));
			angle = degrees(angle);
			angle += 270;
			
			if (dist > 0.15)
			{
				angle = fmod(angle, 360 * dist);
				dense = dense * angle;
				dense /= 25;
			}
			
			float3 color = tex2D(_ColorTex, i.uv + _Distort.xx).rgb * _Color.rgb;
			return float4(dense.xxx * color, dense);
       	}
	ENDCG
	SubShader {
		Tags { "RenderType" = "Transparent" "IgnoreProjector" = "True" "Queue" = "Transparent" }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			ENDCG
		}
	}
	FallBack Off
}