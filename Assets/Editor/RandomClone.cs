using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RandomClone : ScriptableObject
{
    [MenuItem("Tools/Random Clone", false, 10)]
    static void RandomCloneObjects()
    {
        List<GameObject> selectedObjects = new List<GameObject>(Selection.gameObjects);

        if (selectedObjects.Count > 0)
        {
            GameObject firstObj = selectedObjects[0];
            selectedObjects.RemoveAt(0); // remove the first selected object

            int numOfClones = Random.Range(3, 21); // Random number between 3 and 20.
            for (int i = 0; i < numOfClones; i++)
            {
                GameObject randomObject = selectedObjects[Random.Range(0, selectedObjects.Count)]; // Pick random object from selectedObjects

                GameObject clone = Instantiate(randomObject, firstObj.transform.position, randomObject.transform.rotation, firstObj.transform);

                // For random position around the original object.
                float randomX = Random.Range(-3.0f, 3.0f);
                float randomY = Random.Range(-3.0f, 3.0f);
                float randomZ = Random.Range(-3.0f, 3.0f);
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


