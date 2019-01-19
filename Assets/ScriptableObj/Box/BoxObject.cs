﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Scriptables", menuName = "Box", order = 1)]
public class BoxObject : ScriptableObject {
    public string objectName = "Box1";

    public ItemObject[] items;
    public float[] itemProbabilities;
}