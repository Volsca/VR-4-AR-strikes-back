Shader "Custom/OutlineShader"
{
    Properties
    {
        _Color ("Base Color", Color) = (1,1,1,1)
        _Size ("Object Size", Float) = 1.1
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineSize ("Outline Size", Range(0, 0.5)) = 0.01
        _OutlineTransparency ("Outline Transparency", Range(0, 1)) = 0.5
    }
    
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100
        
        
        // First pass for the base object
        Pass
        {
            Name "BaseObject"
            Tags { "LightMode" = "ForwardBase" }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
            };
            
            struct v2f
            {
                float4 pos : POSITION;
            };
            
			float _Size;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = v.vertex * _Size;//UnityObjectToClipPos(v.vertex) * _Size;
                return o;
            }
            
            fixed4 _Color;
            
            half4 frag (v2f i) : SV_Target
            {
                return fixed4(_Color.rgb, _Color.a);
            }
            ENDCG
        }
        
        // Second pass for the outline
        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "Always" }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
            };
            
            struct v2f
            {
                float4 pos : POSITION;
            };
            
            fixed4 _OutlineColor;
            float _OutlineSize;
            float _OutlineTransparency;
			
            
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex) * _OutlineSize;//(_Size + _OutlineSize); // _Size not found ?!
                return o;
            }
            
            half4 frag (v2f i) : SV_Target
            {
                return fixed4(_OutlineColor.rgb, _OutlineTransparency);
            }
            ENDCG
        }
    }
}