using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/ Gun", fileName = "New Gun")]
public class GunSO : ScriptableObject
{
    public string gunName;
    public float fireRate;
    public int gunDamage;
    public int price;
    public Sprite gunImage;
    public Projectile projectile;
    public int maxAmmo;
    public float reloadTime;
}

public enum Projectile
{
    bullet,
    missile,
    laser
}
