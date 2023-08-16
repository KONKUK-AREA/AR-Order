Shader "Sprites/AlphaMask" { 
    Properties { 
      _MainTex("Base (RGB) Trans (A)", 2D) = "white" {} 
      _Color ("Main Color", Color) = (0,0,0,0) 
      _MaskTex("Mask Texture (RGB)", 2D) = "white" {} 
      _CutOff("_CutOff", Range(0,5) ) = 0.5 
    } 
    Subshader { 
      Tags { 
        "Queue"="Transparent" 
        "IgnoreProjector"="False" 
        "RenderType"="Transparent" 
      } 
  
      Lighting On //doesn't do anything 
  
      CGPROGRAM 
      #pragma surface surf Lambert alpha 
  
        struct Input { 
          float2 uv_MainTex; 
        }; 
  
        half4 _Color; 
        sampler2D _MainTex; 
        sampler2D _MaskTex; 
        float _CutOff; 
  
        void surf (Input IN, inout SurfaceOutput o) { 
          half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color; 
          o.Emission = c.rgb; 
          o.Alpha = c.a -( tex2D(_MaskTex, IN.uv_MainTex).a * _CutOff); 
      } 
      ENDCG 
    } 
}