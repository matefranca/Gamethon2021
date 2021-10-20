using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/ Stock Item", fileName = "New Stock Item")]
public class StockItemSO : ScriptableObject
{
    public string companyName;
    public int initialStockValue;
}
