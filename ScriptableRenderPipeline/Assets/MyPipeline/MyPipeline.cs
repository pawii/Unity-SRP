using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;


// RenderPipeline implements IRenderPipeline
public class MyPipeline : RenderPipeline
{
    public override void Render(ScriptableRenderContext renderContext, Camera[] cameras)
    {
        // Basic implementation checks if pipeline is valid,
        // if it's not - raise error
        base.Render(renderContext, cameras);

        foreach (var camera in cameras)
        {
            Render(renderContext, camera);
        }
    }

    public void Render(ScriptableRenderContext context, Camera camera)
    {
        ScriptableCullingParameters cullingParameters;
        if (!CullResults.GetCullingParameters(camera, out cullingParameters))
        {
            return;
        }
        CullResults cull = CullResults.Cull(ref cullingParameters, context);


        // setup camera properties like position, rotation, scaling, ect.
        context.SetupCameraProperties(camera);


        ClearCameraSettings();


        var drawSettings = new DrawRendererSettings(camera, new ShaderPassName("SRPDefaultUnlit"));
        var filterSettings = new FilterRenderersSettings(true)
        {
            renderQueueRange = RenderQueueRange.opaque
        };
        context.DrawRenderers(cull.visibleRenderers, ref drawSettings, filterSettings);


        // commands issue to the buffer, but not execute immediately
        context.DrawSkybox(camera);

        filterSettings.renderQueueRange = RenderQueueRange.transparent;
        context.DrawRenderers(cull.visibleRenderers, ref drawSettings, filterSettings);


        // execute buffer commands
        context.Submit();


        void ClearCameraSettings()
        {
            var buffer = new CommandBuffer() { name = camera.name };
            CameraClearFlags clearFlags = camera.clearFlags;
            buffer.ClearRenderTarget(
                (clearFlags & CameraClearFlags.Depth) != 0,
                (clearFlags & CameraClearFlags.Color) != 0,
                camera.backgroundColor
            );
            context.ExecuteCommandBuffer(buffer);
            buffer.Release();
        }
    }

}
