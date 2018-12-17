using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUIController : MonoBehaviour {

    public GameObject WarningBlock;
    private bool _warned;

    void Start() {

    }

    void Update() {

        if (Input.GetKeyDown("space")) {
            ShowWarning();
        }
            if (!GameController.IsGameStopped()) {
            if (BlockSpawner.LastRowSpawnedPos <= Constants.Warning_y && !_warned) {
                ShowWarning();
                return;
            }

            if (BlockSpawner.LastRowSpawnedPos <= Constants.GameOver_y) {
                GameController.GameOver();
                return;
            }
        }
    }

    public void Shop() {
        Debug.LogWarning("Shop!");
    }

    public void Pause() {
        if (GameController.IsGameStopped()) {
            GameController.ResumeGame();
        }
        else {
            Debug.LogWarning("Pause!");
            GameController.PauseGame();
        }
    }

    public void ShowWarning() {
        Debug.LogWarning("Warning!");
        List<GameObject> warnings = new List<GameObject>();
        for (int i = 0; i < 13; i++) {
            var d = Instantiate(WarningBlock);
            SpriteRenderer _t = d.transform.GetComponent<SpriteRenderer>();
            _t.color = Color.clear;
            d.transform.position = GetPosition(i);

            warnings.Add(d);
        }
        StartCoroutine(WarnBlockBlink(warnings, 0.5f));
        _warned = true;
    }

    private IEnumerator WarnBlockBlink(List<GameObject> warnings, float fadeOutTime) {
        for (float t = 0.01f; t < fadeOutTime/4; t += Time.deltaTime) {
            foreach (var w in warnings) {
                SpriteRenderer _t = w.transform.GetComponent<SpriteRenderer>();
                Color originalColor = _t.color;
                _t.color = Color.Lerp(originalColor, Color.white, Mathf.Min(1, t*4 / fadeOutTime));
                yield return null;
            }
        yield return null;

        }

        //Debug.Log("first loop ");
        //foreach (var w in warnings) {
        //    w.transform.GetComponent<SpriteRenderer>().color = Color.green;
        //}
        //yield return new WaitForSeconds(0.15f);
        //foreach (var w in warnings) {
        //    w.transform.GetComponent<SpriteRenderer>().color = Color.white;
        //}
        //yield return new WaitForSeconds(0.2f);
        //foreach (var w in warnings) {
        //    w.transform.GetComponent<SpriteRenderer>().color = Color.green;
        //}
        //yield return new WaitForSeconds(0.15f);
        //foreach (var w in warnings) {
        //    w.transform.GetComponent<SpriteRenderer>().color = Color.white;
        //}
        //yield return new WaitForSeconds(0.2f);

        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime) {
            for (int i = warnings.Count-1; i >= 0; i--) {

                SpriteRenderer _t = warnings[i].transform.GetComponent<SpriteRenderer>();
                Color originalColor = _t.color;
                _t.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t/ fadeOutTime));
               yield return null;
            }
            yield return null;
        }

        foreach (var w in warnings) {
            Destroy(w, 0.0001f);
        }
    }

    private Vector3 GetPosition(int i) {
        Vector3 position = transform.position;
        /// 2.731f is a shift to center the whole thing on the screen
        position = new Vector3(i * Constants.BlockSize - 2.731f, Constants.GameOver_y, transform.position.z);
        Debug.Log("pos = " + position);
        return position;
    }
}
