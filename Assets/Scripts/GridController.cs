using System.Collections.Generic;
using UnityEngine;

public class GridController : SceneSingleton<GridController> {

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
    public static int BlocksAmount;

    public static List<Block> blocksSpawned = new List<Block>();
    private List<Block> obstaclesCoordinates = new List<Block>();

    private void Start() { //OnLevelWasLoaded
        gc = GetComponent<GameController>();
        for (int i = 0; i < 18 - gc.currentLevel.emptyRowsCount; i++) {
            SpawnRowOfBlocks(true);
        }
        foreach (Block b in   blocksSpawned) {
            if (b._type == BlockType.Obstacle) {
                obstaclesCoordinates.Add(b);
            }
        }

        BlocksAmount = gc.currentLevel.GetBlocksAmount();
    }

    public void SpawnRowOfBlocks(bool moveObstacles) {
        RemoveOneTurnBlocks();
        MoveOneRowDown(moveObstacles);
        //add new line
        if (rowsSpawned < gc.currentLevel.rows.Count) {
            List<CellData> cells = gc.currentLevel.rows[rowsSpawned].GetCells();
            for (int i = 0; i < playWidth; i++) {
                if (cells[i] != null && cells[i].type != "") {
                    Block block = GetTheBlock(cells, i);
                    block.Setup(cells[i].type, cells[i].life, rowsSpawned, i);
                    blocksSpawned.Add(block);
                }
            }
            rowsSpawned++;
        }
        BallLauncher.canShoot = true;
        checkLastBlocksLine();
    }

    private void checkLastBlocksLine() {

        float lastRowSpawnedPos = 0;
        int lastRowSpawnedIndex= 0;
        for (int i = 0; i < blocksSpawned.Count; i++) {
            if (!blocksSpawned[i].destroyed && blocksSpawned[i]._type != BlockType.Obstacle) {
                lastRowSpawnedPos = blocksSpawned[i].transform.position.y;
                lastRowSpawnedIndex = blocksSpawned[i].row;
                break;
            }
        }
        if (rowsSpawned > 0 && lastRowSpawnedPos <= Constants.Warning_y && lastRowSpawnedPos > Constants.GameOver_y) {
            GameUIController.Instance.HandleWarning();
        }

        if (rowsSpawned > 0 && lastRowSpawnedPos <= Constants.GameOver_y) {
            GameController.isGameOver = true;
            Revive.RowToDestroyIndex = lastRowSpawnedIndex;
            Revive.RowToDestroyPosition = lastRowSpawnedPos;
            //  GameUIController.Instance.HandleGameOver();
            Warning.Instance.ShowWarning();
            return;
        }
    }

    private void MoveOneRowDown(bool moveObstacles) {
        //move blocks down one line
        foreach (var block in blocksSpawned) {
            if (block != null && (block._type != BlockType.Obstacle || moveObstacles)) {
                RectTransform rt = (RectTransform)block.transform;
                //width = rt.rect.width * block.transform.localScale.y;
                float newY = block.transform.position.y - Constants.BlockSize;
                foreach (Block ob in obstaclesCoordinates) {
                    if (ob.col == block.col && ob.transform.position.y == newY) {
                        newY -= Constants.BlockSize;
                        break;
                    }
                }
                block.transform.position = new Vector3(block.transform.position.x, newY , block.transform.position.z);
            }
        }
       // checkLastBlocksLine();
    }

    public void RemoveOneTurnBlocks() {
        foreach (Block b in blocksSpawned) {
            if (b.wasHit) {
                b.DestroySelf();
            }
        }
    }

    private Vector3 GetPosition(int i) {
        Vector3 position = transform.position;
        /// 2.731f is a shift to center the whole thing on the screen
        position = new Vector3(i * Constants.BlockSize - Constants.ShiftToTheCenter, transform.position.y, transform.position.z);
        return position;
    }

    private Block GetTheBlock(List<CellData> cells, int i) {
        Block block;
        switch (cells[i].type) {
            case "FF": {
                    block = Instantiate(fountainPrefab, GetPosition(i), Quaternion.identity);
                    break;
                }
            case "★★": {
                    block = Instantiate(plusBallPrefab, GetPosition(i), Quaternion.identity);
                    break;
                }
            case "LH": {
                    block = Instantiate(laserHorizontalPrefab, GetPosition(i), Quaternion.identity);
                    break;
                }
            case "LV": {
                    block = Instantiate(laserVerticalPrefab, GetPosition(i), Quaternion.identity);
                    break;
                }
            case "LC": {
                    block = Instantiate(laserCrossPrefab, GetPosition(i), Quaternion.identity);
                    break;
                }
            case "BM": {
                    block = Instantiate(bombPrefab, GetPosition(i), Quaternion.identity);
                    break;
                }
            case "BV": {
                    block = Instantiate(bombVerticalPrefab, GetPosition(i), Quaternion.identity);
                    break;
                }
            case "BH": {
                    block = Instantiate(bombHorizontalPrefab, GetPosition(i), Quaternion.identity);
                    break;
                }
            case "BC": {
                    block = Instantiate(bombCrossPrefab, GetPosition(i), Quaternion.identity);
                    break;
                }
            case "ob": {
                    block = Instantiate(blockPrefab, GetPosition(i), Quaternion.identity);
                    break;
                }
            case "os": {
                    block = Instantiate(obstaclePrefab, GetPosition(i), Quaternion.identity);
                    break;
                }
            case "NW": {
                    block = Instantiate(triangleNWPrefab, GetPosition(i), Quaternion.identity);
                    break;
                }
            case "SW": {
                    block = Instantiate(triangleSWPrefab, GetPosition(i), Quaternion.identity);
                    break;
                }
            case "SE": {
                    block = Instantiate(triangleSEPrefab, GetPosition(i), Quaternion.identity);
                    break;
                }
            case "NE": {
                    block = Instantiate(triangleNEPrefab, GetPosition(i), Quaternion.identity);
                    break;
                }
            default: {
                    block = Instantiate(blockPrefab, GetPosition(i), Quaternion.identity);
                    break;
                }
        }

        return block;
    }

    public bool DidIwin() {
        if (gc.currentLevel.rows.Count == rowsSpawned) {
            foreach (var b in blocksSpawned) {
                if (b != null && !b.destroyed) {
                    return false;
                }
            }
            Debug.Log("WOW! you win");
            return true;
        }
        else {
            return false;
        }
    }

//    public int GetBlocksAmount() {
//        int amount = 0;
//        foreach (RowData r in gc.currentLevel.rows) {
//            List<CellData> cells = r.GetCells();
//            for (int i = 0; i < playWidth; i++) {
//                if (cells[i] != null && cells[i].type != "" && ) {

//                }
//        }
//        return amount;
//    }
}
