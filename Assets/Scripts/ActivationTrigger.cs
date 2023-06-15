using UnityEngine;

public class ActivationTrigger : MonoBehaviour
{
    public GameObject objectToActivate;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectToActivate.SetActive(true);
        }
    }
}
