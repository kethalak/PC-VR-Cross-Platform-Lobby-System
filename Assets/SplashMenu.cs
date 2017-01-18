using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;
using System.Collections;
using Valve.VR;
public class SplashMenu : MonoBehaviour {
	public Button vrButton;
	public Button pcButton;
	Valve.VR.EVRInitError VRerror;

	void Start () {
		vrButton.onClick.AddListener(loadVRScene);
		pcButton.onClick.AddListener(loadPCScene);
	}

	void loadVRScene(){
		StartCoroutine(SwitchToVR());
	}

	void loadPCScene(){
		SceneManager.LoadScene ("PC_Start");
	}

	IEnumerator SwitchToVR() {
		VRSettings.enabled = true;
		OpenVR.Init(ref VRerror, EVRApplicationType.VRApplication_Scene);
		yield return new WaitForSeconds(2);
		if(VRerror.ToString() != "None"){
			Debug.Log(VRerror);
			VRSettings.enabled = false;
			OpenVR.Shutdown();
			GameObject error = new GameObject("error");
			error.transform.SetParent(vrButton.transform.parent);
			error.transform.position = new Vector3(vrButton.transform.position.x, vrButton.transform.position.y - 50, vrButton.transform.position.z);
			Text errText = error.AddComponent<Text>();
			errText.rectTransform.sizeDelta = new Vector2(200,100);
			errText.alignment = TextAnchor.MiddleCenter;
			errText.font =  Resources.GetBuiltinResource<Font>("Arial.ttf");
			errText.text = "No VR device detected";
			errText.color = Color.red;
			}
			else{
			VRSettings.LoadDeviceByName("OpenVR");
			yield return new WaitForSeconds(2);
			VRSettings.enabled = true;
			SteamVR.enabled = true;
			SceneManager.LoadScene("VR_Start");
			}
	}
}