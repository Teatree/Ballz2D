using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Block : MonoBehaviour {

    public static Vector2 faraway = new Vector2(-100, -100);

    public int hitsRemaining;
    public bool wasHit; //set true if should be deleted next move
    public bool destroyed;
    bool coroutineCalled;

    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    private TextMeshPro text;

    public string typeCode;
    public BlockType _type;
    public IBehaviour _behaviour;
    private BoxCollider2D boxCollider;

    public GameObject DeathParticle;
    public GameObject BombExplosion;
    public GameObject ScoreFeedbacker;

    public LayerMask WallsMask; // this might not be the best place for it, but it will have to do for now. This mask is necessary for lasers to know where the edges of their reach are. This value can be null

    public int col;
    public int row;
    public int gridRow;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshPro>();
        boxCollider = GetComponent<BoxCollider2D>();

        UpdateVisualState();
    }

    public void UpdateVisualState() {
        if (text != null && _type != null && (_type.Family.Equals("Block") || _type.Family.Equals("Bomb"))) {
            text.SetText(hitsRemaining.ToString());

            if (_type.Family.Equals("Block")) {
                spriteRenderer.color = Color.Lerp(Color.white, Color.green, hitsRemaining / 10f);
                StartCoroutine(BlockBlink(spriteRenderer.color, 1));
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        //Debug.Log("well done!");
    }

    protected IEnumerator BlockBlink(Color initColor, float waitTime) {
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.01f);
        GetComponent<SpriteRenderer>().color = initColor;
        yield return new WaitForSeconds(0.01f);
    }

    public void interactWithBall() {
        if (_behaviour == null) {
            Debug.LogError(_type);
        }
        _behaviour.OnCollide();
    }

    public void DestroySelf() {
        if (!destroyed) {
            if (_type.isCollidable) {
                CreateDeathParticle();
            }
            if (_type.Equals(BlockType.Bomb)) CreateBombExplosion();
            destroyed = true;
            transform.position = faraway;
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
    
    public void isCollidingNonCollidable(Vector3 pos) {
        if (!destroyed && !_type.isCollidable) {
            //Debug.Log("boxCollider.bounds = " + boxCollider + "  " + boxCollider.bounds.center);
            //Debug.Log("Block Pos: " + transform.position);
            if (boxCollider.bounds.Contains(pos)) {
                //Debug.Log("well done!");
                interactWithBall();
            }
        }
    }

    public void Hit() {
        _behaviour.LooseOneLife();
    }

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

    public void CreateScoreFeedbacker(int score) {
        if (!destroyed) {
            GameObject d = Instantiate(ScoreFeedbacker);
            d.transform.GetComponent<ScoreFeedbacker>().Score = score;
            d.transform.position = transform.position;
        }
    }

    public void GetLightningDamage() {
        this.hitsRemaining = hitsRemaining > 1 ? Mathf.RoundToInt(this.hitsRemaining / 2) : 1;
        UpdateVisualState();
    }

    public Block() {

    }

}

public class BlockClone {

    public string typeCode;
    public int col;
    public int row;
    public int gridRow;
    public BlockType _type;
    public IBehaviour _behaviour;
    public int hitsRemaining;

    public BlockClone(Block b) {
        this.typeCode = b.typeCode;
        this._type = b._type;
        this._behaviour = b._behaviour;
        this.hitsRemaining = b.hitsRemaining;
        this.col = b.col;
        this.row = b.row;
        this.gridRow = b.gridRow;
    }
}

