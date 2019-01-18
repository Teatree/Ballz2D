using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Preview : IPopup<Preview> {

    public Text LevelText;

    public Transform scalingParent;
    private int playWidth = 13;
    private int rowsSpawned;

    public static List<Block> blocksSpawned = new List<Block>();

    public void SpawnRowOfBlocks() {
        MoveOneRowDown(true);
        //add new line
        if (rowsSpawned < LevelController.Instance.currentLevel.rows.Count) {
            List<CellData> cells = LevelController.Instance.currentLevel.rows[rowsSpawned].GetCells();
            for (int i = 0; i < playWidth; i++) {
                if (cells[i] != null && cells[i].type != "") {
                    Block block = GetTheBlock(cells[i].type, rowsSpawned, i);
                    //block.gameObject.transform.localScale = new Vector3(block.gameObject.transform.localScale.x*blockScale, block.gameObject.transform.localScale.y*blockScale); // *NEW
                    block.Setup(cells[i].type, cells[i].life, rowsSpawned, i);
                    blocksSpawned.Add(block);
                }
            }
            rowsSpawned++;
        }

    }


    public Vector3 GetPosition(int row, int col) {
        return new Vector3();
    }



    private void MoveOneRowDown(bool moveObstacles) {
        //move blocks down one line
        foreach (var block in blocksSpawned) {
            if (block != null && !block.destroyed && (block._type != BlockType.Obstacle || moveObstacles)) {
                RectTransform rt = (RectTransform)block.transform;
                int newY = ++block.gridRow;
                block.transform.position = GetPosition(newY, block.col);
            }
        }
    }

    public Block GetTheBlock(string type, int row, int col) {
        Block block;

        switch (type) {
            case "FF": {
                    block = Instantiate(GridController.Instance.fountainPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "★★": {
                    block = Instantiate(GridController.Instance.plusBallPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "LH": {
                    block = Instantiate(GridController.Instance.laserHorizontalPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "LV": {
                    block = Instantiate(GridController.Instance.laserVerticalPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "LC": {
                    block = Instantiate(GridController.Instance.laserCrossPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "BM": {
                    block = Instantiate(GridController.Instance.bombPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "BV": {
                    block = Instantiate(GridController.Instance.bombVerticalPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "BH": {
                    block = Instantiate(GridController.Instance.bombHorizontalPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "BC": {
                    block = Instantiate(GridController.Instance.bombCrossPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "ob": {
                    block = Instantiate(GridController.Instance.blockPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "os": {
                    block = Instantiate(GridController.Instance.obstaclePrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "NW": {
                    block = Instantiate(GridController.Instance.triangleNWPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "SW": {
                    block = Instantiate(GridController.Instance.triangleSWPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "SE": {
                    block = Instantiate(GridController.Instance.triangleSEPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            case "NE": {
                    block = Instantiate(GridController.Instance.triangleNEPrefab, GetPosition(0, col), Quaternion.identity, scalingParent);
                    break;
                }
            default: {
                    throw new System.ArgumentException("Block type  " + type + " is not defined ");
                }
        }
        block.typeCode = type;

       // block.gameObject.transform.localScale = new Vector3(Mathf.Abs(grid.localScale.x) * 0.95f * Mathf.Sign(block.gameObject.transform.localScale.x), Mathf.Abs(grid.localScale.y) * 0.95f * Mathf.Sign(block.gameObject.transform.localScale.y));
        //Debug.Log("world cell pos" + grid.transform.GetChild(row).transform.GetChild(col).gameObject.transform.position + " block pos = " + block.transform.position);
        return block;
    }





    public override void OnClick_Close() {
        LevelController.ResumeGame();
        base.OnClick_Close();
    }

    void Start() {
        LevelText.text = "Level: " + (AllLevelsData.CurrentLevelIndex + 1).ToString();

        for (int i = 0; i < 18 - LevelController.Instance.currentLevel.rows.Count; i++) {
            SpawnRowOfBlocks();
            Debug.Log(">>>");
        }
    }
}
