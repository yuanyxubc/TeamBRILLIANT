Shader "Custom/MosaicTile_VR"
{
    Properties
    {
        [Header(Base Settings)]
        _BaseMap ("Base Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        
        [Header(Emission Settings)]
        _EmissionMap ("Emission Map", 2D) = "white" {}
        [Enum(Solid Color,0, Gradient,1, Texture,2)] _EmissionColorMode ("Emission Color Mode", Float) = 0
        _EmissionColor ("Emission Color (Solid/Gradient Start)", Color) = (0, 0.75, 1, 1)
        _EmissionColorEnd ("Emission Gradient End", Color) = (0, 0.4, 1, 1)
        _EmissionColorTex ("Emission Color Texture", 2D) = "white" {}
        _EmissionIntensity ("Emission Intensity", Range(0, 50)) = 15.0
        
        [Header(Pulse Animation)]
        _PulseSpeed ("Pulse Speed", Range(0, 10)) = 2.0
        _PulseAmplitude ("Pulse Amplitude", Range(0, 1)) = 0.4
        
        [Header(Flow Animation)]
        _FlowSpeed ("Flow Speed", Range(0, 5)) = 1.5
        _FlowTiling ("Flow Tiling", Range(1, 20)) = 8.0
        _FlowIntensity ("Flow Intensity", Range(0, 3)) = 1.0
        
        [Header(Ripple Effect)]
        _RippleSpeed ("Ripple Speed", Range(0, 5)) = 2.0
        _RippleFrequency ("Ripple Frequency", Range(1, 50)) = 15.0
        _RippleAmplitude ("Ripple Amplitude", Range(0, 2)) = 0.8
        
        [Header(Scanline Effect)]
        _ScanlineSpeed ("Scanline Speed", Range(0, 10)) = 2.0
        _ScanlineWidth ("Scanline Width", Range(0.01, 0.5)) = 0.15
        _ScanlineIntensity ("Scanline Intensity", Range(0, 10)) = 3.0
        
        [Header(Color Pulse)]
        _ColorPulseSpeed ("Color Pulse Speed", Range(0, 5)) = 1.5
        _ColorPulseAmount ("Color Pulse Amount", Range(0, 0.5)) = 0.2
        
        [Header(Fresnel Glow)]
        _FresnelPower ("Fresnel Power", Range(0.1, 10)) = 2.0
        _FresnelIntensity ("Fresnel Intensity", Range(0, 15)) = 5.0
        [Enum(Solid Color,0, Gradient,1, Texture,2)] _FresnelColorMode ("Fresnel Color Mode", Float) = 0
        _FresnelColor ("Fresnel Color (Solid/Gradient Start)", Color) = (0.5, 0.8, 1, 1)
        _FresnelColorEnd ("Fresnel Gradient End", Color) = (1, 1, 1, 1)
        _FresnelColorTex ("Fresnel Color Texture", 2D) = "white" {}
        
        [Header(Interactive)]
        _InteractionBoost ("Interaction Boost", Range(0, 3)) = 1.5
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
            "RenderPipeline" = "UniversalPipeline"
        }
        
        LOD 100
        
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
                float _FlowSpeed;
                float _FlowTiling;
                float _FlowIntensity;
                float _RippleSpeed;
                float _RippleFrequency;
                float _RippleAmplitude;
                float _ScanlineSpeed;
                float _ScanlineWidth;
                float _ScanlineIntensity;
                float _ColorPulseSpeed;
                float _ColorPulseAmount;
                float _FresnelPower;
                float _FresnelIntensity;
                float _FresnelColorMode;
                float4 _FresnelColor;
                float4 _FresnelColorEnd;
                float _InteractionBoost;
            CBUFFER_END
            
            // 增强的噪声函数
            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453123);
            }
            
            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                f = f * f * (3.0 - 2.0 * f);
                
                float a = hash(i);
                float b = hash(i + float2(1.0, 0.0));
                float c = hash(i + float2(0.0, 1.0));
                float d = hash(i + float2(1.0, 1.0));
                
                return lerp(lerp(a, b, f.x), lerp(c, d, f.x), f.y);
            }
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
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
                
                // Enhanced pulse effect (non-linear curve)
                float pulse = sin(_Time.y * _PulseSpeed) * 0.5 + 0.5;
                pulse = pow(pulse, 1.5); // Enhanced contrast
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
                
                // Flow effect (more visible noise flow)
                float2 flowUV = input.uv * _FlowTiling;
                flowUV.x += _Time.y * _FlowSpeed;
                float flowNoise = noise(flowUV);
                flowNoise = pow(flowNoise, 2.0); // Enhanced contrast
                float flowPattern = flowNoise * _FlowIntensity;
                
                // Ripple effect (spreading from center)
                float2 center = float2(0.5, 0.5);
                float dist = distance(input.uv, center);
                float ripple = sin(dist * _RippleFrequency - _Time.y * _RippleSpeed);
                ripple = ripple * 0.5 + 0.5;
                ripple = pow(ripple, 2.0); // Enhanced contrast
                float rippleEffect = ripple * _RippleAmplitude;
                
                // Scanline effect (more visible)
                float scanline = frac(input.uv.y * 5.0 + _Time.y * _ScanlineSpeed);
                scanline = smoothstep(_ScanlineWidth, 0.0, abs(scanline - 0.5));
                scanline = pow(scanline, 0.5); // Make scanline more visible
                
                // Color pulse (make color itself change)
                float colorPulse = sin(_Time.y * _ColorPulseSpeed);
                float3 pulsedColor = emissionBaseColor;
                pulsedColor.r += colorPulse * _ColorPulseAmount;
                pulsedColor.g += sin(_Time.y * _ColorPulseSpeed * 1.3) * _ColorPulseAmount;
                pulsedColor.b += sin(_Time.y * _ColorPulseSpeed * 0.7) * _ColorPulseAmount;
                pulsedColor = saturate(pulsedColor);
                
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
                
                // Combine all effects (multiplication and addition blending)
                // Base intensity affected by pulse
                float intensityMultiplier = pulseMultiplier;

                // Add flow effect (addition)
                float additiveEffects = 0;
                additiveEffects += flowPattern * _EmissionIntensity * 0.3;
                additiveEffects += rippleEffect * _EmissionIntensity * 0.5;
                additiveEffects += scanline * _ScanlineIntensity;

                // Final emission intensity
                float finalIntensity = _EmissionIntensity * intensityMultiplier + additiveEffects;
                
                // Apply color
                half3 emission = pulsedColor * finalIntensity * emissionMap.rgb;

                // Fresnel rim (using fresnel color, more visible)
                half3 fresnelColor = lerp(pulsedColor, fresnelBaseColor, 0.5);
                half3 fresnelGlow = fresnel * fresnelColor * _FresnelIntensity;
                fresnelGlow *= pulseMultiplier; // Fresnel also affected by pulse
                emission += fresnelGlow;

                // Final color
                half3 finalColor = baseColor.rgb * 0.2 + emission; // Reduce base color influence

                // Apply fog
                finalColor = MixFog(finalColor, input.fogFactor);
                
                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
        
        // 阴影投射Pass
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            
            ZWrite On
            ZTest LEqual
            ColorMask 0
            
            HLSLPROGRAM
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };
            
            Varyings ShadowPassVertex(Attributes input)
            {
                Varyings output;
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.positionCS = TransformWorldToHClip(positionWS);
                return output;
            }
            
            half4 ShadowPassFragment(Varyings input) : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }
    }
    
    FallBack "Universal Render Pipeline/Lit"
}

