using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Clear.Managers
{
    public class DialogueManager : SingletonInstance<DialogueManager>
    {
        [Header("Dialogue Skip Button")]
        [SerializeField]
        private Button skipButton;

        private bool dialogueEnabled;
        
        private Queue<string> sentences;

        private void Start()
        {
            UIManager.GetInstance().DisableSpaceBarSkip();

            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(SkipSentence);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && dialogueEnabled)
            {
                SkipSentence();
            }
        }

        public void SetDialogueSO(DialogueSO dialogueSO)
        {
            sentences = new Queue<string>();

            foreach (string sentence in dialogueSO.sentences)
            {
                sentences.Enqueue(sentence);
            }
        }

        public void OpenDialogue()
        {
            dialogueEnabled = true;
            UIManager.GetInstance().OpenDialogue();
            NextDialogue();
        }

        public void NextDialogue()
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            string currentSentence = sentences.Dequeue();
            UIManager.GetInstance().SetDialogueText(currentSentence);
        }        

        private void SkipSentence()
        {
            if (UIManager.GetInstance().IsSpaceBarSkipActive())
            {
                AudioManager.GetInstance().Play(GameConstants.BUTTON_SELECT_SOUND_NAME);
                UIManager.GetInstance().DisableSpaceBarSkip();
                NextDialogue();
            }
        }
       
        public void EndDialogue()
        {
            dialogueEnabled = false;
            UIManager.GetInstance().CloseDialogue();
            GameManager.GetInstance().StartLevel();
        }
    }
}