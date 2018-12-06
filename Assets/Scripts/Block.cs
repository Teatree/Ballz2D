using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class Block : MonoBehaviour {

    private static Dictionary<string, BlockType> typesMap;

    public static Dictionary<string, BlockType> TypeMap {
        get {
            if (typesMap == null) {
                typesMap = new Dictionary<string, BlockType>();
                typesMap.Add("ob", BlockType.Block);
                typesMap.Add("NW", BlockType.TriNW);
                typesMap.Add("NE", BlockType.TriNE);
                typesMap.Add("SE", BlockType.TriSE);
                typesMap.Add("SW", BlockType.TriSW);
                typesMap.Add("BM", BlockType.Bomb);
                typesMap.Add("BV", BlockType.BombVertical);
                typesMap.Add("BH", BlockType.BombHorisontal);
                typesMap.Add("BC", BlockType.BombCross);
                typesMap.Add("LV", BlockType.LaserVertical);
                typesMap.Add("LH", BlockType.LaserHorisontal);
                typesMap.Add("LC", BlockType.LaserCross);
                typesMap.Add("st", BlockType.ExtraBall);
                typesMap.Add("★★", BlockType.ExtraBall);
                typesMap.Add("FF", BlockType.Fountain);
            }
            return typesMap;
        }
    }

    public int hitsRemaining = 5;

    private SpriteRenderer spriteRenderer;
    private TextMeshPro text;

    public BlockType _type;
    public IBehaviour _behaviour;

    public bool destroyed; 

    public int col;
    public int row;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshPro>();
        UpdateVisualState();
    }

    public void UpdateVisualState() {
        if (text != null) {
            text.SetText(hitsRemaining.ToString());
            spriteRenderer.color = Color.Lerp(Color.white, Color.red, hitsRemaining / 10f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        _behaviour.Update();
       // UpdateVisualState();
    }

    public void DestroySelf() {
        destroyed = true;
        Destroy(gameObject, 0.0000001f);
    }

    internal void Setup(string blockType, int hits, int row, int col) {
        this.row = row;
        hitsRemaining = hits;
        try {
            _type = TypeMap[blockType];
        } catch (KeyNotFoundException r) {
            Debug.Log(">> blockType not found = " + blockType);
        }

        if (_type == BlockType.LaserHorisontal) {
            _behaviour = new LaserBehaviour();
        } else {
            _behaviour = new BlockBehaviour();
        }
        _behaviour.setBlock(this);

        UpdateVisualState();
    }

    public void HitOnce() {
        hitsRemaining--;
    }

    public enum BlockType {
Block, TriNW, TriSW, TriSE, TriNE, Bomb, BombVertical, BombHorisontal, BombCross, LaserVertical, LaserHorisontal, LaserCross, ExtraBall, Fountain}
}