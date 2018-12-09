using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class Block : MonoBehaviour {

    public int hitsRemaining = 5;
    public bool wasHit; //set true if should be deleted next move
    public bool destroyed;

    private SpriteRenderer spriteRenderer;
    private TextMeshPro text;

    public BlockType _type;
    public IBehaviour _behaviour;

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

    //private void OnCollisionEnter2D(Collision2D collision) {
    //    _behaviour.OnCollide();
    //}


    public void OnCollisionEnter2D() {
        _behaviour.OnCollide();
    }

    void OnCollisionExit2D(Collision2D other) {
        _behaviour.OnCollisionExit();
    }

    public void DestroySelf() {
        if (!destroyed) {
            destroyed = true;
            Destroy(gameObject, 0.0000001f);
        }
    }

    internal void Setup(string blockType, int hits, int row, int col) {
        this.row = row;
        this.col = col;
        hitsRemaining = hits;

        //Set block type
        try {
            _type = BlockType.TypeMap[blockType];
        } catch (KeyNotFoundException r) {
            Debug.Log(">> blockType not found = " + blockType);
        }

        //Set block behaviour
        _behaviour = BlockType.getBehaviourByType(blockType);
   
        _behaviour.setBlock(this);

        UpdateVisualState();
    }

    public void Hit() {
        _behaviour.GetOneLife();
    }
}

