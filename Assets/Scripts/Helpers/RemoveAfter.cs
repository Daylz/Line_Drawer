using UnityEngine;
using System.Collections;

public class RemoveAfter : MonoBehaviour
{
	public float delay = 1f;

	// Use this for initialization
	void Start () 
	{
		Destroy(gameObject, delay);
	}
}
