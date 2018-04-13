using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Audio;

public class AudioManager : MonoBehaviour {

	public Song[] songs;
	public Sound[] sounds;

	[HideInInspector]
	public int currentSongIndex;

	[HideInInspector]
	public Song currentSong;

	private AudioSource songSource;
	private AudioSource[] soundsSources;

	public void AudioSetup() {
		DontDestroyOnLoad(this.gameObject);
		for (int i = 0; i < songs.Length; i++) {
			if (i == 0) {
				songs[i].prev = songs[songs.Length - 1];
			} else {
				songs[i].prev = songs[i - 1];
			}
			if (i == songs.Length - 1) {
				songs[i].next = songs[0];
			} else {
				songs[i].next = songs[i + 1];
			}
		}
		if (songs.Length > 0) {
			currentSong = songs[0];
			songs[0].source = gameObject.AddComponent<AudioSource>();
			songSource = songs[0].source;
			songs[0].source.playOnAwake = false;
			UpdateSongAudioSource();
		}

		soundsSources = new AudioSource[sounds.Length];
		for (int i = 0; i < sounds.Length; i++) {
			soundsSources[i] = gameObject.AddComponent<AudioSource>();
			soundsSources[i].clip = sounds[i].clip;
			soundsSources[i].volume = sounds[i].volume * GameController.Instance.sfxVolume;
			soundsSources[i].pitch = sounds[i].pitch;
		}		
	}

	public Sound PlaySound(string name) {
		Sound s = Array.Find(sounds, sound => sound.clip.name == name);
		s.source.Play();
		return s;
	}

	public Song Play (string name) {
		Song s = Array.Find(songs, song => song.name == name);
        currentSong.source.Stop();
        currentSong.source = null;
        currentSong = s;
        UpdateSongAudioSource();
        PlaySong();
        return currentSong;
        //s.source.Play();
	}

	public Song NextSong() {
		currentSong.source.Stop();
		currentSong.source = null;
		currentSong = currentSong.next;
		UpdateSongAudioSource();
		PlaySong();
		return currentSong;
	}

	public Song PrevSong() {
		currentSong.source.Stop();
		currentSong.source = null;
		currentSong = currentSong.prev;
		UpdateSongAudioSource();
		PlaySong();
		return currentSong;
	}

	public void PlaySong() {
		currentSong.source.Play();
	}

	public void PauseSong() {
		currentSong.source.Pause();
	}

	public void UpdateVolume() {
		currentSong.source.volume = currentSong.volume * GameController.Instance.masterVolume * GameController.Instance.musicVolume;
		//whatever other sources we end up having get updated here too
	}
//multiply song audio but music audio slider when that gets set
	public void UpdateSongAudioSource() {
		currentSong.source = gameObject.GetComponent<AudioSource>();
		currentSong.source.clip = currentSong.clip;
		currentSong.source.volume = currentSong.volume * GameController.Instance.masterVolume * GameController.Instance.musicVolume;
		currentSong.source.pitch = currentSong.pitch;
	}

	public void UpdateSoundAudioSource(GameObject src, Sound s) {
		s.source = src.GetComponent<AudioSource>();
		s.source.clip = s.clip;
		s.source.volume = s.volume * GameController.Instance.masterVolume * GameController.Instance.sfxVolume;
		s.source.pitch = s.pitch;
	}
}
