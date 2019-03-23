using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

        double prob = 0;
        int sumWeightsOfItems = BoxAd.itemWights.Take(BoxAd.itemWights.Length).Sum();

        for (int i = 0; i<BoxAd.items.Length; i++) {
            prob += (float)BoxAd.itemWights[i] / (float)sumWeightsOfItems;
            dic.Add((float)prob, BoxAd.items[i]);
        }

        float rand = Random.Range(0.0f, 1.0f);

        List<float> dicKeys = new List<float>(dic.Keys);

        if (rand < dicKeys[0]) return dic[dicKeys[0]];
        for (int y = 1; y < dic.Count-1; y++) {
            if(rand >= dicKeys[y-1] && rand < dicKeys[y]) {
                return dic[dicKeys[y]];
            }
        }

        var item = dic[dicKeys[dic.Count - 1]];

        return item;
    }

    public ItemObject GetBoxContents_BoxStars(int index) {
        BoxStars1 = PlayerController.Instance.starBoxes[PlayerController.player.numStarBoxesOpened];
        Dictionary<float, ItemObject> dic = new Dictionary<float, ItemObject>();

        double prob = 0;
        int sumWeightsOfItems = BoxStars1.itemWights.Take(BoxAd.itemWights.Length).Sum();

        for (int i = 0; i < BoxStars1.items.Length; i++) {
            prob += (float)BoxStars1.itemWights[i] / (float)sumWeightsOfItems;
            dic.Add((float)prob, BoxStars1.items[i]);
        }

        float rand = Random.Range(0.0f, 1.0f);

        List<float> dicKeys = new List<float>(dic.Keys);

        if (rand < dicKeys[0]) return dic[dicKeys[0]];
        for (int y = 1; y < dic.Count - 1; y++) {
            if (rand >= dicKeys[y - 1] && rand < dicKeys[y]) {
                return dic[dicKeys[y]];
            }
        }

        var item = dic[dicKeys[dic.Count - 1]];

        return item;
    }
}
