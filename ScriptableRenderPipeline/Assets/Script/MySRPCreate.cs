using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class MySRPCreate  {

    [MenuItem("Leo/CreateMySRP")]
    public static void CreateSRP()
    {
        var instance = ScriptableObject.CreateInstance<BasicAssetPipe>();
        AssetDatabase.CreateAsset(instance, "Assets/MyScriptableRenderPipeline.asset");
        GraphicsSettings.renderPipelineAsset = instance;
    }
}
