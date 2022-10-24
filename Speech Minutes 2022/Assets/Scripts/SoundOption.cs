﻿using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;
 
public class SoundOption : MonoBehaviour {
 
	//　SoundOptionキャンバスを設定
	[SerializeField]
	private GameObject soundOptionCanvas;
	//　GameSoundShot
	[SerializeField]
	private AudioMixerSnapshot gameSoundShot;
	//　OptionSoundShot
	[SerializeField]
	private AudioMixerSnapshot optionSoundShot;
 
	[SerializeField]
	private AudioMixer audioMixer;
 
	void Update () {
		//4が押されたらUIをオン・オフ
		if(Input.GetKeyDown("l")) {
			soundOptionCanvas.SetActive(!soundOptionCanvas.activeSelf);
 
			if(soundOptionCanvas.activeSelf) {
				optionSoundShot.TransitionTo(0.01f);
			} else {
				gameSoundShot.TransitionTo(0.01f);
			}
		}
	}
	public void SetMaster(float volume) {
		audioMixer.SetFloat("MasterVol", volume);
	}
 
	public void SetBGM(float volume) {
		audioMixer.SetFloat("VoiceVol", volume);
	}
}
 