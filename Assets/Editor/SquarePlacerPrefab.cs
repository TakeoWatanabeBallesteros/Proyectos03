using UnityEngine;
using UnityEditor;

public class SquarePlacerPrefab : EditorWindow
{
    GameObject[] prefabs;
    int squareSize = 5;
    float spacing = 1f;
    Vector3 initialPosition = Vector3.zero;

    [MenuItem("Tools/Square Prefab Placer")]
    public static void ShowWindow()
    {
        GetWindow<SquarePlacerPrefab>("Square Prefab Placer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Square Prefab Placer", EditorStyles.boldLabel);

        // Using ScriptableObject target for drawing array in Inspector
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty prefabsProperty = so.FindProperty("prefabs");

        EditorGUILayout.PropertyField(prefabsProperty, true); // True means show children

        // We need to update the properties for each of those separately
        squareSize = EditorGUILayout.IntField("Square Size", squareSize);
        spacing = EditorGUILayout.FloatField("Spacing", spacing);
        initialPosition = EditorGUILayout.Vector3Field("Initial Position", initialPosition);

        so.ApplyModifiedProperties(); // Remember to apply modified properties

        if (GUILayout.Button("Place Prefabs"))
        {
            PlacePrefabs();
        }
    }

    void PlacePrefabs()
    {
        if (prefabs.Length == 0)
        {
            Debug.LogError("At least one prefab must be specified.");
            return;
        }

        for (int x = 0; x < squareSize; x++)
        {
            for (int z = 0; z < squareSize; z++)
            {
                Vector3 position = initialPosition + new Vector3(x * spacing, 0, z * spacing);

                // Choose a random prefab from the array
                GameObject chosenPrefab = prefabs[Random.Range(0, prefabs.Length)];

                GameObject newPrefab = (GameObject)PrefabUtility.InstantiatePrefab(chosenPrefab);
                newPrefab.transform.position = position;
            }
        }
    }
}

