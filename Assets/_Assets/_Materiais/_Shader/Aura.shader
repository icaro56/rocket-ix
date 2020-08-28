Shader "Rocket/Aura" {
	Properties
	{
		MainColor ("Main Color", Color) = (1.0, 1.0, 0.5, 1.0)
	}
	SubShader 
	{
		
		Pass 
		{
			Name "Pass0"
			Blend SrcAlpha OneMinusSrcAlpha  
			
			CGPROGRAM
			#pragma vertex vp
			#pragma fragment fp
			#include "UnityCG.cginc"
			
			float4 MainColor;
			
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
				OUT.V = float3(0,1,0);//(_WorldSpaceCameraPos - worldPosition.xyz);
				OUT.pos = mul(UNITY_MATRIX_MVP, IN.vertex); 
				return OUT;
			}
			
			float auraexpand;
			//FRAGMENT PROGRAM
			half4 fp(vertexOutput IN) : COLOR
			{
				float4 finalColor = MainColor;
				float3 Ln = normalize(IN.V);
				float3 Nn = normalize(IN.N);
				float intensity = saturate(dot(Ln, Nn)) * auraexpand;
				finalColor.a = 1 - intensity;
				return finalColor;
			}
			
			ENDCG
		}
	} 
	FallBack "VertexLit"
}
