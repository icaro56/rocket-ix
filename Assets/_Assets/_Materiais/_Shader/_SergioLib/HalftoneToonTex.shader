Shader "Custom/HalftoneToonTex" {
	Properties
	{
		Thickness ("Thickness", Range(0.0, 1.0)) = 0.3
		Scale ("Scale", Range(1.0, 10.0)) = 3.0
		Yrot ("Y Rot", Range(1.0, 1.8)) = 1.0
		Frequency ("Frequency", Range(1.0, 100.0)) = 40.0
		RGBBase ("RGB (Base)", 2D) = "white" {}	
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
			float Scale; // For imperfect, isotropic anti-aliasing in
			float Yrot;  // absence of dFdx() and dFdy() functions
			float Frequency; //
			sampler2D RGBBase;
			int Levels;
						
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float level : TEXCOORD1;
				float edge : TEXCOORD2;
			};
			
			float RGBBase_ST;
			
			//VERTEX PROGRAM
			vertexOutput vp(appdata_base IN)
			{
				vertexOutput OUT;
				
				OUT.pos = mul(UNITY_MATRIX_MVP, IN.vertex); 
				OUT.uv = IN.texcoord;
				
				//Obj no mundo
				float4 worldPos = mul(_Object2World, IN.vertex);
				float3 worldNormal = mul(_Object2World, float4(IN.normal, 0.0f)).xyz;
				
				
				//normal vector
				float3 N = normalize(worldNormal);
				//light vector
				float3 L = normalize(_WorldSpaceLightPos0).xyz;
				//Toon level
				float NdotL = dot(N, L);
				NdotL = (NdotL*0.5) + 0.5; 
				OUT.level = NdotL; 
								
				//eyePosition - a.k.a View Vector
				float3 E = normalize(_WorldSpaceCameraPos - worldPos).xyz;
				//light + view
				float3 H = normalize(L + E);
				
												
				//Edge detection, dot eye and normal vectors
				OUT.edge = saturate(dot(N,E));
			
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
				half4 result = half4(0.0f, 0.0f, 0.0f, 1.0f); 				
				if(IN.edge > Thickness)
				{
					float4 MainColor = tex2D(RGBBase, IN.uv);
					
					//calculando as faixas	
					float level = IN.level * Levels;
					level -= frac(level);
					level += 0.5f;
					level /= Levels;
					level = clamp(level, 0.2f, 0.8f);
										
					// Distance to nearest point in a grid of
				    // (frequency x frequency) points over the unit square
				    float2x2 m;
				    
				    m[0].x = 0.707f; //(0.707f, -0.707f, 0.707f, 0.707f);
				    m[0].y = -0.707f;
				    m[1].x =  0.707f;
				    m[1].y = 0.707f;
				    
				    float2 st2 = mul(m, IN.uv);
				    float2 nearest = 2.0f * frac(Frequency * st2) - 1.0f;
				    float dist = length(nearest);
				    
				    // Use a texture to modulate the size of the dots
				    float radius = sqrt(1.0f - IN.level); // Use green channel
				    float3 white = float3(1.0f, 1.0f, 1.0f);
				    float3 black = float3(0.3f, 0.3f, 0.3f);
				    float3 fragcolor = lerp(black, white, aastep(radius, dist));
				    
				    result = MainColor * level * float4(fragcolor, 1.0);
			    }		
				return result;
			}
			
			ENDCG
		}		
	} 
	FallBack "VertexLit"
}
