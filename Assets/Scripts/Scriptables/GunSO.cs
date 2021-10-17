using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/ Gun", fileName = "New Gun")]
public class GunSO : ScriptableObject
{
    public float fireRate;
    public int gunDamage;
}
