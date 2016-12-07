// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/SpaceExPlanetRenderer"
{
	Properties {
		 _ColorHot ("Hot Color", Color) = (0.82, 0.475, 0.29, 1.0)
         _ColorTemp ("Temp Color", Color) = (0.914, 0.294, 0.353, 1.0)
         _ColorCold ("Cold Color", Color) = (0.53, 0.6, 0.9, 1.0)
		 _NoiseText("Noise Texture", 2D) = "white" {}
	}
	SubShader {
		Tags {"LightMode"="ForwardBase"}
 
		Pass
		{ 
			CGPROGRAM
			#pragma vertex vert             
			#pragma fragment frag
			#include "UnityCG.cginc" // for UnityObjectToWorldNormal
            #include "UnityLightingCommon.cginc" // for _LightColor0
 
			struct vertInput
			{
				float4 pos : POSITION;
				float4 normal: NORMAL;
				fixed3 textcoord : TEXCOORD0;
			};  
 
			struct vertOutput
			{
				float4 pos : POSITION;
				float4 normal : NORMAL;
				float2 uv: TEXCOORD0;
				fixed4 diff : COLOR0;
			};
 
			vertOutput vert(vertInput input)
			{
				vertOutput o;

				UNITY_INITIALIZE_OUTPUT(vertOutput, o);

				
				o.pos = mul(UNITY_MATRIX_MVP, input.pos);
				o.uv = input.textcoord;
				o.normal = input.normal;
				
				/*half3 worldNormal = UnityObjectToWorldNormal(input.normal);
                // dot product between normal and light direction for
                // standard diffuse (Lambert) lighting
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                // factor in the light color
                o.diff = nl * _LightColor0;
				o.diff += 0.125; */
				return o;
			}
 
			//uniform float _Values[256];
			sampler2D _EnvTexture;
			sampler2D _NoiseText;
			uniform float4 _ColorCold;
			uniform float4 _ColorTemp;
			uniform float4 _ColorHot;
 
			half4 frag(vertOutput output) : COLOR
			{
				float h = tex2D(_EnvTexture, output.uv).r;
				float n = tex2D(_NoiseText, output.uv).r;
				
				half3 worldNormal = UnityObjectToWorldNormal(output.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				

				float4 diff = nl * _LightColor0;
				h += (0.6*n - 0.3);

				float4 c1 = lerp(_ColorTemp, _ColorHot,  (h - 0.5) * 2);
				float4 c2 = lerp(_ColorCold, _ColorTemp, (h * 2));

				
				if ( h >= 0.5)
					return saturate(c1 * diff);
				return saturate(c2 * diff);
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}