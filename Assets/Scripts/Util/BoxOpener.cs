using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpener : SceneSingleton<BoxOpener> {

    public BoxObject BoxAd;
    public BoxObject BoxDay;
    public BoxObject BoxStars;
    public BoxObject BoxShop1;
    public BoxObject BoxShop2;

	void Start () {
		
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
}
