using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kid : MonoBehaviour
{
    private GameController GM;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        GM.AddChild();
        Destroy(gameObject);
    }
}
