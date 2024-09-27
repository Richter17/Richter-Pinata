using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure
{
	public class SoundManager : MonoBehaviour
	{
		public static SoundManager Instance;
		
		[SerializeField] private AudioSource _audioSourcePrefab;

		private ObjectPool<AudioSource> _audioSourcePool;
		private Dictionary<AudioClip, AudioSource> _sourcesDict = new();

		private void Awake()
		{
			_audioSourcePool = new ObjectPool<AudioSource>(a => a.gameObject.SetActive(true),
				a => a.gameObject.SetActive(false),
				() => Instantiate(_audioSourcePrefab, transform));
			// Ensure there is only one instance of SoundManager
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public void PlaySFX(AudioClip clip, float volume = 1f)
		{
			var source = GetAudioSource(clip, volume);
			source.Play();
			StartCoroutine(ReturnToPoolWhenFinished(source));
		}
		
		public void PlayMusic(AudioClip clip, float volume = 1f)
		{
			var source = GetAudioSource(clip, volume);
			source.loop = true;
		}

		public void Stop(AudioClip clip)
		{
			if(!clip)
				return;
			if(!_sourcesDict.TryGetValue(clip, out var source))
				return;
			source.Stop();
			_audioSourcePool.Release(source);
		}

		private AudioSource GetAudioSource(AudioClip clip, float volume = 1f)
		{
			var source = _audioSourcePool.Get();
			source.clip = clip;
			source.volume = volume;
			_sourcesDict[clip] = source;
			return source;
		}

		private IEnumerator ReturnToPoolWhenFinished(AudioSource source)
		{
			yield return new WaitWhile(() => source.isPlaying);
			_audioSourcePool.Release(source);
			
		}
	}
}