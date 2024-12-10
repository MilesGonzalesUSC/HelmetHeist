using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] Animator fadeAnim;

    private void Awake( ) {
        fadeAnim = gameObject.GetComponent<Animator>();
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad( gameObject );
        } else {
            Destroy( gameObject );
        }
    }

    public void NextLevel(int Scene) {
        LoadLevel( Scene );
	}

    IEnumerator LoadLevel(int Scene) {
        fadeAnim.SetTrigger( "End" );
        yield return new WaitForSeconds( 1.5f );
        SceneManager.LoadSceneAsync( Scene );
        fadeAnim.SetTrigger( "Start" );
    }
}
