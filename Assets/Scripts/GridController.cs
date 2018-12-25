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
        gc = GetComponent<GameController>();
        for (int i = 0; i < 18 - gc.currentLevel.emptyRowsCount; i++) {
            SpawnRowOfBlocks(true);
        }
        foreach (Block b in blocksSpawned) {
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
                    Block block = GetTheBlock(cells, rowsSpawned, i);
                    //block.gameObject.transform.localScale = new Vector3(block.gameObject.transform.localScale.x*blockScale, block.gameObject.transform.localScale.y*blockScale); // *NEW
                    block.Setup(cells[i].type, cells[i].life, rowsSpawned, i);
                    blocksSpawned.Add(block);
                    //Constants.BlockSize = block.GetComponent<RectTransform>().rect.width * Mathf.Abs(block.transform.localScale.x) * Mathf.Abs(block.transform.parent.localScale.x);
                    //Constants.ShiftToTheCenter *= 
                    //Debug.Log("Constants.BlockSize " + Constants.BlockSize);
                }
            }
            rowsSpawned++;
        }
        BallLauncher.canShoot = true;
        checkLastBlocksLine();
    }

    private void checkLastBlocksLine() {
        float lastRowSpawnedPos = 0;
        int lastRowSpawnedIndex = 0;
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
                //float newY = block.transform.GetComponent<RectTransform>().rect.height * block.transform.localScale.x;
                foreach (Block ob in obstaclesCoordinates) {
                    if (ob.col == block.col && ob.transform.position.y == newY) {
                        newY -= ob.transform.GetComponent<RectTransform>().rect.height;

                        break;
                    }
                }
                //block.transform.position = new Vector3(block.transform.position.x, newY, block.transform.position.z);
                block.gridRow++;
                block.transform.position = grid.transform.GetChild(block.gridRow).transform.GetChild(block.col).gameObject.transform.position;
                
                //Debug.Log("NewY: " + newY);
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
        return grid.transform.GetChild(0).transform.GetChild(col).gameObject.transform.position;
    }


    private Block GetTheBlock(List<CellData> cells, int row, int col) {
        Block block;
        switch (cells[col].type) {
            case "FF": {
                    block = Instantiate(fountainPrefab);
                    break;
                }
            case "★★": {
                    block = Instantiate(plusBallPrefab, GetPosition(row, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "LH": {
                    block = Instantiate(laserHorizontalPrefab, GetPosition(row, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "LV": {
                    block = Instantiate(laserVerticalPrefab, GetPosition(row, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "LC": {
                    block = Instantiate(laserCrossPrefab, GetPosition(row, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "BM": {
                    block = Instantiate(bombPrefab, GetPosition(row, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "BV": {
                    block = Instantiate(bombVerticalPrefab, GetPosition(row, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "BH": {
                    block = Instantiate(bombHorizontalPrefab, GetPosition(row, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "BC": {
                    block = Instantiate(bombCrossPrefab, GetPosition(row, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "ob": {
                    block = Instantiate(blockPrefab, GetPosition(row, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "os": {
                    block = Instantiate(obstaclePrefab, GetPosition(row, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "NW": {
                    block = Instantiate(triangleNWPrefab, GetPosition(row, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "SW": {
                    block = Instantiate(triangleSWPrefab, GetPosition(row, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "SE": {
                    block = Instantiate(triangleSEPrefab, GetPosition(row, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "NE": {
                    block = Instantiate(triangleNEPrefab, GetPosition(row, col), Quaternion.identity, scalingParent);
                    break;
                }
            default: {
                    block = Instantiate(blockPrefab);
                    break;
                }
        }
        //   block.transform.position = GetPosition(row, col);
        //block.gameObject.transform.SetParent(grid.transform.GetChild(row).gameObject.transform.GetChild(col).transform);
        //block.gameObject.transform.localPosition = new Vector3(0, 0, 0);

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
