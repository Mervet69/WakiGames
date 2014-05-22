using UnityEngine;
using System.Collections;
public class LoadScene : MonoBehaviour {
	public string siguienteEscena;
	public float tiempoEspera;
	void Start() {
		Invoke("Load", tiempoEspera);
	}
	
	void Load() {
		LevelManager.Load(siguienteEscena);
	}
}
