using UnityEngine;

namespace Clear
{
	[System.Serializable]
	public class Sound
	{
		public string name;

		public AudioClip clip;

		[Range(0f, 1f)]
		public float volume = .75f;

		public bool loop = false;
		public bool isSFX = false;

		[HideInInspector]
		public AudioSource source;
	}
}