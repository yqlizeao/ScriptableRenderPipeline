using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

public class UnlitPipeInstance : RenderPipeline {

    CommandBuffer _cb;


    public override void Dispose()
    {
        base.Dispose();
        if (_cb!=null)
        {
            _cb.Dispose();
            _cb = null;
        }
    }

    public override void Render(ScriptableRenderContext renderContext, Camera[] cameras)
    {
        base.Render(renderContext, cameras);
        if (_cb == null)
        {
            _cb = new CommandBuffer();
        }
        foreach (var camera in cameras)
        {
            renderContext.SetupCameraProperties(camera);
            _cb.name = "Setup";
            //显式将当前渲染目标设置为相机Backbuffer。
            _cb.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
            //设置渲染目标的颜色为相机背景色
            _cb.ClearRenderTarget(true, true, camera.backgroundColor);
            renderContext.ExecuteCommandBuffer(_cb);
            _cb.Clear();
            //绘制天空盒子，注意需要在ClearRenderTarget之后进行，不然颜色会被覆盖。
            renderContext.DrawSkybox(camera);

            //设置裁剪
            var culled = new CullResults();
            CullResults.Cull(camera, renderContext, out culled);

            //设置Renderer Settings
            //在构造的时候就需要传入Lightmode参数
            //使用Shader中指定光照模式为Always的pass !!!
            var ds = new DrawRendererSettings(camera, new ShaderPassName("Always"));
            ds.sorting.flags = SortFlags.None;

            //设置过滤
            var fs = new FilterRenderersSettings(true);
            //只绘制不透明物体
            fs.renderQueueRange = RenderQueueRange.opaque;
            //绘制所有层
            fs.layerMask = ~0;

            //绘制物体
            renderContext.DrawRenderers(culled.visibleRenderers, ref ds, fs);

            renderContext.Submit();
        }
    }
}
