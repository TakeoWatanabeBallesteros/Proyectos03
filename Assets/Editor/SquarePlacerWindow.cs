using UnityEngine;
using UnityEditor;

public class SquarePlacerWindow : EditorWindow
{
    GameObject prefab;
    int squareSize = 5;
    float spacing = 1f;

    [MenuItem("Tools/Prefab Placer")]
    public static void ShowWindow()
    {
        GetWindow<SquarePlacerWindow>("Prefab Placer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Placer", EditorStyles.boldLabel);

        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);
        squareSize = EditorGUILayout.IntField("Square Size", squareSize);
        spacing = EditorGUILayout.FloatField("Spacing", spacing);

        if (GUILayout.Button("Place Prefabs"))
        {
            PlacePrefabs();
        }
    }

    void PlacePrefabs()
    {
        if (prefab == null)
        {
            Debug.LogError("A prefab must be specified.");
            return;
        }

        for (int x = 0; x < squareSize; x++)
        {
            for (int z = 0; z < squareSize; z++)
            {
                Vector3 position = new Vector3(x * spacing, 0, z * spacing);
                GameObject newPrefab = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                newPrefab.transform.position = position;
            }
        }
    }
}
