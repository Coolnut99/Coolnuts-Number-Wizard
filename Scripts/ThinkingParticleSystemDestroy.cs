using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinkingParticleSystemDestroy : MonoBehaviour {

	private ParticleSystem ps;
	private ParticleSystem.EmissionModule em;

	// Use this for initialization
	void Awake () {
		ps = GetComponent<ParticleSystem>();
		em = ps.emission;
		em.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void StartParticleSystem() {
		//var em = ps.emission;
		GetComponent<ParticleSystem>().enableEmission = true;
	}

	public void StopParticleSystem() {
		//var em = ps.emission;
		//ps.enableEmission = false;
		GetComponent<ParticleSystem>().enableEmission = false;
	}
}
