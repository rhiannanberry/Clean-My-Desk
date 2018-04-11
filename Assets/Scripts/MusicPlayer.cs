using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicPlayer : MonoBehaviour {
	public GameObject pauseButton, playButton;

	TextMeshProUGUI songDetails;
	AudioManager manager;
	// Use this for initialization
	void Start () {
		songDetails = GetComponentInChildren<TextMeshProUGUI>();
		manager = GameController.Instance.audioManager;
	}
	
	// Update is called once per frame
	void Update () {
		if (manager.currentSong.source.isPlaying) {
			UpdateSongDetails();
			if (playButton.activeSelf|| !pauseButton.activeSelf) {
				playButton.SetActive(false);
				pauseButton.SetActive(true);
			}
		} else if (!playButton.activeSelf || pauseButton.activeSelf){
			pauseButton.SetActive(false);
			playButton.SetActive(true);
		}
	}

	public void UpdateSongDetails() {
		Song s = manager.currentSong;
		songDetails.text = s.name + " - " + s.artist;
	}

	public void Play() {
		manager.PlaySong();
		playButton.SetActive(false);
		pauseButton.SetActive(true);
	}

	public void Pause() {
		manager.PauseSong();
		pauseButton.SetActive(false);
		playButton.SetActive(true);
	}

	public void Next() {
		manager.NextSong();
		UpdateSongDetails();
	}

	public void Prev() {
		manager.PrevSong();
		UpdateSongDetails();
	}
}
