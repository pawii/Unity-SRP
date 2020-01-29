using UnityEngine;
using UnityEngine.Experimental.Rendering;


[CreateAssetMenu(menuName = "Rendering/My Pipeline")]
public class MyPipelineAsset : RenderPipelineAsset
{
    // Returns IRenderPipeline, which settings rendering
    protected override IRenderPipeline InternalCreatePipeline()
    {
        return new MyPipeline();
    }
}
