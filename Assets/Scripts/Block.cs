using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Block : MonoBehaviour {

    public int hitsRemaining = 5;
    public bool wasHit; //set true if should be deleted next move
    public bool destroyed;
    bool coroutineCalled;

    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    private TextMeshPro text;

    public BlockType _type;
    public IBehaviour _behaviour;

    public GameObject DeathParticle;
    public GameObject BombExplosion;
    public int col;
    public int row;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshPro>();
        UpdateVisualState();
    }

    public void UpdateVisualState() {
        if (text != null && _type != null && (_type.Family.Equals("Block") || _type.Family.Equals("Bomb"))) {
            text.SetText(hitsRemaining.ToString());

            if (_type.Family.Equals("Block")) {
                spriteRenderer.color = Color.Lerp(Color.white, Color.red, hitsRemaining / 10f);
                StartCoroutine(BlockBlink(spriteRenderer.color, 1));
            }
        }
    }

    protected IEnumerator BlockBlink(Color initColor, float waitTime) {
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.01f);
        GetComponent<SpriteRenderer>().color = initColor;
        yield return new WaitForSeconds(0.01f);
    }

    public void interactWithBall(Ball ball) {
        if (_behaviour == null) {
            Debug.LogError(_type.ToString());
        }
        _behaviour.OnCollide(ball);
    }

    public void DestroySelf() {
        if (!destroyed) {
            if (_type.isCollidable) CreateDeathParticle();
            if (_type.isCollidable && _type.Equals(BlockType.Bomb)) CreateBombExplosion();
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
        _behaviour.LooseOneLife();
    }

    //public BallLauncher GetBallLouncher() {
    //   return FindObjectOfType<BallLauncher>();
    //}

    public void Update() {
        _behaviour.Update();
    }

    public void CreateDeathParticle() {
        var d = Instantiate(DeathParticle);
        d.transform.GetComponent<BlockDeath>().DeathColor = spriteRenderer.color;
        d.transform.position = transform.position;
    }

    public void CreateBombExplosion() {
        if (BombExplosion != null) {
            var d = Instantiate(BombExplosion);
            d.transform.position = transform.position;
        }
    }
}

