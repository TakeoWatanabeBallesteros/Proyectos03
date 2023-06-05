using UnityEditor;
using UnityEngine;

public class ObjectRotation : EditorWindow
{
    [MenuItem("Tools/Rotate Selected")]
    public static void ShowWindow()
    {
        GetWindow<ObjectRotation>("Rotate Selected");
    }

    private void OnGUI()
    {
        GUILayout.Label("Randomly Rotate Selected", EditorStyles.boldLabel);

        if (GUILayout.Button("Rotate Selected Objects"))
        {
            RotateSelectedObjects();
        }
    }

    private void RotateSelectedObjects()
    {
        foreach (GameObject selected in Selection.gameObjects)
        {
            selected.transform.rotation = Quaternion.Euler(
                UnityEngine.Random.Range(0, 360),
                UnityEngine.Random.Range(0, 360),
                UnityEngine.Random.Range(0, 360));
        }
    }
}
