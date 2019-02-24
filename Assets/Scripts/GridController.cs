using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : SceneSingleton<GridController> {

    private LevelController gc;
    public Transform scalingParent;
    public Transform grid;
    [Header("Block Settings")]
    public float blockScale = 1;

    #region prefabs
    [Header("Block Prefabs")]
    [SerializeField]
    public Block obstaclePrefab;
    [SerializeField]
    public Block blockPrefab;
    [SerializeField]
    public Block triangleNWPrefab;
    [SerializeField]
    public Block triangleNEPrefab;
    [SerializeField]
    public Block triangleSEPrefab;
    [SerializeField]
    public Block triangleSWPrefab;
    [SerializeField]
    public Block laserHorizontalPrefab;
    [SerializeField]
    public Block laserVerticalPrefab;
    [SerializeField]
    public Block laserCrossPrefab;
    [SerializeField]
    public Block bombPrefab;
    [SerializeField]
    public Block bombCrossPrefab;
    [SerializeField]
    public Block bombVerticalPrefab;
    [SerializeField]
    public Block bombHorizontalPrefab;
    [SerializeField]
    public Block plusBallPrefab;
    [SerializeField]
    public Block fountainPrefab;
    #endregion

    private int playWidth = 13;
    private int rowsSpawned;
    public static int BlocksAmount;

    public static List<Block> blocksSpawned = new List<Block>();
    public List<BlockClone> blocksSpawnedSaved = new List<BlockClone>();

    private List<Block> obstaclesCoordinates = new List<Block>();
    public static bool doNotMoveRowDown;

    private void Start() { //OnLevelWasLoaded

        PlayServicesUI.Unlock1Achievement();

        blocksSpawned = new List<Block>();
        gc = GetComponent<LevelController>();
        for (int i = 0; i < 18 - gc.currentLevel.emptyRowsCount; i++) {
            SpawnRowOfBlocks(true);
        }
        foreach (Block b in blocksSpawned) {
            if (b._type == BlockType.Obstacle) {
                obstaclesCoordinates.Add(b);
            }
        }

        Constants.GameOver_y_grid_index = grid.childCount - 1;
        Constants.Warning_y_grid_index = grid.childCount - 2;

        BlocksAmount = gc.currentLevel.GetBlocksAmount();

        Revive.available = true; //Level just started Revive is available

        foreach (Block b in blocksSpawned) {
            b.transform.position = new Vector3(b.transform.position.x, b.transform.position.y, 10.791512f); // really weird fix for a really weird bug
            //Debug.Log("b.transform.position = " + b.transform.position);
        }
    }

    public void SpawnRowOfBlocks(bool moveObstacles) {
        if (!doNotMoveRowDown) {
            RemoveOneTurnBlocks();
            if (moveObstacles) {
                MoveOneRowDown(moveObstacles);
            } else {
                animateOneRowDown();
            }
            //add new line
            if (rowsSpawned < gc.currentLevel.rows.Count) {
                List<Block> blocksToBlink = new List<Block>();
                List<CellData> cells = gc.currentLevel.rows[rowsSpawned].GetCells();
                for (int i = 0; i < playWidth; i++) {
                    if (cells[i] != null && cells[i].type != "") {
                        Block block = GetTheBlock(cells[i].type, rowsSpawned, i);
                        //block.gameObject.transform.localScale = new Vector3(block.gameObject.transform.localScale.x*blockScale, block.gameObject.transform.localScale.y*blockScale); // *NEW
                        block.Setup(cells[i].type, cells[i].life, rowsSpawned, i);
                        blocksSpawned.Add(block);
                        blocksToBlink.Add(block);
                    }
                }
                //if(!moveObstacles) {
                //    StartCoroutine(BlinkBlink(blocksToBlink));
                //}
                rowsSpawned++;
            }
        }
        BallLauncher.canShoot = true;
    }

    private IEnumerator BlinkBlink(List<Block> blocks) {
        for (int i = 0; i < 4; i++) {
            foreach (Block b in blocks) {
                if (b.transform.gameObject.activeSelf) {
                    b.transform.gameObject.SetActive(false);
                } else {
                    b.transform.gameObject.SetActive(true);
                }
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void checkLastBlocksLine() {
        float lastRowSpawnedPos = 0;
        int lastRowSpawnedIndex = 0;
        int lastGridRow = 0;

        for (int i = 0; i < blocksSpawned.Count; i++) {
            if (!blocksSpawned[i].destroyed && blocksSpawned[i]._type != BlockType.Obstacle
                                && blocksSpawned[i].gridRow > lastGridRow) {
                lastRowSpawnedPos = blocksSpawned[i].transform.position.y;
                lastRowSpawnedIndex = blocksSpawned[i].row;
                lastGridRow = blocksSpawned[i].gridRow;
                // break;
            }
        }

        if (rowsSpawned > 0 && lastGridRow == Constants.Warning_y_grid_index) { //Show warnings ani
            BallLauncher.canShoot = true;
            Warning.Instance.ShowWarning();
            // return;
        }
        if (rowsSpawned > 0 && lastGridRow == Constants.GameOver_y_grid_index) { // Show gameOver popup
            LevelController.isGameOver = true;
            Revive.RowToDestroyIndex = lastRowSpawnedIndex;
            Revive.RowToDestroyPosition = lastRowSpawnedPos;
            GameUIController.Instance.HandleGameOver();
            return;
        }
    }

    private void MoveOneRowDown(bool moveObstacles) {
        //move blocks down one line
        Dictionary<Block, float> newYforBlocks = new Dictionary<Block, float>();
        foreach (var block in blocksSpawned) {
            if (block != null && !block.destroyed && (block._type != BlockType.Obstacle || moveObstacles)) {
                int newY = ++block.gridRow;
                ConsiderObstacles(block, ref newY);
                newYforBlocks.Add(block, newY);
                if (!isSpecialCase(block, newY)) {

                    block.transform.position = GetPosition(newY, block.col);
                    RectTransform rt = block.GetComponent<RectTransform>();
                    rt.position = new Vector3(rt.position.x, rt.position.y, 0);
                }
            }
        }
        // moveTo(newYforBlocks);
        checkLastBlocksLine();
        blocksSpawnedSaved = new List<BlockClone>();
    }

    public void animateOneRowDown() {
        Dictionary<Block, float> newYforBlocks = new Dictionary<Block, float>();
        foreach (var block in blocksSpawned) {
            if (block != null && !block.destroyed && (block._type != BlockType.Obstacle)) {
                int newY = ++block.gridRow;
                ConsiderObstacles(block, ref newY);
                newYforBlocks.Add(block, GetPosition(newY, block.col).y);
            }
        }
        moveTo(newYforBlocks);
        checkLastBlocksLine();
        blocksSpawnedSaved = new List<BlockClone>();
    }

    public void moveTo(Dictionary<Block, float> d) {
        StartCoroutine(moveDown(d));
    }

    private IEnumerator moveDown(Dictionary<Block, float> newYforBlocks) {
        bool shouldStop = false;
        while (!shouldStop) {
            foreach (Block b in newYforBlocks.Keys) {
                //:TODO 
                //if (!isSpecialCase(b, (int)newPosY)) { 
                float temp = Time.deltaTime * 0.7f;
                // b.transform.position = Vector3.Lerp(b.transform.position, new Vector3(b.transform.position.x, newYforBlocks[b]), temp);
                b.transform.position = new Vector3(b.transform.position.x, (b.transform.position.y - 0.05f));

                RectTransform rt = b.GetComponent<RectTransform>();
                rt.position = new Vector3(rt.position.x, rt.position.y, 0);
                // }
                if (b.transform.position.y <= newYforBlocks[b]) {
                    shouldStop = true;
                    b.transform.position = new Vector3(b.transform.position.x, newYforBlocks[b]);
                }
            }

            yield return null;
        }

    }
    private bool isSpecialCase(Block b, int newY) {
        if (newY == Constants.GameOver_y_grid_index) {
            if (!b._type.isCollidable) {
                if (b._type == BlockType.ExtraBall) {
                    ExtraBallBehaviour beh = b._behaviour as ExtraBallBehaviour;
                    beh.GetExtraBall();
                }
                b.DestroySelf();
            }
        }
        return false;
    }

    private void ConsiderObstacles(Block block, ref int newY) {
        foreach (Block ob in obstaclesCoordinates) { //ignore obstacles 
            if (ob.col == block.col && ob.gridRow == newY) {
                newY++;
                block.gridRow = newY;
                break;
            }
        }
    }

    public void RemoveOneTurnBlocks() {
        foreach (Block b in blocksSpawned) {
            if (b.wasHit) {
                b.DestroySelf();
            }
        }
    }

    public Vector3 GetPosition(int row, int col) {
        return grid.transform.GetChild(row).transform.GetChild(col).gameObject.transform.position;
    }

    public Block GetTheBlock(string type, int row, int col) {
        Block block;

        switch (type) {
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
                    throw new System.ArgumentException("Block type  " + type + " is not defined ");
                }
        }
        block.typeCode = type;

        block.gameObject.transform.localScale = new Vector3(Mathf.Abs(grid.localScale.x) * 0.95f * Mathf.Sign(block.gameObject.transform.localScale.x), Mathf.Abs(grid.localScale.y) * 0.95f * Mathf.Sign(block.gameObject.transform.localScale.y));
        //Debug.Log("world cell pos" + grid.transform.GetChild(row).transform.GetChild(col).gameObject.transform.position + " block pos = " + block.transform.position);
        return block;
    }

    public bool DidIwin() {
        if (gc.currentLevel.rows.Count == rowsSpawned) {
            foreach (var b in blocksSpawned) {
                if (b != null && !b.destroyed && b._type.isCollidable) {
                    return false;
                }
            }
            GameUIController.Instance.HandleWin();

            PlayServicesController.Instance.PublishScoreToLeaderBoard(LevelController.levelScore);
            return true;
        }
        else {
            return false;
        }
    }
}
