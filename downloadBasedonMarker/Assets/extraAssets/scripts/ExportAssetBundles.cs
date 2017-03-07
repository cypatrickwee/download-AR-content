#if  UNITY_STANDALONE_WIN || UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
/*public class ExportAssetBundles : Editor {
    [MenuItem("Assets/Build AssetBundle")]
    static void ExportResource()
    {
        string path = "Assets/AssetBundle/myAssetBundle.unity3d";
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,BuildTarget.Android);
    }
}*/
public class ExportAssetBundles : MonoBehaviour
{
    [MenuItem("Example/Build Asset Bundles")]
    static void BuildABs()
    {
        string path = "Assets/AssetBundle/";

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.Android);
    }
}
#endif
