Shader "Rocket/RimCutoff" {
Properties {
	_Mask ("Mask Texture", 2D) = "white" {}
	_RimColor ("Rim Color", Color) = (0.5,0.5,0.5,0.5)
	_InnerColor ("Inner Color", Color) = (0.5,0.5,0.5,0.5)
	_InnerColorPower ("Inner Color Power", Range(0.0,1.0)) = 0.5
	_RimPower ("Rim Power", Range(0.0,5.0)) = 2.5
	_AlphaPower ("Alpha Rim Power", Range(0.0,8.0)) = 4.0
	_AllPower ("All Power", Range(0.0, 10.0)) = 1.0
	_Cutoff ("Cutoff", Range(0.0, 1.0)) = 1.0
}
	SubShader 
	{
		Tags { "Queue" = "Transparent" }

		CGPROGRAM
		#pragma surface surf Lambert alpha novertexlights nolightmap nodirlightmap
		struct Input 
		{
			half2 uv_Mask;
			half3 viewDir;
			INTERNAL_DATA
		};
		
		sampler2D _Mask;
		fixed4 _RimColor;
		half _RimPower;
		half _AlphaPower;
		half _AlphaMin;
		half _InnerColorPower;
		half _AllPower;
		fixed4 _InnerColor;
		half _Cutoff;
		
		void surf (Input IN, inout SurfaceOutput o) 
		{
			half2 newUV = half2(IN.uv_Mask.x  + sin(_Time.x*2), IN.uv_Mask.y); 
			fixed4 c =  tex2D(_Mask, newUV); 

			//half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
			half rim = 1.0 - saturate(dot (normalize(float3(0.0,1.0,0.0)), o.Normal));
			o.Emission = _RimColor.rgb * pow (rim, _RimPower) * _AllPower + (_InnerColor.rgb * 2 * _InnerColorPower);

			o.Alpha = (pow (rim, _AlphaPower))*_AllPower + c.g;
			if(_Cutoff > c.r)
				o.Alpha = 0;
		}
		ENDCG 
	}
	Fallback "VertexLit"
} 