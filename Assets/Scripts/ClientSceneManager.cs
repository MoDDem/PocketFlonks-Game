using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientSceneManager : MonoBehaviour {
	public static ClientSceneManager instance;

	public GameLocation location = GameLocation.None;
	public List<GameObject> movableObjects = new List<GameObject>();

	public delegate void SceneChanged(int id);
	public delegate void LocationChanged(GameLocation location);
	public event SceneChanged OnSceneChanged;
	public event LocationChanged OnLocationChanged;

	[Range(0, 100)]
	public int loadingProgress;
	[Range(0, 100)]
	public int unloadingProgress;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	public void ChangeGameLocation(GameLocation location) {
		ChangeGameScene((int) location);
	}

	public void ChangeGameScene(int sceneID) {
		foreach(var item in movableObjects)
			if(item != gameObject) item.SetActive(false);

		StartCoroutine(LoadSceneAsync(sceneID));
	}

	private IEnumerator LoadSceneAsync(int id) {
		AsyncOperation operation = SceneManager.LoadSceneAsync(id, LoadSceneMode.Additive);

		loadingProgress = Convert.ToInt32(operation.progress * 100);

		while(!operation.isDone)
			yield return null;

		loadingProgress = 100;
		Scene loadedScene = SceneManager.GetSceneByBuildIndex(id);
		foreach(var item in movableObjects)
			SceneManager.MoveGameObjectToScene(item, loadedScene);

		yield return StartCoroutine(UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex));

		SceneManager.SetActiveScene(loadedScene);
		foreach(var item in movableObjects)
			if(item != gameObject) item.SetActive(true);

		OnSceneChanged?.Invoke(id);
		OnLocationChanged?.Invoke((GameLocation) id);
	}

	private IEnumerator UnloadSceneAsync(int id) {
		AsyncOperation operation = SceneManager.UnloadSceneAsync(id);

		unloadingProgress = Convert.ToInt32(operation.progress * 100);

		while(!operation.isDone)
			yield return null;

		unloadingProgress = 100;
	}
}
