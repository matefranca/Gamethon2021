using System;
using UnityEngine;

namespace Clear.Managers
{
	public class AudioManager : Singleton<AudioManager>
	{
		[Header("Sounds.")]
		[SerializeField]
		private Sound[] sounds;

		public bool Canplay { get; private set; } = true;

		public bool musicEnabled = true;

        protected override void OnInitialize()
        {
			foreach (Sound s in sounds)
			{
				s.source = gameObject.AddComponent<AudioSource>();
				s.source.clip = s.clip;
				s.source.loop = s.loop;
			}
		}

		public void Play(string sound)
		{
			if (!Canplay) return;

			Sound s = Array.Find(sounds, item => item.name == sound);
			if (s == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}

			s.source.volume = s.volume;
			s.source.Play();
		}

		public void Stop(string sound)
		{
			Sound s = Array.Find(sounds, item => item.name == sound);
			if (s == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}

			s.source.Stop();
		}

		public bool IsPlaying(string sound)
		{
			Sound s = Array.Find(sounds, item => item.name == sound);
			if (s == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return false;
			}

			return s.source.isPlaying;
		}

		public void SetCanPlay(bool active)
        {
			Canplay = active;
        }
	}
}