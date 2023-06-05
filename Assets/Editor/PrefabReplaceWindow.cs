using UnityEngine;
using UnityEditor;

public class PrefabReplacerWindow : EditorWindow
{
    GameObject oldPrefab;
    GameObject newPrefab;

    [MenuItem("Tools/Replace Prefab")]
    public static void ShowWindow()
    {
        GetWindow<PrefabReplacerWindow>("Replace Prefab");
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Replacer", EditorStyles.boldLabel);

        oldPrefab = (GameObject)EditorGUILayout.ObjectField("Old Prefab", oldPrefab, typeof(GameObject), false);
        newPrefab = (GameObject)EditorGUILayout.ObjectField("New Prefab", newPrefab, typeof(GameObject), false);

        if (GUILayout.Button("Replace Selected Prefabs"))
        {
            ReplaceSelectedPrefabs();
        }
    }

    void ReplaceSelectedPrefabs()
    {
        if (oldPrefab == null || newPrefab == null)
        {
            Debug.LogError("Both old and new prefabs must be specified.");
            return;
        }

        foreach (GameObject go in Selection.gameObjects)
        {
            PrefabAssetType prefabType = PrefabUtility.GetPrefabAssetType(go);
            if (prefabType == PrefabAssetType.Regular || prefabType == PrefabAssetType.Variant)
            {
                GameObject prefabRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(go);
                if (prefabRoot == null)
                    continue;

                GameObject rootPrefab = PrefabUtility.GetCorrespondingObjectFromSource(prefabRoot);
                if (rootPrefab == oldPrefab)
                {
                    GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(newPrefab);
                    newObject.transform.SetParent(prefabRoot.transform.parent, true);
                    newObject.transform.localPosition = prefabRoot.transform.localPosition;
                    newObject.transform.localRotation = prefabRoot.transform.localRotation;
                    newObject.transform.localScale = prefabRoot.transform.localScale;
                    DestroyImmediate(prefabRoot);
                }
            }
        }
    }
}

