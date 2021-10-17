using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/ Item", fileName = "New Item")]
public class ItemSO : ScriptableObject
{
    public string companyName;
    public int initialStockValue;
}
