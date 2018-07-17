using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

public class BaseLightAssetPipe : RenderPipelineAsset
{
    protected override IRenderPipeline InternalCreatePipeline()
    {
        return new BaseLightPipeInstance();
    }
}
