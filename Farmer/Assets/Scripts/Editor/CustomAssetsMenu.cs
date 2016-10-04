using UnityEngine;
using UnityEditor;

public class CustomAssetsMenu
{
    [MenuItem("Assets/Create/Upgrade")]
    public static void CreateUpgrade()
    {
        ScriptableObjectUtility.CreateAsset<Upgrade>();
    }
}