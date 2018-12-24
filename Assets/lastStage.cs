using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lastStage : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log(transform.childCount);
        int i = 0;

        //Now destroy them

        for (int j = 0; j < transform.childCount; j++) {
            //Debug.Log("transform.GetChild(" + j + ")" + transform.GetChild(j));

            List<GameObject> allChildren = new List<GameObject>(transform.GetChild(j).childCount);
            Debug.Log("allChildren.Count: " + allChildren.Count);
            for (int q = 0; q < transform.GetChild(j).childCount; q++) {
                allChildren.Add(transform.GetChild(j).transform.GetChild(q).gameObject);
                Debug.Log("allChildren: " + allChildren[q]);
            }
            allChildren.Sort((p1, p2) => p1.transform.position.x.CompareTo(p2.transform.position.x)); /*( x => ((character)x.transform.position.x)).ToList();*/
            for (int g = 0; g < allChildren.Count; g++) {
                allChildren[g].name = "col_" + g;
                allChildren[g].transform.SetSiblingIndex(g);
            }

            for (int s = 0; s < transform.GetChild(j).childCount; s++) {

                
                //transform.GetChild(j).transform.GetChild(s).gameObject.name = "col_" + s;
                //if (transform.GetChild(j).transform.GetChild(s).gameObject.name == "col_0") Destroy(transform.GetChild(j).transform.GetChild(s).gameObject);
                //if (transform.GetChild(j).transform.GetChild(s).GetComponent<SpriteRenderer>() != null) transform.GetChild(j).transform.GetChild(s).GetComponent<SpriteRenderer>().color = Color.green;
                //for(int p = 0; p < transform.GetChild(j).transform.GetChild(s).childCount; p++) {
                //    if (transform.GetChild(j).transform.GetChild(s).transform.GetChild(p).gameObject.name == "GameObject") Destroy(transform.GetChild(j).transform.GetChild(s).transform.GetChild(p).gameObject);
                //}
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
