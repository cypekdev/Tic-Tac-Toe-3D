using UnityEngine;

public class Player
{
    public Material material;
    public string gameObjectTag;
    public string nickName;
    public bool isInGame;

    public Player(Material material, string gameObjectTag, string nickName)
    {
        this.material      = material;
        this.gameObjectTag = gameObjectTag;
        this.nickName      = nickName;
        this.isInGame      = true;
    }
}
