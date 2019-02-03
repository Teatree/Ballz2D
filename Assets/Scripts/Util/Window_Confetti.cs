using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_Confetti : SceneSingleton<Window_Confetti> {

    [SerializeField] private Transform pfConfetti;
    [SerializeField] private Color[] colorArray;

    private List<Confetti> confettiList;
    private float spawnTimer;
    private const float SPAWN_TIMER_MAX = 0.033f;
    public bool active;

    public void ReleaseConfetti() {
        active = true;
        confettiList = new List<Confetti>();
        SpawnConfetti();
    }

    private void Update() {

        if (confettiList != null && confettiList.Count > 0) {
            foreach (Confetti confetti in new List<Confetti>(confettiList)) {
                if (confetti.Update()) {
                    confettiList.Remove(confetti);
                }
            }
        }
        if (active) {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f) {
                spawnTimer += SPAWN_TIMER_MAX;
                int spawnAmount = Random.Range(1, 4);
                for (int i = 0; i < spawnAmount; i++) {
                    SpawnConfetti();
                }
            }
        }
    }

    private void SpawnConfetti() {
        float width = transform.GetComponent<RectTransform>().rect.width;
        float height = transform.GetComponent<RectTransform>().rect.height;
        Vector2 anchoredPosition = new Vector2(Random.Range(-width / 2f, width / 2f), height / 2f);
        Color color = colorArray[Random.Range(0, colorArray.Length)];
        Confetti confetti = new Confetti(pfConfetti, transform, anchoredPosition, color, -height / 2f);
        confettiList.Add(confetti);
    }

    private class Confetti {

        private Transform transform;
        private RectTransform rectTransform;
        private Vector2 anchoredPosition;
        private Vector3 euler;
        private float eulerSpeed;
        private Vector2 moveAmount;
        private float minimumY;

        public Confetti(Transform prefab, Transform container, Vector2 anchoredPosition, Color color, float minimumY) {
            this.anchoredPosition = anchoredPosition;
            this.minimumY = minimumY;
            transform = Instantiate(prefab, container);
            rectTransform = transform.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;

            rectTransform.sizeDelta *= Random.Range(.8f, 1.2f);

            euler = new Vector3(0, 0, Random.Range(0, 360f));
            eulerSpeed = Random.Range(150f, 300f);
            eulerSpeed *= Random.Range(0, 2) == 0 ? 1f : 1f;
            transform.localEulerAngles = euler;

            moveAmount = new Vector2(0, Random.Range(-50f, -150f));

            transform.GetComponent<Image>().color = color;
        }

        public bool Update() {
            anchoredPosition += moveAmount * Time.deltaTime;
            rectTransform.anchoredPosition = anchoredPosition;

            euler.z += eulerSpeed * Time.deltaTime;
            transform.localEulerAngles = euler;
            Color c = transform.GetComponent<Image>().color;
            c.a = c.a - 0.005f;
            transform.GetComponent<Image>().color = c;

            if (anchoredPosition.y < minimumY) {
                Destroy(transform.gameObject);
                return true;
            } else {
                return false;
            }
        }


    }

}
