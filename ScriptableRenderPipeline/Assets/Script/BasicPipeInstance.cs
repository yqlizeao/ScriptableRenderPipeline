using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

public class BasicPipeInstance : RenderPipeline {
    private Color m_ClearColor = Color.black;
    CommandBuffer _cb;
    public BasicPipeInstance(Color clearColor)
    {
        m_ClearColor = clearColor;
    }
    //这个函数在管线被销毁的时候调用。
    public override void Dispose()
    {
        base.Dispose();
        if (_cb != null)
        {
            _cb.Dispose();
            _cb = null;
        }
    }
    //这个函数在需要绘制管线的时候调用。
    public override void Render(ScriptableRenderContext renderContext, Camera[] cameras)
    {
        base.Render(renderContext, cameras);

        if (_cb == null)_cb = new CommandBuffer();

        foreach (var camera in cameras)
        {
            //将上下文设置为当前相机的上下文。
            //使用了SetupCameraProperties指令，则当前相机会被自动设置为渲染目标，而当前相机如果正好又用于显示给玩家看的话，那么渲染目标自然就是最终的屏幕Backbuffer了。
            renderContext.SetupCameraProperties(camera);
            _cb.ClearRenderTarget(true, true, m_ClearColor);
            renderContext.ExecuteCommandBuffer(_cb);
            _cb.Clear();//_cb.Release()涵盖了dispose方法会导致报错。
            renderContext.Submit();//所有的CommandBuffer最终都需要合并到ScriptableRenderContext中，并且在调用Submit方法的时候被一次性执行。
        }
        
    }

}
