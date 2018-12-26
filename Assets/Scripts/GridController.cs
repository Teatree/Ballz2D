using System.Collections.Generic;
using UnityEngine;

public class GridController : SceneSingleton<GridController> {

    private GameController gc;
    public Transform scalingParent;
    public Transform grid;
    [Header("Block Settings")]
    public float blockScale = 1;


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
        blocksSpawned = new List<Block>();
        gc = GetComponent<GameController>();
        for (int i = 0; i < 18 - gc.currentLevel.emptyRowsCount; i++) {
            SpawnRowOfBlocks(true);
        }
        foreach (Block b in blocksSpawned) {
            if (b._type == BlockType.Obstacle) {
                obstaclesCoordinates.Add(b);
            }
        }

        Constants.GameOver_y = grid.childCount-1;
        Constants.Warning_y = grid.childCount-2;

        
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
                    Block block = GetTheBlock(cells, rowsSpawned, i);
                    //block.gameObject.transform.localScale = new Vector3(block.gameObject.transform.localScale.x*blockScale, block.gameObject.transform.localScale.y*blockScale); // *NEW
                    block.Setup(cells[i].type, cells[i].life, rowsSpawned, i);
                    blocksSpawned.Add(block);
                }
            }
            rowsSpawned++;
        }
        BallLauncher.canShoot = true;
        //checkLastBlocksLine();
    }

    private void checkLastBlocksLine() {
        float lastRowSpawnedPos = 0;
        int lastRowSpawnedIndex = 0;
        int lastGridRow = 0;
        for (int i = 0; i < blocksSpawned.Count; i++) {
            if (!blocksSpawned[i].destroyed && blocksSpawned[i]._type != BlockType.Obstacle) {
                lastRowSpawnedPos = blocksSpawned[i].transform.position.y;
                lastRowSpawnedIndex = blocksSpawned[i].row;
                lastGridRow = blocksSpawned[i].gridRow;
                break;
            }
        }
        if (rowsSpawned > 0 && lastGridRow == Constants.Warning_y) {
            GameUIController.Instance.HandleWarning();
        }

        if (rowsSpawned > 0 && lastGridRow == Constants.GameOver_y) {
            GameController.isGameOver = true;
            Revive.RowToDestroyIndex = lastRowSpawnedIndex;
            Revive.RowToDestroyPosition = lastRowSpawnedPos;
            GameUIController.Instance.HandleGameOver();
            return;
        }
    }

    private void MoveOneRowDown(bool moveObstacles) {
        //move blocks down one line
        foreach (var block in blocksSpawned) {
            if (block != null && (block._type != BlockType.Obstacle || moveObstacles)) {
                RectTransform rt = (RectTransform)block.transform;
                int newY = ++block.gridRow;
                foreach (Block ob in obstaclesCoordinates) {
                    if (ob.col == block.col && ob.gridRow == newY) {
                        newY++;
                        block.gridRow = newY;
                        break;
                    }
                }
              //  block.transform.position = grid.transform.GetChild(block.gridRow).transform.GetChild(block.col).gameObject.transform.position;
                block.transform.position = GetPosition(newY, block.col);
            }
        }
        checkLastBlocksLine();
    }

    public void RemoveOneTurnBlocks() {
        foreach (Block b in blocksSpawned) {
            if (b.wasHit) {
                b.DestroySelf();
            }
        }
    }

    private Vector3 GetPosition(int row, int col) {
        //Debug.Log("world pos" + grid.transform.GetChild(0).transform.GetChild(col).gameObject.transform.position);
        return grid.transform.GetChild(row).transform.GetChild(col).gameObject.transform.position;
    }


    private Block GetTheBlock(List<CellData> cells, int row, int col) {
        Block block;
        switch (cells[col].type) {
            case "FF": {
                    block = Instantiate(fountainPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "★★": {
                    block = Instantiate(plusBallPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "LH": {
                    block = Instantiate(laserHorizontalPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "LV": {
                    block = Instantiate(laserVerticalPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "LC": {
                    block = Instantiate(laserCrossPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "BM": {
                    block = Instantiate(bombPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "BV": {
                    block = Instantiate(bombVerticalPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "BH": {
                    block = Instantiate(bombHorizontalPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "BC": {
                    block = Instantiate(bombCrossPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "ob": {
                    block = Instantiate(blockPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "os": {
                    block = Instantiate(obstaclePrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "NW": {
                    block = Instantiate(triangleNWPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "SW": {
                    block = Instantiate(triangleSWPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "SE": {
                    block = Instantiate(triangleSEPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "NE": {
                    block = Instantiate(triangleNEPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            default: {
                    block = Instantiate(blockPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
        }

        block.gameObject.transform.localScale = new Vector3(Mathf.Abs(grid.localScale.x) * 0.95f * Mathf.Sign(block.gameObject.transform.localScale.x), Mathf.Abs(grid.localScale.y) * 0.95f * Mathf.Sign(block.gameObject.transform.localScale.y));
        //Debug.Log("world cell pos" + grid.transform.GetChild(row).transform.GetChild(col).gameObject.transform.position + " block pos = " + block.transform.position);
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

            //Maybe Change it later
            GameUIController.Instance.HandleWin();

            return true;
        }
        else {
            return false;
        }
    }

    //private Vector3 GetPosition(int i) { // Get Ready
    //    Vector3 position = transform.localPosition;
    //    /// 2.731f is a shift to center the whole thing on the screen
    //    position = new Vector3(i * Constants.BlockSize * transform.parent.localScale.x - Constants.ShiftToTheCenter * transform.parent.localScale.x, transform.position.y, transform.position.z);
    //    //Debug.Log("Constants.ShiftToTheCenter*transform.parent.localScale.x " + Constants.ShiftToTheCenter * transform.parent.localScale.x);
    //    return position;
    //}

}
