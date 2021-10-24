using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clear.Managers;

[CreateAssetMenu(menuName = "Scriptables/ Level", fileName = "New Level")]
public class LevelSO : ScriptableObject
{
    public int initalLevel;

    public bool hasDialogue;
    public DialogueSO dialogueSO;

    [Space(10)]
    public int numberOfEnemies;
    public float timeBtwnSpawns;
    public EnemyType enemyType;

    public LevelSO(bool hasDialogue, DialogueSO dialogueSO, int numberOfEnemies, float timeBtwnSpawns, EnemyType enemyType)
    {
        this.hasDialogue = hasDialogue;
        this.dialogueSO = dialogueSO;
        this.numberOfEnemies = numberOfEnemies;
        this.timeBtwnSpawns = timeBtwnSpawns;
        this.enemyType = enemyType;
    }
}
