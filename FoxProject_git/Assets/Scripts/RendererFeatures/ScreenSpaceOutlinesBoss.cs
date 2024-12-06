using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class ScreenSpaceOutlinesBoss : ScriptableRendererFeature
{

    [System.Serializable]
    private class ScreenSpaceOutlineSettings
    {

        [Header("General Outline Settings")]
        public Color outlineColor = Color.white;
        public Color outlineColor_Boss = Color.red;
        public Color outlineColor_obj1 = Color.blue;
        public Color outlineColor_obj2 = Color.green;
        [Range(0.0f, 20.0f)]
        public float outlineScale = 1.0f;

        [Header("Depth Settings")]
        [Range(0.0f, 100.0f)]
        public float depthThreshold = 1.5f;
        [Range(0.0f, 500.0f)]
        public float robertsCrossMultiplier = 100.0f;

        [Header("Normal Settings")]
        [Range(0.0f, 1.0f)]
        public float normalThreshold = 0.4f;

        [Header("Depth Normal Relation Settings")]
        [Range(0.0f, 2.0f)]
        public float steepAngleThreshold = 0.2f;
        [Range(0.0f, 500.0f)]
        public float steepAngleMultiplier = 25.0f;

    }
    [System.Serializable]
    private class ViewSpaceNormalsTextureSettings
    {

        [Header("General Scene View Space Normal Texture Settings")]
        public RenderTextureFormat colorFormat;
        public int depthBufferBits = 16;
        public FilterMode filterMode;
        public Color backgroundColor = Color.black;

        [Header("View Space Normal Texture Object Draw Settings")]
        public PerObjectData perObjectData;
        public bool enableDynamicBatching;
        public bool enableInstancing;

    }

    //_SceneViewSpaceNormals
    private class ViewSpaceNormalsTexturePass : ScriptableRenderPass
    {

        private ViewSpaceNormalsTextureSettings normalsTextureSettings;
        private FilteringSettings filteringSettings;
        private FilteringSettings occluderFilteringSettings;

        private readonly List<ShaderTagId> shaderTagIdList;
        private readonly Material normalsMaterial;
        private readonly Material occludersMaterial;

        private readonly RenderTargetHandle normals;

        public ViewSpaceNormalsTexturePass(RenderPassEvent renderPassEvent, LayerMask layerMask, LayerMask occluderLayerMask, ViewSpaceNormalsTextureSettings settings)
        {
            this.renderPassEvent = renderPassEvent;
            this.normalsTextureSettings = settings;
            filteringSettings = new FilteringSettings(RenderQueueRange.opaque, layerMask);
            occluderFilteringSettings = new FilteringSettings(RenderQueueRange.opaque, occluderLayerMask);

            shaderTagIdList = new List<ShaderTagId> {
                new ShaderTagId("UniversalForward"),
                new ShaderTagId("UniversalForwardOnly"),
                new ShaderTagId("LightweightForward"),
                new ShaderTagId("SRPDefaultUnlit")
            };

            normals.Init("_SceneViewSpaceNormals");     //"_SceneViewSpaceNormals" = shaderProperty
            normalsMaterial = new Material(Shader.Find("Hidden/ViewSpaceNormals"));

            occludersMaterial = new Material(Shader.Find("Hidden/UnlitColor"));
            occludersMaterial.SetColor("_Color", normalsTextureSettings.backgroundColor);
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            RenderTextureDescriptor normalsTextureDescriptor = cameraTextureDescriptor;
            normalsTextureDescriptor.colorFormat = normalsTextureSettings.colorFormat;
            normalsTextureDescriptor.depthBufferBits = normalsTextureSettings.depthBufferBits;

            cmd.GetTemporaryRT(normals.id, normalsTextureDescriptor, normalsTextureSettings.filterMode);
            //This creates a temporary render texture with given parameters, and sets it up as a global shader property with nameID.
            //이렇게 하면 지정된 매개변수로 임시 렌더 텍스처가 생성되고 nameID가 있는 전역 셰이더 속성으로 설정됩니다.
            ConfigureTarget(normals.Identifier());  //Configures render targets for this render pass.이 렌더 패스에 대한 렌더 타겟을 구성
            ConfigureClear(ClearFlag.All, normalsTextureSettings.backgroundColor);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!normalsMaterial || !occludersMaterial)
            {
                Debug.Log("!normalsMaterial || !occludersMaterial");
                return;
            }
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler("SceneViewSpaceNormalsTextureCreation")))
            {
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                DrawingSettings drawSettings = CreateDrawingSettings(shaderTagIdList, ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
                drawSettings.perObjectData = normalsTextureSettings.perObjectData;
                drawSettings.enableDynamicBatching = normalsTextureSettings.enableDynamicBatching;
                drawSettings.enableInstancing = normalsTextureSettings.enableInstancing;
                drawSettings.overrideMaterial = normalsMaterial;

                DrawingSettings occluderSettings = drawSettings;
                occluderSettings.overrideMaterial = occludersMaterial;
                context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filteringSettings);
                //Schedules the drawing of a set of visible objects, and optionally overrides the GPU's render state.
                //눈에 보이는 객체 집합의 그리기를 예약하고, 선택적으로 GPU의 렌더 상태를 재정의합니다
                context.DrawRenderers(renderingData.cullResults, ref occluderSettings, ref occluderFilteringSettings);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(normals.id);
        }

    }

    //_PerObjectOutlineColorTexture
    private class PerObjectOutlineColorTexturePass : ScriptableRenderPass
    {

        private ViewSpaceNormalsTextureSettings normalsTextureSettings;
        private FilteringSettings filteringSettings;
        private FilteringSettings filteringSettings_Boss;
        private FilteringSettings filteringSettings_obj1;
        private FilteringSettings filteringSettings_obj2;

        private readonly List<ShaderTagId> shaderTagIdList;
        private readonly Material perObjectDataMaterial;
        private readonly Material perObjectDataMaterial_Boss;
        private readonly Material perObjectDataMaterial_obj1;
        private readonly Material perObjectDataMaterial_obj2;

        private readonly RenderTargetHandle perObjectData;

        public PerObjectOutlineColorTexturePass(RenderPassEvent renderPassEvent, 
            LayerMask layerMask, LayerMask outlinesLayerMask_Boss, LayerMask outlinesLayerMask_obj1, LayerMask outlinesLayerMask_obj2,
            ViewSpaceNormalsTextureSettings settings)
        {
            this.renderPassEvent = renderPassEvent;
            this.normalsTextureSettings = settings;
            filteringSettings = new FilteringSettings(RenderQueueRange.opaque, layerMask);
            filteringSettings_Boss = new FilteringSettings(RenderQueueRange.opaque, outlinesLayerMask_Boss);
            filteringSettings_obj1 = new FilteringSettings(RenderQueueRange.opaque, outlinesLayerMask_obj1);
            filteringSettings_obj2 = new FilteringSettings(RenderQueueRange.opaque, outlinesLayerMask_obj2);

            shaderTagIdList = new List<ShaderTagId> {
                new ShaderTagId("UniversalForward"),
                new ShaderTagId("UniversalForwardOnly"),
                new ShaderTagId("LightweightForward"),
                new ShaderTagId("SRPDefaultUnlit")
            };

            perObjectData.Init("_PerObjectOutlineColorTexture");     //"_PerObjectOutlineColorTexture" = shaderProperty
            perObjectDataMaterial = new Material(Shader.Find("Hidden/UnlitColor"));
            perObjectDataMaterial.SetColor("_Color", Color.white);
            perObjectDataMaterial_Boss = new Material(Shader.Find("Hidden/UnlitColor"));
            perObjectDataMaterial_Boss.SetColor("_Color", Color.red);
            perObjectDataMaterial_obj1 = new Material(Shader.Find("Hidden/UnlitColor"));
            perObjectDataMaterial_obj1.SetColor("_Color", Color.green);
            perObjectDataMaterial_obj2 = new Material(Shader.Find("Hidden/UnlitColor"));
            perObjectDataMaterial_obj2.SetColor("_Color", Color.blue);


        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            RenderTextureDescriptor normalsTextureDescriptor = cameraTextureDescriptor;
            normalsTextureDescriptor.colorFormat = normalsTextureSettings.colorFormat;
            normalsTextureDescriptor.depthBufferBits = normalsTextureSettings.depthBufferBits;

            cmd.GetTemporaryRT(perObjectData.id, normalsTextureDescriptor, normalsTextureSettings.filterMode);
            //This creates a temporary render texture with given parameters, and sets it up as a global shader property with nameID.
            //이렇게 하면 지정된 매개변수로 임시 렌더 텍스처가 생성되고 nameID가 있는 전역 셰이더 속성으로 설정됩니다.
            ConfigureTarget(perObjectData.Identifier());  //Configures render targets for this render pass.이 렌더 패스에 대한 렌더 타겟을 구성
            ConfigureClear(ClearFlag.All, normalsTextureSettings.backgroundColor);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!perObjectDataMaterial)
            {
                Debug.Log("!perObjectDataMaterial");
                return;
            }
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler("PerObjectOutlineColorTextureCreation")))
            {
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                DrawingSettings drawSettings = CreateDrawingSettings(shaderTagIdList, ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
                drawSettings.perObjectData = normalsTextureSettings.perObjectData;
                drawSettings.enableDynamicBatching = normalsTextureSettings.enableDynamicBatching;
                drawSettings.enableInstancing = normalsTextureSettings.enableInstancing;
                drawSettings.overrideMaterial = perObjectDataMaterial;

                DrawingSettings drawSettings_Boss = drawSettings;
                drawSettings_Boss.overrideMaterial = perObjectDataMaterial_Boss;
                DrawingSettings drawSettings_obj1 = drawSettings;
                drawSettings_obj1.overrideMaterial = perObjectDataMaterial_obj1;
                DrawingSettings drawSettings_obj2 = drawSettings;
                drawSettings_obj2.overrideMaterial = perObjectDataMaterial_obj2;

                context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filteringSettings);
                context.DrawRenderers(renderingData.cullResults, ref drawSettings_Boss, ref filteringSettings_Boss);
                context.DrawRenderers(renderingData.cullResults, ref drawSettings_obj1, ref filteringSettings_obj1);
                context.DrawRenderers(renderingData.cullResults, ref drawSettings_obj2, ref filteringSettings_obj2);
                
                //Schedules the drawing of a set of visible objects, and optionally overrides the GPU's render state.
                //눈에 보이는 객체 집합의 그리기를 예약하고, 선택적으로 GPU의 렌더 상태를 재정의합니다
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(perObjectData.id);
        }

    }
    private class ScreenSpaceOutlinePass : ScriptableRenderPass
    {

        private readonly Material screenSpaceOutlineMaterial;

        RenderTargetIdentifier cameraColorTarget;

        RenderTargetIdentifier temporaryBuffer;
        int temporaryBufferID = Shader.PropertyToID("_TemporaryBuffer");

        public ScreenSpaceOutlinePass(RenderPassEvent renderPassEvent, ScreenSpaceOutlineSettings settings)
        {
            this.renderPassEvent = renderPassEvent;

            screenSpaceOutlineMaterial = new Material(Shader.Find("Hidden/Outlines00"));
            screenSpaceOutlineMaterial.SetColor("_OutlineColor", settings.outlineColor);
            screenSpaceOutlineMaterial.SetColor("_OutlineColor_Boss", settings.outlineColor_Boss);
            screenSpaceOutlineMaterial.SetColor("_OutlineColor_obj1", settings.outlineColor_obj1);
            screenSpaceOutlineMaterial.SetColor("_OutlineColor_obj2", settings.outlineColor_obj2);
            screenSpaceOutlineMaterial.SetFloat("_OutlineScale", settings.outlineScale);

            screenSpaceOutlineMaterial.SetFloat("_DepthThreshold", settings.depthThreshold);
            screenSpaceOutlineMaterial.SetFloat("_RobertsCrossMultiplier", settings.robertsCrossMultiplier);

            screenSpaceOutlineMaterial.SetFloat("_NormalThreshold", settings.normalThreshold);

            screenSpaceOutlineMaterial.SetFloat("_SteepAngleThreshold", settings.steepAngleThreshold);
            screenSpaceOutlineMaterial.SetFloat("_SteepAngleMultiplier", settings.steepAngleMultiplier);
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor temporaryTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            temporaryTargetDescriptor.depthBufferBits = 0;
            cmd.GetTemporaryRT(temporaryBufferID, temporaryTargetDescriptor, FilterMode.Bilinear);
            temporaryBuffer = new RenderTargetIdentifier(temporaryBufferID);

            cameraColorTarget = renderingData.cameraData.renderer.cameraColorTarget;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!screenSpaceOutlineMaterial)
                return;

            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler("ScreenSpaceOutlinesBoss")))
            {

                Blit(cmd, cameraColorTarget, temporaryBuffer);
                Blit(cmd, temporaryBuffer, cameraColorTarget, screenSpaceOutlineMaterial);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(temporaryBufferID);
        }

    }

    [SerializeField] private RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    [SerializeField] private LayerMask outlinesLayerMask;
    [SerializeField] private LayerMask outlinesLayerMask_Boss;
    [SerializeField] private LayerMask outlinesLayerMask_obj1;
    [SerializeField] private LayerMask outlinesLayerMask_obj2;
    [SerializeField] private LayerMask outlinesOccluderLayerMask;

    [SerializeField] private ScreenSpaceOutlineSettings outlineSettings = new ScreenSpaceOutlineSettings();
    [SerializeField] private ViewSpaceNormalsTextureSettings viewSpaceNormalsTextureSettings = new ViewSpaceNormalsTextureSettings();

    private ViewSpaceNormalsTexturePass viewSpaceNormalsTexturePass;
    private ScreenSpaceOutlinePass screenSpaceOutlinePass;
    private PerObjectOutlineColorTexturePass perObjectOutlineColorTexturePass;

    public override void Create()
    {
        if (renderPassEvent < RenderPassEvent.BeforeRenderingPrePasses)
            renderPassEvent = RenderPassEvent.BeforeRenderingPrePasses;

        //-1은 layermask everything
        viewSpaceNormalsTexturePass = new ViewSpaceNormalsTexturePass(renderPassEvent, -1, outlinesOccluderLayerMask, viewSpaceNormalsTextureSettings);
        perObjectOutlineColorTexturePass = new PerObjectOutlineColorTexturePass (renderPassEvent,
             outlinesLayerMask, outlinesLayerMask_Boss, outlinesLayerMask_obj1, outlinesLayerMask_obj2,
            viewSpaceNormalsTextureSettings);
        screenSpaceOutlinePass = new ScreenSpaceOutlinePass(renderPassEvent, outlineSettings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(viewSpaceNormalsTexturePass);
        renderer.EnqueuePass(perObjectOutlineColorTexturePass);
        renderer.EnqueuePass(screenSpaceOutlinePass);
    }

}
