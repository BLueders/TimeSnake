using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	void Awake(){
		Instance = this;
	}

	void Start(){
		Snake.Instance.CreateWithLength (1, 1, 6);
	}

}

