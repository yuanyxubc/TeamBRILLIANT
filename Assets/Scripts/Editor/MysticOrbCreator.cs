using UnityEngine;
using UnityEditor;

/// <summary>
/// Unity编辑器工具 - 一键创建神秘能量球
/// </summary>
public class MysticOrbCreator : EditorWindow
{
    private string orbName = "MysticOrb";
    private Color primaryColor = new Color(0.3f, 0.6f, 1f);
    private Color secondaryColor = new Color(1f, 0.7f, 0.3f);
    private bool createWithParticles = true;
    private bool createWithLightning = true;
    private bool createWithFloating = true;
    
    [MenuItem("GameObject/Effects/创建神秘能量球")]
    public static void ShowWindow()
    {
        MysticOrbCreator window = GetWindow<MysticOrbCreator>("能量球创建器");
        window.minSize = new Vector2(400, 500);
        window.Show();
    }
    
    void OnGUI()
    {
        GUILayout.Label("神秘能量球创建器", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        EditorGUILayout.HelpBox("这个工具会自动创建一个完整的能量球效果，包括模型、材质和脚本。", MessageType.Info);
        GUILayout.Space(10);
        
        // 基础设置
        GUILayout.Label("基础设置", EditorStyles.boldLabel);
        orbName = EditorGUILayout.TextField("能量球名称", orbName);
        GUILayout.Space(5);
        
        // 颜色设置
        GUILayout.Label("颜色设置", EditorStyles.boldLabel);
        primaryColor = EditorGUILayout.ColorField("主要颜色（外层）", primaryColor);
        secondaryColor = EditorGUILayout.ColorField("次要颜色（内层）", secondaryColor);
        GUILayout.Space(5);
        
        // 功能选项
        GUILayout.Label("功能选项", EditorStyles.boldLabel);
        createWithParticles = EditorGUILayout.Toggle("包含粒子系统", createWithParticles);
        createWithLightning = EditorGUILayout.Toggle("包含闪电效果", createWithLightning);
        createWithFloating = EditorGUILayout.Toggle("包含浮动效果", createWithFloating);
        GUILayout.Space(10);
        
        // 预设按钮
        GUILayout.Label("快速预设", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("蓝色主题"))
        {
            primaryColor = new Color(0.3f, 0.6f, 1f);
            secondaryColor = new Color(1f, 0.7f, 0.3f);
        }
        if (GUILayout.Button("紫色主题"))
        {
            primaryColor = new Color(0.5f, 0f, 1f);
            secondaryColor = new Color(1f, 0.3f, 0.8f);
        }
        if (GUILayout.Button("绿色主题"))
        {
            primaryColor = new Color(0f, 1f, 0.5f);
            secondaryColor = new Color(0.8f, 1f, 0f);
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(20);
        
        // 创建按钮
        if (GUILayout.Button("创建能量球", GUILayout.Height(40)))
        {
            CreateMysticOrb();
        }
        
        GUILayout.Space(10);
        EditorGUILayout.HelpBox("创建后，请确保您的项目使用Universal Render Pipeline (URP)，并启用了Bloom后处理效果。", MessageType.Warning);
    }
    
    void CreateMysticOrb()
    {
        // 创建主对象
        GameObject mainOrb = new GameObject(orbName);
        
        // 设置位置
        if (SceneView.lastActiveSceneView != null)
        {
            mainOrb.transform.position = SceneView.lastActiveSceneView.camera.transform.position + 
                                        SceneView.lastActiveSceneView.camera.transform.forward * 5f;
        }
        
        // 创建材质
        Material outerGlowMat = CreateOuterGlowMaterial(primaryColor);
        Material innerCoreMat = CreateInnerCoreMaterial(secondaryColor);
        Material energyVortexMat = CreateEnergyVortexMaterial(primaryColor);
        
        // 创建外层发光球
        GameObject outerGlow = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        outerGlow.name = "OuterGlow";
        outerGlow.transform.parent = mainOrb.transform;
        outerGlow.transform.localPosition = Vector3.zero;
        outerGlow.transform.localScale = Vector3.one * 1.2f;
        outerGlow.GetComponent<Renderer>().material = outerGlowMat;
        DestroyImmediate(outerGlow.GetComponent<Collider>());
        
        // 创建内部核心
        GameObject innerCore = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        innerCore.name = "InnerCore";
        innerCore.transform.parent = mainOrb.transform;
        innerCore.transform.localPosition = Vector3.zero;
        innerCore.transform.localScale = Vector3.one * 0.8f;
        innerCore.GetComponent<Renderer>().material = innerCoreMat;
        DestroyImmediate(innerCore.GetComponent<Collider>());
        
        // 创建能量漩涡
        GameObject energyVortex = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        energyVortex.name = "EnergyVortex";
        energyVortex.transform.parent = mainOrb.transform;
        energyVortex.transform.localPosition = Vector3.zero;
        energyVortex.transform.localScale = Vector3.one * 0.9f;
        energyVortex.GetComponent<Renderer>().material = energyVortexMat;
        DestroyImmediate(energyVortex.GetComponent<Collider>());
        
        // 添加能量流动脚本
        OrbEnergyFlow energyFlow = energyVortex.AddComponent<OrbEnergyFlow>();
        
        // 添加主脚本
        MysticOrb orbScript = mainOrb.AddComponent<MysticOrb>();
        
        // 使用反射设置私有字段（因为是SerializeField）
        SerializedObject serializedOrb = new SerializedObject(orbScript);
        serializedOrb.FindProperty("innerSphere").objectReferenceValue = innerCore.transform;
        serializedOrb.FindProperty("glowMaterial").objectReferenceValue = outerGlowMat;
        serializedOrb.ApplyModifiedProperties();
        
        // 创建粒子系统
        if (createWithParticles)
        {
            GameObject particlesParent = new GameObject("Particles");
            particlesParent.transform.parent = mainOrb.transform;
            particlesParent.transform.localPosition = Vector3.zero;
            
            CreateSpiralParticles(particlesParent.transform, primaryColor);
            CreateSparkParticles(particlesParent.transform, secondaryColor);
            CreateGlowParticles(particlesParent.transform, primaryColor);
        }
        
        // 添加闪电效果
        if (createWithLightning)
        {
            OrbLightningEffect lightning = mainOrb.AddComponent<OrbLightningEffect>();
        }
        
        // 添加浮动效果
        if (createWithFloating)
        {
            mainOrb.AddComponent<OrbFloating>();
        }
        
        // 添加光源
        GameObject lightObj = new GameObject("OrbLight");
        lightObj.transform.parent = mainOrb.transform;
        lightObj.transform.localPosition = Vector3.zero;
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = primaryColor;
        light.intensity = 3f;
        light.range = 8f;
        
        // 选中新创建的对象
        Selection.activeGameObject = mainOrb;
        
        // 聚焦到对象
        if (SceneView.lastActiveSceneView != null)
        {
            SceneView.lastActiveSceneView.FrameSelected();
        }
        
        Debug.Log($"成功创建神秘能量球: {orbName}");
    }
    
    Material CreateOuterGlowMaterial(Color color)
    {
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.name = "OuterGlow_Mat";
        
        // 设置为透明
        mat.SetFloat("_Surface", 1); // Transparent
        mat.SetFloat("_Blend", 0); // Alpha
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.renderQueue = 3000;
        
        Color baseColor = new Color(color.r, color.g, color.b, 0.3f);
        mat.SetColor("_BaseColor", baseColor);
        
        // 启用发光
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", color * 3f);
        
        return mat;
    }
    
    Material CreateInnerCoreMaterial(Color color)
    {
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.name = "InnerCore_Mat";
        
        mat.SetColor("_BaseColor", color);
        
        // 启用发光
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", color * 5f);
        
        return mat;
    }
    
    Material CreateEnergyVortexMaterial(Color color)
    {
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.name = "EnergyVortex_Mat";
        
        // 设置为透明
        mat.SetFloat("_Surface", 1); // Transparent
        mat.SetFloat("_Blend", 0); // Alpha
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.renderQueue = 3000;
        
        Color baseColor = new Color(color.r, color.g, color.b, 0.5f);
        mat.SetColor("_BaseColor", baseColor);
        
        // 启用发光
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", color * 2f);
        
        return mat;
    }
    
    void CreateSpiralParticles(Transform parent, Color color)
    {
        GameObject particleObj = new GameObject("SpiralParticles");
        particleObj.transform.parent = parent;
        particleObj.transform.localPosition = Vector3.zero;
        
        ParticleSystem ps = particleObj.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.duration = 5f;
        main.loop = true;
        main.startLifetime = 2f;
        main.startSpeed = 0.5f;
        main.startSize = 0.08f;
        main.startColor = color;
        main.maxParticles = 100;
        
        var emission = ps.emission;
        emission.rateOverTime = 20f;
        
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.5f;
        
        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color, 0f), new GradientColorKey(color, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
        );
        colorOverLifetime.color = gradient;
        
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        Material particleMat = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
        particleMat.SetColor("_BaseColor", color);
        particleMat.SetFloat("_Surface", 1);
        particleMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        particleMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
        renderer.material = particleMat;
    }
    
    void CreateSparkParticles(Transform parent, Color color)
    {
        GameObject particleObj = new GameObject("SparkParticles");
        particleObj.transform.parent = parent;
        particleObj.transform.localPosition = Vector3.zero;
        
        ParticleSystem ps = particleObj.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.duration = 1f;
        main.loop = true;
        main.startLifetime = 0.7f;
        main.startSpeed = 1.5f;
        main.startSize = 0.04f;
        main.startColor = color;
        main.maxParticles = 50;
        
        var emission = ps.emission;
        emission.rateOverTime = 10f;
        emission.SetBursts(new ParticleSystem.Burst[] {
            new ParticleSystem.Burst(0f, 10)
        });
        
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.6f;
        
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        Material particleMat = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
        particleMat.SetColor("_BaseColor", color);
        particleMat.SetFloat("_Surface", 1);
        particleMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        particleMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
        renderer.material = particleMat;
    }
    
    void CreateGlowParticles(Transform parent, Color color)
    {
        GameObject particleObj = new GameObject("GlowParticles");
        particleObj.transform.parent = parent;
        particleObj.transform.localPosition = Vector3.zero;
        
        ParticleSystem ps = particleObj.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.duration = 5f;
        main.loop = true;
        main.startLifetime = 4f;
        main.startSpeed = 0.1f;
        main.startSize = 0.4f;
        Color glowColor = new Color(color.r, color.g, color.b, 0.3f);
        main.startColor = glowColor;
        main.maxParticles = 30;
        
        var emission = ps.emission;
        emission.rateOverTime = 5f;
        
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.7f;
        
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        Material particleMat = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
        particleMat.SetColor("_BaseColor", glowColor);
        particleMat.SetFloat("_Surface", 1);
        particleMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        particleMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
        renderer.material = particleMat;
    }
}

