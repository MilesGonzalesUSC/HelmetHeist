using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultDoor : MonoBehaviour
{
	public bool open;
	public GameObject Door;
	private Animator vaultAnim;

	public void Start( ) {
		open = false;
		vaultAnim = Door.GetComponent<Animator>();
	}
	public void Update( ) {
		if(open) {
			vaultAnim.SetTrigger("Open");
		}

	}
}
