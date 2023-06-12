using UnityEngine;

public class Event_WallBreak : MonoBehaviour
{
    public GameObject prefabToActivate;
    public GameObject prefabToDeactivate;

    public GameObject eplosionPrefab;

    public void BreakWall()
    {
        if (prefabToActivate != null && prefabToDeactivate != null)
        {
            prefabToActivate.SetActive(true);
            prefabToDeactivate.SetActive(false);
        }
        else
        {
            Debug.LogError("Prefabs are not assigned properly.");
        }
    }
}
