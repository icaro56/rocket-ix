Shader "Rocket/Incandecente" {
	Properties
	{
		MainColor ("Main Color", Color) = (1.0, 1.0, 0.5, 1.0)
		RGBBase ("RGB (Base)", 2D) = "white" {}	
	}
	SubShader 
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Alphatest Greater 0 ZWrite Off ColorMask RGB

		Pass 
		{
			Name "Pass0"
			Blend SrcAlpha OneMinusSrcAlpha  
			
			CGPROGRAM
			#pragma vertex vp
			#pragma fragment fp
			#include "UnityCG.cginc"
			
			float4 MainColor;
			sampler2D RGBBase;
			
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float3 uv : TEXCOORD0;
			};
			
			//VERTEX PROGRAM
			vertexOutput vp(appdata_base IN)
			{
				vertexOutput OUT;
				OUT.uv = IN.texcoord;
				OUT.pos = mul(UNITY_MATRIX_MVP, IN.vertex); 
				return OUT;
			}
			
			float incandecencia;
			//FRAGMENT PROGRAM
			half4 fp(vertexOutput IN) : COLOR
			{
				//Calculando textura
				float4 tex = tex2D(RGBBase, IN.uv);
				float4 finalColor = tex * MainColor;
				finalColor -= incandecencia;
				//finalColor.a = finalColor.a * incandecencia;
				return finalColor;
			}
			
			ENDCG
		}
	} 
	FallBack "VertexLit"
}
