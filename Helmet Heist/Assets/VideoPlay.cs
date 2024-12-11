using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoPlay : MonoBehaviour
{
	public VideoPlayer VP;

	public void Update( ) {
		if((VP.frame)> 0 && (VP.isPlaying == false)) {
			SceneManager.LoadScene(2);
		}
	}
}
