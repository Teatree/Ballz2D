﻿using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    private static Dictionary<string, bool> sceneStateTracker;
    public static SceneController sceneController;
    bool gameStart;
    public static string initScene = "";

    private static bool shouldShowRestartIntersticial;
    public static int shouldShowLevelIntersticial = 2; // counter of how many times player should load level before he is shown an ad
    public static int shouldShowLevelIntersticialcounter;

    public void Awake() {
        PlayerController.PlayerDataLoad();

        Application.targetFrameRate = 60;
        Debug.Log(" >>>>> scene " + initScene);
        if (!gameStart) {
            if (sceneStateTracker == null) {
                sceneStateTracker = new Dictionary<string, bool> {
                    { "GameScene", false },
                    { "MenuScene", false }
                };
            }
            sceneController = this;
            if (initScene != null && "GameScene".Equals(initScene)) {
                sceneStateTracker["GameScene"] = false;
                LoadGame();
            }
            else if (initScene != null && "MenuScene".Equals(initScene)) {
                sceneStateTracker["MenuScene"] = false;
                LoadMenu();
            } else {
                if (PlayerController.player.completedLvls != null && PlayerController.player.completedLvls.Count == 0 && AllLevelsData.CurrentLevelIndex == 0) {
                    LoadGame();
                }
                else {
                    LoadMenu();
                }
            }
            gameStart = true;
        }

        Debug.Log("I'm loadin services here !");

    }

    private void UnloadScene(string scene) {
        //Debug.Log("> u >" + sceneStateTracker[scene]);
        if (sceneStateTracker[scene]) {
            StartCoroutine(Unload(scene));
            sceneStateTracker[scene] = false;
        }
    }

    private void UnloadScene(int scene) {
        StartCoroutine(Unload(scene));
    }

    private void LoadScene(string scene) {
        if (!sceneStateTracker[scene]) {
            //Debug.Log("> l >" + scene);
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            sceneStateTracker[scene] = true;
        }
    }

    public void LoadGame() {
        BallLauncher.ExtraAdBalls = 0;
      
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

    private IEnumerator Unload(int scene) {
        yield return null;
        SceneManager.UnloadSceneAsync(scene);
    }

    public void RestartGame() {
        initScene = "GameScene";

        Debug.Log(">>>>  UnityAddsController.AdsLoaded > " + UnityAddsController.AdsLoaded);
        if (PlayerController.player.stars > 7 && !PlayerController.player.noAds && UnityAddsController.AdsLoaded) {
            if (shouldShowRestartIntersticial) {
                UnityAddsController.Instance.ShowEnterActionPhaseAfterRestartAd();
                shouldShowRestartIntersticial = false;
            }
            else {
                shouldShowRestartIntersticial = true;
                LoadMenu();
            }
        } else {
            LoadMenu();
        }

        
    }

    public void ReloadMenu() {
        initScene = "MenuScene";
        LoadGame();
      //  SceneManager.LoadScene("Permanent", LoadSceneMode.Single);
    }
}
