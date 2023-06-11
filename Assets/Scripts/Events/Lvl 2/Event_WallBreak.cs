using UnityEngine;

public class Event_WallBreak : MonoBehaviour
{
    public static Event_WallBreak Instance { get; private set; }

    public GameObject prefabToActivate;
    public GameObject prefabToDeactivate;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

//Add this to call the event: ObjectModifier.Instance.BreakWall();
