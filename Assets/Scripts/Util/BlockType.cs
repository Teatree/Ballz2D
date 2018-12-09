using System.Collections.Generic;

public class BlockType {
    public string Family { get; set; }  //Laser, Block, Bomb

    public BlockType(string fam) {
        this.Family = fam;
    }
    public static BlockType Block = new BlockType("Block");
    public static BlockType TriNW = new BlockType("Block");
    public static BlockType TriSW = new BlockType("Block");
    public static BlockType TriSE = new BlockType("Block");
    public static BlockType TriNE = new BlockType("Block");
    public static BlockType Bomb = new BlockType("Bomb");
    public static BlockType BombVertical = new BlockType("Bomb");
    public static BlockType BombHorisontal = new BlockType("Bomb");
    public static BlockType BombCross = new BlockType("Bomb");
    public static BlockType LaserVertical = new BlockType("Laser");
    public static BlockType LaserHorisontal = new BlockType("Laser");
    public static BlockType LaserCross = new BlockType("Laser");
    public static BlockType ExtraBall = new BlockType("Block");
    public static BlockType Fountain = new BlockType("Block");

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
                typesMap.Add("st", ExtraBall);
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