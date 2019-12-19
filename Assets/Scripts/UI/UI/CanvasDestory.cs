using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDestory : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
	}

}
