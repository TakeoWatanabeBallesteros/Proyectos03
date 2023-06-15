using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class RandomCloneWindow : EditorWindow
{
    GameObject centerObject;

    [MenuItem("Tools/Random Clone")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow<RandomCloneWindow>("Random Clone");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select the Center Object", EditorStyles.boldLabel);
        centerObject = (GameObject)EditorGUILayout.ObjectField(centerObject, typeof(GameObject), true);

        if (GUILayout.Button("Clone Objects"))
        {
            RandomCloneObjects();
        }
    }

    void RandomCloneObjects()
    {
        List<GameObject> selectedObjects = new List<GameObject>(Selection.gameObjects);

        if (selectedObjects.Count > 0)
        {
            selectedObjects.Remove(centerObject);

            int numOfClones = Random.Range(3, 21); // Random number between 3 and 20.
            for (int i = 0; i < numOfClones; i++)
            {
                GameObject randomObject = selectedObjects[Random.Range(0, selectedObjects.Count)]; // Pick random object from selectedObjects

                GameObject clone = Instantiate(randomObject, centerObject.transform.position, randomObject.transform.rotation, centerObject.transform);

                // For random position around the original object.
                float randomX = Random.Range(-10.0f, 10.0f); 
                float randomY = Random.Range(0.0f, 10.0f); 
                float randomZ = Random.Range(-10.0f, 10.0f);
                clone.transform.localPosition = clone.transform.localPosition + new Vector3(randomX, randomY, randomZ);

                // For random rotation.
                float rotationX = Random.Range(0, 360);
                float rotationY = Random.Range(0, 360);
                float rotationZ = Random.Range(0, 360);
                clone.transform.eulerAngles = new Vector3(rotationX, rotationY, rotationZ);
            }
        }
        else
        {
            Debug.Log("No game objects selected. Please select objects to clone.");
        }
    }
}


