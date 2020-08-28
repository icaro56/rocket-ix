Shader "Custom/CartoonColor" {
	Properties
	{
		Thickness ("Thickness", Range(0.0, 1.0)) = 0.3
		MainColor ("Diffuse Color", Color) = (0, 0.5, 0, 1)
		Levels ("Levels", Range(1, 10)) = 3
	}
	SubShader 
	{
		Pass 
		{
			Name "Pass0"
			
			CGPROGRAM
			#pragma vertex vp
			#pragma fragment fp
			#include "UnityCG.cginc"
			
			float Thickness;
			float4 MainColor;
			int Levels;
						
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float level : TEXCOORD0;
				float edge : TEXCOORD1;
			};
			
			//VERTEX PROGRAM
			vertexOutput vp(appdata_base IN)
			{
				vertexOutput OUT;
				
				OUT.pos = mul(UNITY_MATRIX_MVP, IN.vertex);
				
				//Obj no mundo
				float4 worldPos = mul(_Object2World, IN.vertex);
				float3 worldNormal = mul(_Object2World, float4(IN.normal, 0.0f)).xyz;
								
				//normal vector
				float3 N = normalize(worldNormal);
				//light vector
				float3 L = normalize(_WorldSpaceLightPos0).xyz;
				//Normal dot Light
				float NdotL = dot(N, L);
				NdotL = (NdotL*0.5) + 0.5; 
				OUT.level = NdotL; 
								
				//eyePosition - a.k.a View Vector
				float3 E = normalize(_WorldSpaceCameraPos.xyz - worldPos);
				//Edge detection, dot eye and normal vectors
				OUT.edge = saturate(dot(N,E));
			
				return OUT;
			}
			 
			
			//FRAGMENT PROGRAM
			half4 fp(vertexOutput IN) : COLOR
			{
				half4 result = half4(0.0f, 0.0f, 0.0f, 1.0f); 	
				//Se calculamos se não for edge
				if(IN.edge > Thickness)
				{
					//calculando as faixas	
					float level = IN.level * Levels;
					level -= frac(level);
					level += 0.5f;
					level /= Levels;
					level = clamp(level, 0.2f, 0.8f);
					
					result = MainColor * level;
				}
				return result;
			}
			
			ENDCG
		}		
	} 
	FallBack "VertexLit"
}
