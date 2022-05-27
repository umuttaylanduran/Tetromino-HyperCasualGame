using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent( typeof(AudioSource) )]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [System.Serializable]
    public class Sound
    {
        public AudioClip clip;
        [HideInInspector]
        public int simultaneousPlayCount = 0;
    }

    public int maxSimultaneousSounds = 7;

    public Sound hitButton;
    public Sound hitItem;
    public Sound cameraRotate;
    public Sound createGround;
    public Sound scaleGround;
    public Sound playerHitObstacle;
    public Sound click;
    public Sound gameOver;

	public delegate void OnMuteStatusChanged(bool isMuted);
	public static event OnMuteStatusChanged MuteStatusChanged;

	private AudioSource audioSource;
	private const string MUTE_PREF_KEY = "MutePreference";
	private const int MUTED = 1;
	private const int UN_MUTED = -1;

    void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
		
	void Start()
	{
		audioSource = GetComponent<AudioSource>();

        SetMute(IsMuted());
	}

    public void PlaySound(Sound sound, bool autoScaleVolume = true, float maxVolumeScale = 1f)
	{
        StartCoroutine(CRPlaySound(sound, autoScaleVolume, maxVolumeScale));	
	}

    IEnumerator CRPlaySound(Sound sound, bool autoScaleVolume = true, float maxVolumeScale = 1f)
    {
        if (sound.simultaneousPlayCount >= maxSimultaneousSounds)
        {
            yield break;
        }

        sound.simultaneousPlayCount++;

        float vol = maxVolumeScale;

        if (autoScaleVolume && sound.simultaneousPlayCount > 0)
        {
            vol = vol/(float)(sound.simultaneousPlayCount);
        }

            
        audioSource.PlayOneShot(sound.clip, vol);

        float delay = sound.clip.length * 0.7f;

        yield return new WaitForSeconds(delay);

        sound.simultaneousPlayCount--;
    }

   
    public void PlayMusic(Sound music, bool loop = true)
	{
        audioSource.clip = music.clip;
        audioSource.loop = loop;
		audioSource.Play();
	}

   
	public void PauseMusic()
	{
		audioSource.Pause();
	}

   
	public void ResumeMusic()
	{
		audioSource.UnPause();
	}

   
    public void Stop()
    {
        audioSource.Stop();
    }

   
	public bool IsMuted()
	{
		return (PlayerPrefs.GetInt(MUTE_PREF_KEY, UN_MUTED) == MUTED);
	}

   
	public void ToggleMute()
	{
		
		bool mute = !IsMuted();

		if (mute)
		{
			
			PlayerPrefs.SetInt(MUTE_PREF_KEY, MUTED);

			if (MuteStatusChanged != null)
			{
				MuteStatusChanged(true);
			}
		}
		else
		{
			
			PlayerPrefs.SetInt(MUTE_PREF_KEY, UN_MUTED);

			if (MuteStatusChanged != null)
			{
				MuteStatusChanged(false);
			}
		}

		SetMute(mute);
	}

    void SetMute(bool isMuted)
    {
        audioSource.mute = isMuted;
    }
}
