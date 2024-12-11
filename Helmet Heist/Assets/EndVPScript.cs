using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndVPScript : MonoBehaviour {
	public VideoPlayer VP;

	public void Update( ) {
		if((VP.frame) > 0 && (VP.isPlaying == false)) {
			Application.Quit();
		}
	}
}
