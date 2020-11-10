// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

//https://sharpcoderblog.com/blog/create-a-hologram-effect-in-unity-3d @2019
//http://man.hubwiz.com/docset/Unity_3D.docset/Contents/Resources/Documents/docs.unity3d.com/Manual/SinglePassStereoRenderingHoloLens.html
Shader "FX/Hologram Shader"
{
	Properties
	{
		_Color("Color", Color) = (0, 1, 1, 1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_AlphaTexture("Alpha Mask (R)", 2D) = "white" {}
		//Alpha Mask Properties
		_Scale("Alpha Tiling", Float) = 3
		_ScrollSpeedV("Alpha scroll Speed", Range(0, 5.0)) = 1.0
		// Glow
		_GlowIntensity("Glow Intensity", Range(0.01, 1.0)) = 0.5
		// Glitch
		_GlitchSpeed("Glitch Speed", Range(0, 50)) = 50.0
		_GlitchIntensity("Glitch Intensity", Range(0.0, 0.1)) = 0
		//distortion
		_Distort("Distort", Range(0, 100)) = 1.0
	}

		SubShader
		{
			Tags{ "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			GrabPass{ "_GrabTexture" }
			Pass
			{
				Lighting Off
				ZWrite On
				Blend SrcAlpha One
				Cull Back

				CGPROGRAM

					#pragma vertex vertexFunc
					#pragma fragment fragmentFunc

					#include "UnityCG.cginc"

					struct appdata {
						float4 vertex : POSITION;
						float2 uv : TEXCOORD0;
						float3 normal : NORMAL;
						UNITY_VERTEX_INPUT_INSTANCE_ID
					};

					struct v2f {
						float4 position : SV_POSITION;
						float2 uv : TEXCOORD0;
						float3 grabPos : TEXCOORD1;
						float3 viewDir : TEXCOORD2;
						float3 worldNormal : NORMAL;
						UNITY_VERTEX_INPUT_INSTANCE_ID
						UNITY_VERTEX_OUTPUT_STEREO
					};

					fixed4 _Color, _MainTex_ST, _GrabTexture_TexelSize;
					fixed _Distort;
					sampler2D  _GrabTexture;
					half _Scale, _ScrollSpeedV, _GlowIntensity, _GlitchSpeed, _GlitchIntensity;

					v2f vertexFunc(appdata IN) {
						v2f OUT;

						UNITY_SETUP_INSTANCE_ID(IN);
						UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
						UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
						OUT.position = UnityObjectToClipPos(IN.vertex);
						OUT.uv = IN.uv;

						//OUT.position = UnityObjectToClipPos(IN.vertex);
						//OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);

						//Glitch
						IN.vertex.z += sin(_Time.y * _GlitchSpeed * 5 * IN.vertex.y) * _GlitchIntensity;

						//Alpha mask coordinates
						OUT.grabPos = UnityObjectToViewPos(IN.vertex);

						//Scroll Alpha mask uv
						OUT.uv.y += _Time * _ScrollSpeedV;

						OUT.worldNormal = UnityObjectToWorldNormal(IN.normal);
						OUT.viewDir = normalize(UnityWorldSpaceViewDir(OUT.grabPos.xyz));

						return OUT;
					}

					UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
					UNITY_DECLARE_SCREENSPACE_TEXTURE(_AlphaTexture);

					fixed4 fragmentFunc(v2f IN, fixed face : VFACE) : SV_Target{

						half dirVertex = (dot(IN.grabPos, 1.0) + 1) / 2;

						UNITY_SETUP_INSTANCE_ID(IN);

						fixed4 alphaColor = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_AlphaTexture,  IN.grabPos.xy * _Scale);
						fixed4 pixelColor = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, IN.uv);
						pixelColor = 1 - pixelColor;
						//pixelColor.w = alphaColor.w;


						//Distortion.
						IN.viewDir.xy += (pixelColor.rg * 2 - 1) * _Distort * _GrabTexture_TexelSize.xy;
						fixed4 distortColor = tex2D(_GrabTexture, IN.viewDir);
						distortColor *= _Color * _Color.a;
						pixelColor = lerp(distortColor, pixelColor, IN.grabPos.r);

						// Rim Light
						half rim = 1.0 - saturate(dot(IN.viewDir, IN.worldNormal));



						return pixelColor * _Color * (rim + _GlowIntensity);
					}

					

				ENDCG
			}
		}
}