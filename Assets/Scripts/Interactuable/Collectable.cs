using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] float HP = 75f;
    float DamageTimer;
    public float delayTimer;
    public bool Destroyed;
    MeshRenderer texture;
    public GameObject Sparks;
    GameController GM;

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameController").GetComponent<GameController>();
        texture = GetComponent<MeshRenderer>();
        Destroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0 && !Destroyed)
        {
            HP = 0;
            Destroyed = true;
            texture.material.color = new Color(0f, 0f, 0f, 1f);
            Sparks.SetActive(true);
        }
        if (HP > 0 && DamageTimer > 0)
        {
            DamageTimer -= Time.deltaTime;
        }

    }
    public void TakeDamage(float DMG)
    {
        if (DamageTimer > 0)
        {
            return;
        }
        HP -= DMG;
        DamageTimer = delayTimer;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !Destroyed)
        {
            GM.AddCollectable();
            Destroy(gameObject);
        }
    }
}