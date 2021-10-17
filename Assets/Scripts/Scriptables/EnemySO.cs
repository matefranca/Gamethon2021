using UnityEngine;
using Clear.Managers;

[CreateAssetMenu(menuName = "Scriptables/ Enemy", fileName = "New Enemy")]
public class EnemySO : ScriptableObject
{
    public EnemyType enemyType;
    public float moveSpeed;
    public float lifes;
}
