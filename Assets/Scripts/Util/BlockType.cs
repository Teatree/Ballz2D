using System.Collections.Generic;

public class BlockType {
    public string Family { get; set; }  //Laser, Block, Bomb, Pick
    public bool isCollidable { get; set; } 

    public BlockType(string fam, bool isColl) {
        this.Family = fam;
        this.isCollidable = isColl;
    }

    public static BlockType Block = new BlockType("Block", true);
    public static BlockType TriNW = new BlockType("Block", true);
    public static BlockType TriSW = new BlockType("Block", true);
    public static BlockType TriSE = new BlockType("Block", true);
    public static BlockType TriNE = new BlockType("Block", true);
    public static BlockType Bomb = new BlockType("Bomb", true);
    public static BlockType BombVertical = new BlockType("Bomb", true);
    public static BlockType BombHorisontal = new BlockType("Bomb", true);
    public static BlockType BombCross = new BlockType("Bomb", true);
    public static BlockType LaserVertical = new BlockType("Laser", false);
    public static BlockType LaserHorisontal = new BlockType("Laser", false);
    public static BlockType LaserCross = new BlockType("Laser", false);
    public static BlockType ExtraBall = new BlockType("Pick", false);
    public static BlockType Fountain = new BlockType("Pick", false);

    private static Dictionary<string, BlockType> typesMap;

    public static Dictionary<string, BlockType> TypeMap {
        get {
            if (typesMap == null) {
                typesMap = new Dictionary<string, BlockType>();
                typesMap.Add("ob", Block);
                typesMap.Add("NW", TriNW);
                typesMap.Add("NE", TriNE);
                typesMap.Add("SE", TriSE);
                typesMap.Add("SW", TriSW);
                typesMap.Add("BM", Bomb);
                typesMap.Add("BV", BombVertical);
                typesMap.Add("BH", BombHorisontal);
                typesMap.Add("BC", BombCross);
                typesMap.Add("LV", LaserVertical);
                typesMap.Add("LH", LaserHorisontal);
                typesMap.Add("LC", LaserCross);
                typesMap.Add("★★", ExtraBall);
                typesMap.Add("FF", Fountain);
            }
            return typesMap;
        }
    }

    public static IBehaviour getBehaviourByType(string _type) {
        switch (_type) {
            case "LH": {
                    return new LaserHorisontalBehaviour();
                }
            case "LV": {
                    return new LaserVerticalBehaviour();
                }
            case "LC": {
                    return new LaserCrossBehaviour();
                }
            case "BM": {
                    return new BombBehaviour();
                }
            case "BV": {
                    return new BombVerticalBehaviour();
                }
            case "BH": {
                    return new BombHorisontalBehaviour();
                }
            case "BC": {
                    return new BombCrossBehaviour();
                }
            case "ob": {
                    return new BlockBehaviour();
                }
            default: {
                    return new BlockBehaviour();
                }
        }
    }
}