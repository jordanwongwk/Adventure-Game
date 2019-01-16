using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBGMScript : MonoBehaviour {

    [SerializeField] AudioClip battleBGM;
    [SerializeField] AudioClip victoryFanfare;
    [SerializeField] AudioClip victoryBGM;
    [SerializeField] AudioClip loseBGM;

    AudioSource masterAudioSource;

	// Use this for initialization
	void Start () {
        masterAudioSource = GetComponent<AudioSource>();
        masterAudioSource.loop = true;

        // if got adjust volume, adjust here
	}

    public void BeginBattleBGM()
    {
        masterAudioSource.clip = battleBGM;
        masterAudioSource.Play();
    }

    public void BeginVictoryBGM()
    {
        masterAudioSource.Stop();
        StartCoroutine(VictoryBGMCoroutine());
    }

    IEnumerator VictoryBGMCoroutine()
    {
        masterAudioSource.PlayOneShot(victoryFanfare);
        yield return new WaitForSeconds(victoryFanfare.length);
        masterAudioSource.clip = victoryBGM;
        masterAudioSource.Play();
    }

    public void BeginLosingBGM()
    {
        masterAudioSource.loop = false; // For this BGM only
        masterAudioSource.clip = loseBGM;
        masterAudioSource.Play();
    }
}
