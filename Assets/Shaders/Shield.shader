Shader "Unlit/ShieldFX"
{//https://github.com/vux427/ForceFieldFX/blob/master/ForceFieldFX/Assets/Shader/ShieldFX.shader
	Properties
	{
		_MainColor("MainColor", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_Fresnel("Fresnel Intensity", Range(0,200)) = 3.0
		_FresnelWidth("Fresnel Width", Range(0,2)) = 3.0
		_Distort("Distort", Range(0, 100)) = 1.0
		_IntersectionThreshold("Highlight of intersection threshold", range(0,1)) = .1 //Max difference for intersections
		_ScrollSpeedU("Scroll U Speed",float) = 2
		_ScrollSpeedV("Scroll V Speed",float) = 0

		_EmissionMap("Emission Map", 2D) = "black" {}
		[HDR] _EmissionColor("Emission Color", Color) = (0,0,0)
	}
		SubShader
		{
			Tags{ "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

			GrabPass{ "_GrabTexture" }
			Pass
			{
				Lighting Off ZWrite On
				Blend SrcAlpha OneMinusSrcAlpha
				Cull Off

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct appdata
				{
					fixed4 vertex : POSITION;
					fixed4 normal : NORMAL;
					fixed3 uv : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f
				{
					fixed2 uv : TEXCOORD0;
					fixed4 vertex : SV_POSITION;
					fixed4 rimColor : TEXCOORD1;
					fixed4 screenPos : TEXCOORD2;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
				};

				sampler2D _CameraDepthTexture, _GrabTexture, _EmissionMap;
				fixed4 _MainTex_ST,_MainColor,_GrabTexture_ST, _GrabTexture_TexelSize, _EmissionColor;
				fixed _Fresnel, _FresnelWidth, _Distort, _IntersectionThreshold, _ScrollSpeedU, _ScrollSpeedV;
		
				UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);


				v2f vert(appdata v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, v.uv);

					
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;

					//scroll uv
					o.uv.x += _Time * _ScrollSpeedU;
					o.uv.y += _Time * _ScrollSpeedV;

					//fresnel 
					fixed3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
					fixed dotProduct = 1 - saturate(dot(v.normal, viewDir));
					o.rimColor = smoothstep(1 - _FresnelWidth, 1.0, dotProduct) * .5f;
					o.screenPos = ComputeScreenPos(o.vertex);
					COMPUTE_EYEDEPTH(o.screenPos.z);//eye space depth of the vertex 
					return o;
				}

				fixed4 frag(v2f i,fixed face : VFACE) : SV_Target
				{
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

					//intersection
					fixed intersect = saturate((abs(LinearEyeDepth(tex2D(_CameraDepthTexture,i.screenPos).r) - i.screenPos.z)) / _IntersectionThreshold);
					fixed4 albedo = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv);

					fixed3 main = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv);
					//distortion
					i.screenPos.xy += (main.rg * 2 - 1) * _Distort * _GrabTexture_TexelSize.xy;
					fixed3 distortColor = tex2D(_GrabTexture, i.screenPos);
					distortColor *= _MainColor * _MainColor.a + 1;

					//intersect hightlight
					i.rimColor *= intersect * clamp(0,1,face);
					main *= _MainColor * pow(_Fresnel,i.rimColor);


					//lerp distort color & fresnel color
					main = lerp(distortColor, main, i.rimColor.r);
					main += (face > 0 ? .03 : .3)* _MainColor* _Fresnel;

					half4 output = half4(albedo.rgb * main.rgb, albedo.a);

					half4 emission = tex2D(_EmissionMap, i.uv) * _EmissionColor;
					output.rgb += emission.rgb;
					main.rgb += output.rgb;
					return fixed4(main,.3);
				}

				struct Input 
				{
					float2 uv_MainTex;
				};
				ENDCG
			}
}
		}