using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreFeedbacker : MonoBehaviour {
    public int Score { get; set; }
    public GameObject FeedbackCanvas; 

	// Use this for initialization
	void Start () {
        FeedbackCanvas.transform.position = transform.position;
        GameObject t = FeedbackCanvas.transform.GetChild(0).gameObject;
        t.transform.GetComponent<Text>().text = "" + Score;
        StartCoroutine(FadeScore(FeedbackCanvas, 0.9f));
    }

    public IEnumerator FadeScore(GameObject d, float fadeOutTime) {
        Text _t = d.transform.GetChild(0).gameObject.transform.GetComponent<Text>();
        Color originalColor = _t.color;
        yield return new WaitForSeconds(0.3f);
        for (float t = 0.001f; t < fadeOutTime; t += Time.deltaTime) {
            _t.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
            d.transform.position += new Vector3(0, 0.01f, 0); 
            yield return null;
        }
        Destroy(gameObject, 0.0000001f);
    }
}
