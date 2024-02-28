using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FF7Transition : MonoBehaviour {

	[SerializeField] Camera _mainCamera;
	[SerializeField] Camera RTCamera;
	[SerializeField,Tooltip("The Camnera which renders the quad of resultant bloom.")] 
	Camera RTResultCamera;
	
	[SerializeField] ParticleSystem _particleSystem;
	private ParticleSystem.MainModule _psm_main;
	
	[SerializeField] Material transitionMaterial;
	
	[SerializeField,Tooltip("How much the Render Texture dimension is divided. The greater value will drop the image quality in exchange for performance."),Range(1,8)] 
	int downsampling = 2;
	
	[SerializeField,Tooltip("For previewing resultant Render Texture only. Do not change this value.")] 
	RenderTexture rtSource;
	
	[SerializeField,Tooltip("For previewing screen width value only. Do not change this value.")] 
	double width;
	[SerializeField,Tooltip("For previewing screen height value only. Do not change this value.")] 
	double height;
	
	void Awake () {
		if (_mainCamera == null) {
			_mainCamera = Camera.main;
		}
		if (RTCamera == null) {
			RTCamera = this.GetComponent<Camera>();
		}
		if (_particleSystem == null) {
			_particleSystem = this.GetComponent<ParticleSystem>();
		}
		_particleSystem.Stop();
		_psm_main = _particleSystem.main;
		
		RTCamera.enabled = false;
		RTResultCamera.enabled = false;
		
		width = Screen.width;
		height = Screen.height;
		
		float fov = 0.5f / Mathf.Tan(RTResultCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
		if (width > height) {
			transform.localPosition = new Vector3 (0f, 0f, fov);
			_psm_main.startSizeXMultiplier = (float)(width/height * 1 ) ;
			_psm_main.startSizeYMultiplier = 1f;
		} else if (height > width) {
			float hwRatio = (float)(height / width);
			transform.localPosition = new Vector3 (0f, 0f, fov * hwRatio);
			_psm_main.startSizeXMultiplier = 1f;
			_psm_main.startSizeYMultiplier = hwRatio * 1f;
		}
	}
	
	void OnEnable () {
		int _width = (int)(width / downsampling);
		int _height = (int)(height / downsampling);
		rtSource = RenderTexture.GetTemporary(_width, _height, 16, RenderTextureFormat.Default);
		
		RTCamera.targetTexture = rtSource;
		//RTResultCamera.depth = 100f;
		
		transitionMaterial.SetTexture("_MainTex", rtSource);
		
		RTCamera.enabled = true;
		//RTResultCamera.enabled = true;
		_particleSystem.Play();
		
		StartCoroutine("DisableMainCamera");
	}
	
	IEnumerator DisableMainCamera() {
		yield return new WaitForSeconds(0.35f);
//		_mainCamera.enabled = false;
		yield return null;
	}
	
	void OnDisable () {
		StopAllCoroutines();
		RTCamera.enabled = false;
		//RTResultCamera.enabled = false;
		_mainCamera.enabled = true;
		_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}
}