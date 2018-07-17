using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

public class BaseLightPipeInstance : RenderPipeline {

    CommandBuffer _cb;
    public override void Dispose()
    {
        base.Dispose();
        if (_cb != null)
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
        //通常我们不会将名称的字符串直接传给Unity，而是利用字符串映射，使用一个整数（int）来代表一个变量名，因为整数的比较比字符串快很多，这样有助于提升管线的效率
        var _LightDir = Shader.PropertyToID("_LightDir");
        var _LightColor = Shader.PropertyToID("_LightColor");
        var _CameraPos = Shader.PropertyToID("_CameraPos");

        foreach (var camera in cameras)
        {
            renderContext.SetupCameraProperties(camera);

            _cb.name = "SetUp";
            _cb.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
            _cb.ClearRenderTarget(true, true, camera.backgroundColor);

            Vector4 CameraPostition = new Vector4(
                camera.transform.localPosition.x,
                camera.transform.localPosition.y,
                camera.transform.localPosition.z,
                1.0f);
            _cb.SetGlobalVector(_CameraPos, camera.transform.localToWorldMatrix * CameraPostition);
            _cb.Clear();

            renderContext.DrawSkybox(camera);
            //1.裁剪
            var culled = new CullResults();
            CullResults.Cull(camera, renderContext, out culled);

            //裁剪会给出三个参数：
            //可见的物体列表：visibleRenderers
            //可见的灯光：visibleLights
            //可见的反射探针（Cubemap）：visibleReflectionProbes
            //所有的东西都是未排序的。

            var lights = culled.visibleLights;
            _cb.name = "RenderLights";
            //此处的循环只为了找主光源，只会做一次处理
            foreach (var light in lights)
            {
                if (light.lightType != LightType.Directional)
                {
                    continue;
                }
                //获取光源方向
                Vector4 pos = light.localToWorld.GetColumn(2);
                Vector4 LightDirection = new Vector4(-pos.x, -pos.y, -pos.z, 0);
                //獲取光源顔色
                Color LightColor = light.finalColor;
                _cb.SetGlobalVector(_LightDir, LightDirection);
                _cb.SetGlobalColor(_LightColor, LightColor);
                renderContext.ExecuteCommandBuffer(_cb);
                _cb.Clear();
                //2.过滤
                var fs = new FilterRenderersSettings(true);
                fs.renderQueueRange = RenderQueueRange.opaque;
                fs.layerMask = ~0;

                //3.drawRendererSetting
                var ds = new DrawRendererSettings(camera, new ShaderPassName("BaseLit"));

                renderContext.DrawRenderers(culled.visibleRenderers, ref ds, fs);
                break;
            }
            renderContext.Submit();

        }
    }
}
