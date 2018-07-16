using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class MySRPCreate  {

    [MenuItem("自定义渲染管线/CreateBasicSRP")]
    public static void CreateBasicSRP()
    {
        var instance = ScriptableObject.CreateInstance<BasicAssetPipe>();
        AssetDatabase.CreateAsset(instance, "Assets/MyScriptableRenderPipeline.asset");
        GraphicsSettings.renderPipelineAsset = instance;
    }
    [MenuItem("自定义渲染管线/CreateUnlitSRP")]
    public static void CreateUnlitSRP()
    {
        var instance = ScriptableObject.CreateInstance<UnlitAssetPipe>();
        AssetDatabase.CreateAsset(instance, "Assets/UnlitScriptableRenderPipeline.asset");
        GraphicsSettings.renderPipelineAsset = instance;
    }

}
