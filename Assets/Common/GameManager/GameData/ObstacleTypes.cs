namespace Assets.Scripts.GameData
{
    public enum ObstacleTypes
    {
        SmallBlock,
        MiddleBlock,
        BigBlock,
        Tunnel,
    
        // can get out by jumping
        Pit,
        // falls into void => gameover
        Hole,
    }
}
