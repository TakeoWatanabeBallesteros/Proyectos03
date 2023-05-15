using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class TransformRecorder : MonoBehaviour
{
    private static Dictionary<GameObject, TransformData> chairTransforms = new Dictionary<GameObject, TransformData>();
    private static string path;

  
    public static void RecordTransforms()
    {
        if (path == null)
            path = Application.persistentDataPath + "/TransformData.json";

        chairTransforms.Clear();

        foreach (GameObject chair in Selection.gameObjects)
        {
            chairTransforms[chair] = new TransformData(chair.transform.position, chair.transform.rotation, chair.transform.localScale);
        }

        string json = JsonUtility.ToJson(new TransformDataArray(chairTransforms.Values));
        File.WriteAllText(path, json);
    }

    public static void RestoreTransforms()
    {
        if (path == null)
            path = Application.persistentDataPath + "/TransformData.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            TransformDataArray transformDataArray = JsonUtility.FromJson<TransformDataArray>(json);

            int i = 0;
            foreach (GameObject chair in chairTransforms.Keys)
            {
                chair.transform.position = transformDataArray.items[i].position;
                chair.transform.rotation = transformDataArray.items[i].rotation;
                chair.transform.localScale = transformDataArray.items[i].scale;
                i++;
            }
        }
    }

    [System.Serializable]
    private class TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }

    [System.Serializable]
    private class TransformDataArray
    {
        public TransformData[] items;

        public TransformDataArray(IEnumerable<TransformData> items)
        {
            this.items = new List<TransformData>(items).ToArray();
        }
    }
}

public class MyEditorWindow : EditorWindow
{
    [MenuItem("Window/Transform Recorder")]
    public static void ShowWindow()
    {
        GetWindow<MyEditorWindow>("Transform Recorder");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Record Transforms"))
        {
            TransformRecorder.RecordTransforms();
        }

        if (GUILayout.Button("Restore Transforms"))
        {
            TransformRecorder.RestoreTransforms();
        }
    }
}
