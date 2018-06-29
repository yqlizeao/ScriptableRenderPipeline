using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

public class BasicPipeInstance : RenderPipeline {
    private Color m_ClearColor = Color.black;
    public BasicPipeInstance(Color clearColor)
    {
        m_ClearColor = clearColor;
    }
    public override void Render(ScriptableRenderContext renderContext, Camera[] cameras)
    {
        base.Render(renderContext, cameras);

        var cmd = new CommandBuffer();
        cmd.ClearRenderTarget(true, true, m_ClearColor);
        renderContext.ExecuteCommandBuffer(cmd);
        cmd.Release();
        renderContext.Submit();
    }

}
