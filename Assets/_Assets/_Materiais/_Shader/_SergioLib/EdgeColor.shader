Shader "Custom/EdgeColor" {
	Properties
	{
		Slope ("Slope", Range(0.0, 1.0)) = 0.3
		MainColor ("Main Color", Color) = (1.0, 1.0, 0.5, 1.0)
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
			
			float4 MainColor;
			float Slope; 
			
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float3 N : TEXCOORD0;
				float3 V : TEXCOORD1;
			};
			
			float4 MainTex_ST;
			
			//VERTEX PROGRAM
			vertexOutput vp(appdata_base IN)
			{
				vertexOutput OUT;
				OUT.N = mul(_Object2World, float4(IN.normal, 0)).xyz;
				float4 worldPosition = mul(_Object2World, IN.vertex);
				OUT.V = (_WorldSpaceCameraPos - worldPosition.xyz);
				OUT.pos = mul(UNITY_MATRIX_MVP, IN.vertex); 
				return OUT;
			}
			
			
			//FRAGMENT PROGRAM
			half4 fp(vertexOutput IN) : COLOR
			{
				float3 Ln = normalize(IN.V);
				float3 Nn = normalize(IN.N);
				float intensity = saturate(dot(Ln, Nn));
				if(intensity < Slope)
					intensity = 0;
				return MainColor * intensity;
			}
			
			ENDCG
		}
	} 
	FallBack "VertexLit"
}
