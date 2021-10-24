using UnityEngine;
using Clear.Managers;

namespace Clear
{
    public class FootStep : MonoBehaviour
    {
        public void PlayFootStep()
        {
            if (AudioManager.GetInstance())
                AudioManager.GetInstance().Play(GameConstants.FOOTSTEP_SOUND_NAME);
        }
    }
}