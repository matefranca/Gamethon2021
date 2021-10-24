using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldBonusText : MonoBehaviour
{
    [Header("Gold Text.")]
    [SerializeField]
    private TMP_Text goldText;

    public void Init(string gold)
    {
        goldText.SetText("+" + gold);
    }
}
