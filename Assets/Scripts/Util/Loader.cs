using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {

    void Start () {
        LoadScene();
    }

    private void LoadScene() {
        SceneManager.LoadSceneAsync("Permanent");
    }

}
