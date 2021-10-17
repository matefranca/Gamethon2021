using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clear.Data;

namespace Clear.Managers
{
    public class PlayerManager : SingletonInstance<PlayerManager>
    {
        public GameData GameData { get; private set; }

        protected override void OnInitialize()
        {
            GameData = new GameData();
        }

        public void EndWave()
        {
            LevelUp();
            
        }

        public void LevelUp()
        {
            GameData.level++;
        }
    }
}