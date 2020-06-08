using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum UIStatus {
	HomePanel,
	SettingsPanel,

	WelcomeForm,
	AuthForm,
	GoogleAuth,
	FacebookAuth,
	RegisterForm,
	NewsGrid
}

public class UIScript : MonoBehaviour
{
	public static UIScript instance;
	private UIStatus status;

	public GameObject connectionLostPanel;
	public GameObject canvasAnimations;
	private Animation _animation;

	private void Awake() {
		if(instance == null)
			instance = this;
		else if(instance != this)
			Destroy(gameObject);

		_animation = canvasAnimations.GetComponent<Animation>();
	}

	private IEnumerator OnAnimationEnd(Animation anim, string name, Action action) {
		while(anim.IsPlaying(name))
			yield return null;

		action.Invoke();
	}

	public void ChangeFormTo(string input) {
		UIStatus status = (UIStatus) Enum.Parse(typeof(UIStatus), input);
		
		if(this.status == status)
			return;
		this.status = status;

		switch(status) {
			case UIStatus.HomePanel:
				break;
			case UIStatus.SettingsPanel:
				break;
			case UIStatus.WelcomeForm:
				OpenWelcomeForm();
				break;
			case UIStatus.AuthForm:
				break;
			case UIStatus.GoogleAuth:
				break;
			case UIStatus.FacebookAuth:
				break;
			case UIStatus.RegisterForm:
				break;
			case UIStatus.NewsGrid:
				break;
			default:
				Debug.Log("Unexcpected");
				break;
		}
	}

	private void OpenWelcomeForm() {
		_animation["OpenWelcomeForm"].speed = .5f;
		_animation["OpenWelcomeForm"].time = 0;
		_animation.Play("OpenWelcomeForm");
	}

	/*
	public void OpenSplashScreen() {
		if(splashScreen.activeSelf)
			return;

		splashScreen.SetActive(true);

		var anim = splashScreen.GetComponent<Animation>();
		anim["SplashScreenAnim"].speed = 1;
		anim["SplashScreenAnim"].time = 0;
		anim.Play("SplashScreenAnim");
	}

	public void CloseSplashScreen() {
		if(!splashScreen.activeSelf)
			return;

		var anim = splashScreen.GetComponent<Animation>();
		anim["SplashScreenAnim"].speed = -1;
		anim["SplashScreenAnim"].time = anim["SplashScreenAnim"].length;
		anim.Play("SplashScreenAnim");

		StartCoroutine(OnAnimationEnd(
			anim,
			"SplashScreenAnim",
			() => { splashScreen.SetActive(false); }
		));
	}

	public void OpenLoginPanel() {
		if(loginAnimPanel.activeSelf)
			return;

		loginAnimPanel.SetActive(true);

		var anim = loginAnimPanel.GetComponent<Animation>();
		anim["LoginPanelAnim"].speed = 1;
		anim["LoginPanelAnim"].time = 0;
		anim.Play("LoginPanelAnim");
	}

	public void CloseLoginPanel() {
		if(!loginAnimPanel.activeSelf)
			return;

		var anim = loginAnimPanel.GetComponent<Animation>();
		anim["LoginPanelAnim"].speed = -1;
		anim["LoginPanelAnim"].time = anim["LoginPanelAnim"].length;
		anim.Play("LoginPanelAnim");

		StartCoroutine(OnAnimationEnd(
			anim, 
			"LoginPanelAnim", 
			() => { loginAnimPanel.SetActive(false); }
		));
	}

	public void SwitchFormPanel(int panelID) {
		if(formID == panelID)
			return;
		formID = panelID;

		var anim = loginAnimPanel.GetComponent<Animation>();
		if(panelID == 1) {
			anim["ChangeLocalForm"].speed = 1;
			anim["ChangeLocalForm"].time = 0;
			anim.Play("ChangeLocalForm");
		} else {
			anim["ChangeLocalForm"].speed = -1;
			anim["ChangeLocalForm"].time = anim["ChangeLocalForm"].length;
			anim.Play("ChangeLocalForm");
		}
	}

	public void SendFormData() {
		if(formID == 1) {
			if(string.IsNullOrWhiteSpace(regEmail.text) | string.IsNullOrWhiteSpace(regUsername.text) | string.IsNullOrWhiteSpace(regPassword.text))
				return;
			if(!regEmail.text.Contains("@") | regPassword.text.Length < 8)
				return;

			ClientSend.RegisterUser(regUsername.text, regEmail.text, regPassword.text);
		}else if (formID == 0) {
			if(!loginEmail.text.Contains("@") | loginPassword.text.Length < 8)
				return;

			ClientSend.LoginPlayer(loginEmail.text, loginPassword.text);
		}
	}

	public void ExitFromApp() {
		Application.Quit();
	}

	public void OpenWebsite(string url) {
		Process.Start(url);
	}

	public void JoinGameButton() {
		ClientSend.RequestJoinGame(PlayersManager.instance.gameObject.GetComponent<Player>());
	}
	*/
}
