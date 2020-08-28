Shader "Custom/Halftone" {
	Properties
	{
		Scale ("Scale", Range(1.0, 10.0)) = 3.0
		Yrot ("Y Rot", Range(1.0, 1.8)) = 1.0
		Frequency ("Frequency", Range(1.0, 100.0)) = 40.0
		//MainColor ("Main Color", Color) = (0, 0.5, 0, 1)
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
			float Scale; // For imperfect, isotropic anti-aliasing in
			float Yrot;  // absence of dFdx() and dFdy() functions
			float Frequency; //
			//float4 MainColor;
			
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float level : TEXCOORD1;
				float4 color : TEXCOORD2;
			};
			
			
			
			//VERTEX PROGRAM
			vertexOutput vp(appdata_full IN)
			{
				vertexOutput OUT;
				
				OUT.pos = mul(UNITY_MATRIX_MVP, IN.vertex); 
				OUT.uv = IN.texcoord;
				OUT.color = IN.color;
				
				//Obj no mundo
				float4 worldPos = mul(_Object2World, IN.vertex);
				float3 worldNormal = mul(_Object2World, float4(IN.normal, 0.0f)).xyz;
								
				//normal vector
				float3 N = normalize(worldNormal);
				//light vector
				float3 L = normalize(_WorldSpaceLightPos0).xyz;
				//diffuse
				float NdotL = dot(N, L);
				NdotL = (NdotL * 0.5) + 0.5; 
				//Level de toon
				OUT.level = NdotL; 
			
				return OUT;
			}
			
			
			float aastep(float threshold, float value) 
			{
			  float afwidth = Frequency * (1.0/200.0) / Scale / cos(Yrot);
			  return smoothstep(threshold - afwidth, threshold + afwidth, value);
			}
			
			//FRAGMENT PROGRAM
			half4 fp(vertexOutput IN) : COLOR
			{
				half4 result; 				
				//Halftone
				float2x2 m;
			    m[0].x = 0.707f; 
			    m[0].y = -0.707f;
			    m[1].x =  0.707f;
			    m[1].y = 0.707f;
			    
			    float2 st2 = mul(m, IN.uv);
			    float2 nearest = 2.0f * frac(Frequency * st2) - 1.0f;
			    float dist = length(nearest);

			    float radius = sqrt(1.0f - IN.level); // Use green channel
			    float3 white = float3(1.0f, 1.0f, 1.0f);
			    float3 black = float3(0.3f, 0.3f, 0.3f);
			    float3 fragcolor = lerp(black, white, aastep(radius, dist));
			    			
				return IN.color * float4(fragcolor, 1.0);
			}
			
			ENDCG
		}		
	} 
	FallBack "VertexLit"
}
