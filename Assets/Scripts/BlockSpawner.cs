﻿using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {

    private float THE_WIDTH = 0.4519416f;

    private GameController gc;

    #region prefabs
    [Header("Block Prefabs")]
    [SerializeField]
    private Block obstaclePrefab;
    [SerializeField]
    private Block blockPrefab;
    [SerializeField]
    private Block triangleNWPrefab;
    [SerializeField]
    private Block triangleNEPrefab;
    [SerializeField]
    private Block triangleSEPrefab;
    [SerializeField]
    private Block triangleSWPrefab;
    [SerializeField]
    private Block laserHorizontalPrefab;
    [SerializeField]
    private Block laserVerticalPrefab;
    [SerializeField]
    private Block laserCrossPrefab;
    [SerializeField]
    private Block bombPrefab;
    [SerializeField]
    private Block bombCrossPrefab;
    [SerializeField]
    private Block bombVerticalPrefab;
    [SerializeField]
    private Block bombHorizontalPrefab;
    [SerializeField]
    private Block plusBallPrefab;
    [SerializeField]
    private Block fountainPrefab;
    #endregion

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
                //width = rt.rect.width * block.transform.localScale.y;
                block.transform.position = new Vector3(block.transform.position.x, block.transform.position.y - width, block.transform.position.z);
            }
        }

        //add new line
        List<CellData> cells = gc.currentLevel.rows[rowsSpawned].GetCells();
        for (int i = 0; i < playWidth; i++) {
            if (cells[i] != null && cells[i].type != "") {
                Block block;
                switch (cells[i].type) {
                    case "FF": {
                            block = Instantiate(fountainPrefab, GetPosition(i, width), Quaternion.identity);
                            break;
                        }
                    case "★★": {
                            block = Instantiate(plusBallPrefab, GetPosition(i, width), Quaternion.identity);
                            break;
                        }
                    case "LH": {
                            block = Instantiate(laserHorizontalPrefab, GetPosition(i, width), Quaternion.identity);
                            break;
                        }
                    case "LV": {
                            block = Instantiate(laserVerticalPrefab, GetPosition(i, width), Quaternion.identity);
                            break;
                        }
                    case "LC": {
                            block = Instantiate(laserCrossPrefab, GetPosition(i, width), Quaternion.identity);
                            break;
                        }
                    case "BM": {
                            block = Instantiate(bombPrefab, GetPosition(i, width), Quaternion.identity);
                            break;
                        }
                    case "BV": {
                            block = Instantiate(bombVerticalPrefab, GetPosition(i, width), Quaternion.identity);
                            break;
                        }
                    case "BH": {
                            block = Instantiate(bombHorizontalPrefab, GetPosition(i, width), Quaternion.identity);
                            break;
                        }
                    case "BC": {
                            block = Instantiate(bombCrossPrefab, GetPosition(i, width), Quaternion.identity);
                            break;
                        }
                    case "ob": {
                            block = Instantiate(obstaclePrefab, GetPosition(i, width), Quaternion.identity);
                            break;
                        }
                    case "NW": {
                            block = Instantiate(triangleNWPrefab, GetPosition(i, width), Quaternion.identity);
                            break;
                        }
                    case "SW": {
                            block = Instantiate(triangleSWPrefab, GetPosition(i, width), Quaternion.identity);
                            break;
                        }
                    case "SE": {
                            block = Instantiate(triangleSEPrefab, GetPosition(i, width), Quaternion.identity);
                            break;
                        }
                    case "NE": {
                            block = Instantiate(triangleNEPrefab, GetPosition(i, width), Quaternion.identity);
                            break;
                        }
                    default: {
                            block = Instantiate(blockPrefab, GetPosition(i, width), Quaternion.identity);
                            break;
                        }
                }

                block.Setup(cells[i].type, cells[i].life, rowsSpawned, i);
                blocksSpawned.Add(block);
            }
        }

        rowsSpawned++;
        Debug.Log("spawn a row");
        BallLauncher.canShoot = true;
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
        /// 2.731f is a shift to center the whole thing on the screen
        position = new Vector3(i * width - 2.731f, transform.position.y, transform.position.z);

        return position;
    }
}
