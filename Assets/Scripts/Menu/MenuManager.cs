using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            AudioManager.GetInstance().Stop(GameConstants.MENU_SOUND_NAME);
            SceneManager.LoadScene(1);
        }
    }
}