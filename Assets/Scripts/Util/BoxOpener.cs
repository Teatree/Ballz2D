using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpener : SceneSingleton<BoxOpener> {

    public BoxObject BoxAd;
    public BoxObject BoxDay;
    public BoxObject BoxShop1;
    public BoxObject BoxShop2;

    private BoxObject BoxStars1;

    void Start () {
        BoxStars1 = new BoxObject();
    }
	
    public ItemObject GetBoxContents_BoxAd() {
        Dictionary<float, ItemObject> dic = new Dictionary<float, ItemObject>();

        float prob = 0;
        for (int i = 0; i<BoxAd.items.Length; i++) {
            prob += BoxAd.itemProbabilities[i];
            dic.Add(prob, BoxAd.items[i]);
        }

        float rand = Random.Range(0.0f, 1.0f);
        Debug.Log("rand : " + rand);

        List<float> dicKeys = new List<float>(dic.Keys);

        if (rand < dicKeys[0]) return dic[dicKeys[0]];
        for (int y = 1; y < dic.Count-1; y++) {
            if(rand >= dicKeys[y-1] && rand < dicKeys[y]) {
                return dic[dicKeys[y]];
            }
        }
        return dic[dicKeys[dic.Count - 1]];
    }

    public ItemObject GetBoxContents_BoxStars(int index) {
        BoxStars1 = PlayerController.Instance.starBoxes[PlayerController.player.numStarBoxesOpened];
        Dictionary<float, ItemObject> dic = new Dictionary<float, ItemObject>();

        float prob = 0;
        for (int i = 0; i < BoxStars1.items.Length; i++) {
            prob += BoxStars1.itemProbabilities[i];
            dic.Add(prob, BoxStars1.items[i]);
        }

        float rand = Random.Range(0.0f, 1.0f);
        Debug.Log("rand : " + rand);

        List<float> dicKeys = new List<float>(dic.Keys);

        if (rand < dicKeys[0]) return dic[dicKeys[0]];
        for (int y = 1; y < dic.Count - 1; y++) {
            if (rand >= dicKeys[y - 1] && rand < dicKeys[y]) {
                return dic[dicKeys[y]];
            }
        }
        return dic[dicKeys[dic.Count - 1]];
    }
}
