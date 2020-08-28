Shader "Custom/EdgeTex" {
	Properties
	{
		MainTex ("Texture", 2D) = "white" {}
		Slope ("Slope", Range(0.0, 1.0)) = 0.3
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
			
			sampler2D MainTex;
			float Slope; 
			
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 ldn : TEXCOORD1;
			};
			
			float4 MainTex_ST;
			
			//VERTEX PROGRAM
			vertexOutput vp(appdata_base IN)
			{
				vertexOutput OUT;
				float3 worldNormal = mul(_Object2World, float4(IN.normal, 0)).xyz;
				float4 worldPosition = mul(_Object2World, IN.vertex);
				float3 camNormal = (_WorldSpaceCameraPos - worldPosition.xyz);
				 
				float3 Ln = normalize(camNormal);
				float3 Nn = normalize(worldNormal);
				OUT.ldn = saturate(dot(Ln, Nn));
				OUT.pos = mul(UNITY_MATRIX_MVP, IN.vertex); 
				OUT.uv = IN.texcoord;
				return OUT;
			}
			
			
			//FRAGMENT PROGRAM
			half4 fp(vertexOutput IN) : COLOR
			{
				float test = IN.ldn;
			 	if(test < Slope)
					test = 0;

				half4 texcol = tex2D (MainTex, IN.uv);
				return texcol * test;
			}
			
			ENDCG
		}
	} 
	FallBack "VertexLit"
}
