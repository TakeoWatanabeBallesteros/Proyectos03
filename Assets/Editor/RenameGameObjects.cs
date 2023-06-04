using UnityEngine;
using UnityEditor;

public class RenameGameObjects : EditorWindow
{
    string newName = "Enter name here";

    [MenuItem("Tools/Rename Game Objects")]
    static void Init()
    {
        RenameGameObjects window = (RenameGameObjects)EditorWindow.GetWindow(typeof(RenameGameObjects));
        window.Show();
    }

    void OnGUI()
    {
        newName = EditorGUILayout.TextField("New Name", newName);

        if (GUILayout.Button("Rename"))
        {
            GameObject[] gameObjects = Selection.gameObjects;

            // Sort by hierarchy order
            System.Array.Sort(gameObjects, (a, b) => (a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex())));

            // Rename with index
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].name = newName + i.ToString();
            }
        }
    }
}
