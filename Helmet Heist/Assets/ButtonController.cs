using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {
	public Animator fadeAnim;
	
	public void StartGame( ) {
		SceneManager.LoadScene( 1 );
	}

	IEnumerator LoadLevel( ) {
		fadeAnim.SetTrigger( "End" );
		yield return new WaitForSeconds( 3f );
		SceneManager.LoadSceneAsync(1);
		fadeAnim.SetTrigger( "Start" );
	}

	public void QuitGame( ) {
		QuitGame();
	}

}

