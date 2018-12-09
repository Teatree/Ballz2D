using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {

    private float THE_WIDTH =  0.4519416f;

    private GameController gc;

    [SerializeField]
    private Block blockPrefab;
    [SerializeField]
    private Block laserPrefab;
    [SerializeField]
    private Block bombPrefab;

    private int playWidth = 13;
    private int rowsSpawned;

    public static List<Block> blocksSpawned = new List<Block>();

    private void Start() { //OnLevelWasLoaded
        gc = GetComponent<GameController>();
        for (int i = 0; i < 18 - gc.currentLevel.emptyRowsCount; i++) {
            SpawnRowOfBlocks();
        }
    }

    public void SpawnRowOfBlocks() {

        RemoveOneTurnBlocks();

        //move blocks down one line
        float width = THE_WIDTH;
        foreach (var block in blocksSpawned) {
            if (block != null) {
                RectTransform rt = (RectTransform)block.transform;
                width = rt.rect.width * block.transform.localScale.y;
                block.transform.position = new Vector3(block.transform.position.x, block.transform.position.y - width, block.transform.position.z);
            }
        }

        //add new line
        List<CellData> cells = gc.currentLevel.rows[rowsSpawned].GetCells();
        for (int i = 0; i < playWidth; i++) {
            if (cells[i] != null && cells[i].type != "") {
                Block block;
                if (BlockType.TypeMap[cells[i].type].Family == "Laser") {
                    block = Instantiate(laserPrefab, GetPosition(i, width), Quaternion.identity);
                } else
                if (BlockType.TypeMap[cells[i].type].Family == "Bomb") {
                    block = Instantiate(bombPrefab, GetPosition(i, width), Quaternion.identity);
                }
                else {
                    block = Instantiate(blockPrefab, GetPosition(i, width), Quaternion.identity);
                }
                block.Setup(cells[i].type, cells[i].life, rowsSpawned, i);
                blocksSpawned.Add(block);
            }
        }

        rowsSpawned++;
    }

    public void RemoveOneTurnBlocks() {
        foreach (Block b in blocksSpawned) {
            if (b.wasHit) { 
                b.DestroySelf();
            }
        }
    }

    private Vector3 GetPosition(int i, float width) {
        Vector3 position = transform.position;
        //position += Vector3.right * i * distanceBetweenBlocks
        position = new Vector3(i * width - 2.731f, transform.position.y, transform.position.z);

        return position;
    }
}
