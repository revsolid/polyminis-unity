// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/EnvironmentLiquidShader"
{
	Properties {
		 _ColorHot ("Hot Color", Color) = (0.82, 0.475, 0.29, 1.0)
         _ColorTemp ("Temp Color", Color) = (0.914, 0.294, 0.353, 1.0)
         _ColorCold ("Cold Color", Color) = (0.53, 0.6, 0.9, 1.0)
	}
	SubShader {
		Tags {"Queue"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha // Alpha blend
 
		Pass
		{ 
			CGPROGRAM
			#pragma vertex vert             
			#pragma fragment frag
 
			struct vertInput
			{
				float4 pos : POSITION;
				fixed3 textcoord : TEXCOORD0;
			};  
 
			struct vertOutput
			{
				float4 pos : POSITION;
				fixed3 worldPos : TEXCOORD1;
				float2 uv: TEXCOORD0;
			};
 
			vertOutput vert(vertInput input)
			{
				vertOutput o;
				o.pos = mul(UNITY_MATRIX_MVP, input.pos);
				o.worldPos = input.pos.xyz;
				o.uv = input.textcoord;
				return o;
			}
 
			//uniform float _Values[256];
			sampler2D _TempTexture;
			uniform float4 _ColorCold;
			uniform float4 _ColorTemp;
			uniform float4 _ColorHot;
 
			half4 frag(vertOutput output) : COLOR
			{
				float h = tex2D(_TempTexture, output.uv).r;

				float4 c1 = lerp(_ColorTemp, _ColorHot,  (h - 0.5) * 2);
				float4 c2 = lerp(_ColorCold, _ColorTemp, (h * 2));
				
				if ( h >= 0.5)
					return c1;
				return c2;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}