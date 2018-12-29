﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    private static Dictionary<string, bool> sceneStateTracker = new Dictionary<string, bool>();
    public static SceneController sceneController;
    bool gameStart;

    public AdmobController admob;

    public void Awake() {

        if (!gameStart) {

            sceneStateTracker = new Dictionary<string, bool>();
            sceneStateTracker.Add("GameScene", false);
            sceneStateTracker.Add("MenuScene", false);

            sceneController = this;
            LoadMenu();
            gameStart = true;
        }
    }

    private void UnloadScene(string scene) {
      //  Debug.Log("> u >" + sceneStateTracker[scene]);
        if (sceneStateTracker[scene]) {
           
            StartCoroutine(Unload(scene));
            sceneStateTracker[scene] = false;
        }
    }

    private void LoadScene(string scene) {
        if (!sceneStateTracker[scene]) {
            //Debug.Log("> l >" + scene);
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            sceneStateTracker[scene] = true;
        }
    }

    public void LoadGame() {
        UnloadScene("MenuScene");
        LoadScene("GameScene");
        
    }

    public void LoadMenu() {
        UnloadScene("GameScene");
        LoadScene("MenuScene");
        
    }

    public void UnloadGame() {
        UnloadScene("GameScene");
    }

    public void UnloadMenu() {
        UnloadScene("MenuScene");
    }

    private IEnumerator Unload(string scene) {
        yield return null;
        SceneManager.UnloadSceneAsync(scene);
    }
}
