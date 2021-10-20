using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clear.Managers
{
    public class MenuManager : MonoBehaviour
    {
        private void Start()
        {
            if (!AudioManager.GetInstance().IsPlaying(GameConstants.MENU_SOUND_NAME))
                AudioManager.GetInstance().Play(GameConstants.MENU_SOUND_NAME);
    }

        public void PlayButton()
        {
            Debug.Log("Play");
            AudioManager.GetInstance().Stop(GameConstants.MENU_SOUND_NAME);
            SceneLoader.GetInstance().LoadScene(GameConstants.GAME_SCENE_NAME);
        }
    }
}