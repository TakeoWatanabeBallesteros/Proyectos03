using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    
    public float health { get; set; }
    public float maxHealth { get; }
    
    public Vector3 position { get; }

    public void TakeDamage(float damage);
    
}
