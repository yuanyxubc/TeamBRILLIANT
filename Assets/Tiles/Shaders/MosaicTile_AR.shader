Shader "Custom/MosaicTile_AR"
{
    Properties
    {
        [Header(Base Settings)]
        _BaseMap ("Base Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        
        [Header(Emission Settings)]
        _EmissionMap ("Emission Map", 2D) = "white" {}
        [Enum(Solid Color,0, Gradient,1, Texture,2)] _EmissionColorMode ("Emission Color Mode", Float) = 0
        _EmissionColor ("Emission Color (Solid/Gradient Start)", Color) = (1, 0.84, 0, 1)
        _EmissionColorEnd ("Emission Gradient End", Color) = (1, 0.5, 0, 1)
        _EmissionColorTex ("Emission Color Texture", 2D) = "white" {}
        _EmissionIntensity ("Emission Intensity", Range(0, 20)) = 5.0
        
        [Header(Pulse Animation)]
        _PulseSpeed ("Pulse Speed", Range(0, 10)) = 2.0
        _PulseAmplitude ("Pulse Amplitude", Range(0, 1)) = 0.5
        
        [Header(Color Shift)]
        _ColorShiftSpeed ("Color Shift Speed", Range(0, 5)) = 1.0
        _ColorShiftAmount ("Color Shift Amount", Range(0, 0.3)) = 0.15
        
        [Header(Fresnel Glow)]
        _FresnelPower ("Fresnel Power", Range(0.1, 10)) = 3.0
        _FresnelIntensity ("Fresnel Intensity", Range(0, 5)) = 1.5
        [Enum(Solid Color,0, Gradient,1, Texture,2)] _FresnelColorMode ("Fresnel Color Mode", Float) = 0
        _FresnelColor ("Fresnel Color (Solid/Gradient Start)", Color) = (1, 1, 1, 1)
        _FresnelColorEnd ("Fresnel Gradient End", Color) = (0.5, 0.8, 1, 1)
        _FresnelColorTex ("Fresnel Color Texture", 2D) = "white" {}
        
        [Header(Float Animation)]
        _FloatSpeed ("Float Speed", Range(0, 5)) = 1.0
        _FloatAmount ("Float Amount", Range(0, 0.5)) = 0.1
        
        [Header(Sparkle Effect)]
        _SparkleSpeed ("Sparkle Speed", Range(0, 10)) = 3.0
        _SparkleIntensity ("Sparkle Intensity", Range(0, 2)) = 0.5
        
        [Header(Transparency)]
        _Alpha ("Alpha", Range(0, 1)) = 0.9
        
        [Header(Render Settings)]
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 10
        [Enum(Off, 0, On, 1)] _ZWrite ("Z Write", Float) = 0
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }
        
        LOD 100
        Blend [_SrcBlend] [_DstBlend]
        ZWrite [_ZWrite]
        Cull Off
        
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float3 viewDirWS : TEXCOORD3;
                float fogFactor : TEXCOORD4;
            };
            
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            
            TEXTURE2D(_EmissionMap);
            SAMPLER(sampler_EmissionMap);
            
            TEXTURE2D(_EmissionColorTex);
            SAMPLER(sampler_EmissionColorTex);
            
            TEXTURE2D(_FresnelColorTex);
            SAMPLER(sampler_FresnelColorTex);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _EmissionColorTex_ST;
                float4 _FresnelColorTex_ST;
                float4 _BaseColor;
                float4 _EmissionColor;
                float4 _EmissionColorEnd;
                float _EmissionColorMode;
                float _EmissionIntensity;
                float _PulseSpeed;
                float _PulseAmplitude;
                float _ColorShiftSpeed;
                float _ColorShiftAmount;
                float _FresnelPower;
                float _FresnelIntensity;
                float _FresnelColorMode;
                float4 _FresnelColor;
                float4 _FresnelColorEnd;
                float _FloatSpeed;
                float _FloatAmount;
                float _SparkleSpeed;
                float _SparkleIntensity;
                float _Alpha;
            CBUFFER_END
            
            // 噪声函数
            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453123);
            }
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                
                // Float animation effect (more visible)
                float3 positionOS = input.positionOS.xyz;
                float floatOffset = sin(_Time.y * _FloatSpeed + positionOS.x * 2.0) * _FloatAmount;
                positionOS.y += floatOffset;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(positionOS);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS);
                
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.normalWS = normalInput.normalWS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);
                output.fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
                
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                // Sample textures
                half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
                half4 baseColor = baseMap * _BaseColor;
                half4 emissionMap = SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, input.uv);
                
                // Enhanced pulse effect
                // Use more visible pulse curve
                float pulse = sin(_Time.y * _PulseSpeed) * 0.5 + 0.5;
                pulse = pow(pulse, 2.0); // Enhanced contrast
                float pulseMultiplier = lerp(1.0 - _PulseAmplitude, 1.0 + _PulseAmplitude, pulse);
                
                // Get emission color (based on mode)
                float3 emissionBaseColor;
                if (_EmissionColorMode < 0.5) // Solid Color
                {
                    emissionBaseColor = _EmissionColor.rgb;
                }
                else if (_EmissionColorMode < 1.5) // Gradient
                {
                    float gradientFactor = input.uv.y; // 使用V坐标做渐变
                    emissionBaseColor = lerp(_EmissionColor.rgb, _EmissionColorEnd.rgb, gradientFactor);
                }
                else // Texture
                {
                    float2 emissionTexUV = TRANSFORM_TEX(input.uv, _EmissionColorTex);
                    half4 emissionColorTex = SAMPLE_TEXTURE2D(_EmissionColorTex, sampler_EmissionColorTex, emissionTexUV);
                    emissionBaseColor = emissionColorTex.rgb;
                }
                
                // Color shift effect (make color change over time)
                float colorShift = sin(_Time.y * _ColorShiftSpeed) * _ColorShiftAmount;
                float3 shiftedColor = emissionBaseColor;
                shiftedColor.r += colorShift;
                shiftedColor.g += sin(_Time.y * _ColorShiftSpeed * 1.3) * _ColorShiftAmount;
                shiftedColor.b += sin(_Time.y * _ColorShiftSpeed * 0.7) * _ColorShiftAmount;
                shiftedColor = saturate(shiftedColor);
                
                // Sparkle effect
                float sparkle = hash(input.uv + frac(_Time.y * _SparkleSpeed));
                sparkle = smoothstep(0.97, 1.0, sparkle) * _SparkleIntensity;
                
                // Fresnel rim lighting
                float3 normalWS = normalize(input.normalWS);
                float3 viewDirWS = normalize(input.viewDirWS);
                float fresnel = pow(1.0 - saturate(dot(normalWS, viewDirWS)), _FresnelPower);
                
                // Get Fresnel color (based on mode)
                float3 fresnelBaseColor;
                if (_FresnelColorMode < 0.5) // Solid Color
                {
                    fresnelBaseColor = _FresnelColor.rgb;
                }
                else if (_FresnelColorMode < 1.5) // Gradient
                {
                    float gradientFactor = input.uv.y;
                    fresnelBaseColor = lerp(_FresnelColor.rgb, _FresnelColorEnd.rgb, gradientFactor);
                }
                else // Texture
                {
                    float2 fresnelTexUV = TRANSFORM_TEX(input.uv, _FresnelColorTex);
                    half4 fresnelColorTex = SAMPLE_TEXTURE2D(_FresnelColorTex, sampler_FresnelColorTex, fresnelTexUV);
                    fresnelBaseColor = fresnelColorTex.rgb;
                }
                
                // Combine all effects
                // Emission intensity affected by pulse
                float finalIntensity = _EmissionIntensity * pulseMultiplier;
                // Add sparkle
                finalIntensity += sparkle * _EmissionIntensity;

                // Emission color
                half3 emission = shiftedColor * finalIntensity * emissionMap.rgb;

                // Fresnel rim highlight (using different color to make it more visible)
                half3 fresnelColor = lerp(shiftedColor, fresnelBaseColor, 0.5); // Blend emission and fresnel colors
                half3 fresnelGlow = fresnel * fresnelColor * _FresnelIntensity * pulseMultiplier;
                emission += fresnelGlow;

                // Final color
                half3 finalColor = baseColor.rgb + emission;

                // Apply fog
                finalColor = MixFog(finalColor, input.fogFactor);
                
                return half4(finalColor, baseColor.a * _Alpha);
            }
            ENDHLSL
        }
    }
    
    FallBack "Universal Render Pipeline/Lit"
}

