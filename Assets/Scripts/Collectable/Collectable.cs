using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] float HP = 75f;
    private bool canBeDamaged;
    public float delayTimer;
    MeshRenderer texture;
    public GameObject Sparks;
    ItemManager itemsManager;
    public GameObject Luz;
    public Collider _collider;
    
    private PointsBehavior pointsBehavior;
    
    // Start is called before the first frame update
    void Start()
    {
        itemsManager = Singleton.Instance.ItemsManager;
        texture = GetComponent<MeshRenderer>();
        canBeDamaged = true;
        pointsBehavior = Singleton.Instance.PointsManager;
    }

    public void TakeDamage(float DMG)
    {
        if(!canBeDamaged) return;
        StartCoroutine(DamageCoolDown());
        HP = Mathf.Clamp(HP -= DMG, 0, 75);
        if (HP != 0) return;
        texture.material.color = new Color(0f, 0f, 0f, 1f);
        Sparks.SetActive(true);
        pointsBehavior.RemovePointsCollectable();
        itemsManager.DeadCollectable();
        Destroy(Luz);
        enabled = false;
        _collider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            itemsManager.AddCollectable();
            Destroy(gameObject);
        }
    }

    private IEnumerator DamageCoolDown()
    {
        canBeDamaged = false;
        yield return new WaitForSeconds(delayTimer);
        canBeDamaged = true;
    }
}
