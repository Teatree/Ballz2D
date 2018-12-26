﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning : SceneSingleton<Warning> {

    public GameObject WarningBlock;
    private bool _warned;


    public void ShowWarning() {
       // Debug.Log("sjow warn " + _warned);
        if (!_warned) {
            List<GameObject> warnings = new List<GameObject>();
            for (int i = 0; i < 13; i++) {
                
                var d = Instantiate(WarningBlock);
                SpriteRenderer _t = d.transform.GetComponent<SpriteRenderer>();
                _t.color = new Color(1, 1, 1, 0);
                d.transform.position = GetWarningPosition(i);
                warnings.Add(d);
               // Debug.Log("sjow warn " + _t.color);
            }
            StartCoroutine(WarnBlockAni(warnings, 0.2f));
            _warned = true;
        }
    }

    private IEnumerator WarnBlockAni(List<GameObject> warnings, float fadeOutTime) {

        while (BallLauncher.canShoot) {

            for (float t = 0.01f; t < fadeOutTime / 4; t += Time.deltaTime) {
                foreach (GameObject w in warnings) {
                    SpriteRenderer _t = w.transform.GetComponent<SpriteRenderer>();
                    Color originalColor = _t.color;
                    _t.color = Color.Lerp(originalColor, Color.white, Mathf.Min(1, t * 4 / fadeOutTime));
                    yield return null;
                }
                yield return null;

            }


            for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime) {
                //for (int i = warnings.Count - 1; i >= 0; i--) {
                for (int i = 0; i < warnings.Count; i++) {

                    SpriteRenderer _t = warnings[i].transform.GetComponent<SpriteRenderer>();
                    Color originalColor = _t.color;
                    _t.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
                    yield return null;
                }
                yield return null;
            }
            yield return null;
        }

        foreach (var w in warnings) {
            Destroy(w, 0.0001f);
        }
        _warned = false;
    }

    private Vector3 GetWarningPosition(int i) {
        Vector3 position = transform.position;
        /// 2.731f is a shift to center the whole thing on the screen
        position = new Vector3(
            GridController.Instance.grid.GetChild(Constants.Warning_y_grid_index+1).GetChild(i).transform.position.x,
            GridController.Instance.grid.GetChild(Constants.Warning_y_grid_index+1).transform.position.y, 
            transform.position.z + 1);
        return position;
    }
}
