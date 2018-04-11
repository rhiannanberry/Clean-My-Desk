using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Audio;

public class AudioManager : MonoBehaviour {

	public Song[] songs;

	[HideInInspector]
	public int currentSongIndex;

	[HideInInspector]
	public Song currentSong;

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
			songs[0].source.playOnAwake = false;
			UpdateSongAudioSource();
		}
	}

	public void Play (string name) {
		Song s = Array.Find(songs, song => song.name == name);
		s.source.Play();
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
	private void UpdateSongAudioSource() {
		currentSong.source = gameObject.GetComponent<AudioSource>();
		currentSong.source.clip = currentSong.clip;
		currentSong.source.volume = currentSong.volume * GameController.Instance.masterVolume * GameController.Instance.musicVolume;
		currentSong.source.pitch = currentSong.pitch;
	}
}
