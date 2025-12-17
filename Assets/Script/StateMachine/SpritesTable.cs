using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "SpritesTable", menuName = "PlayingSpritesTable")]
public class SpritesTable : ScriptableObject
{
    public List<Sprites> SpritesList = new List<Sprites>();
    public Dictionary<string, Sprites> GetAllSpritesByRoleName;
    public Dictionary<string, Sprite[]> GetBehaviorBySpecialNameString;

    public void Init()
    {
        GetAllSpritesByRoleName = new Dictionary<string, Sprites>();
        GetBehaviorBySpecialNameString = new Dictionary<string, Sprite[]>();
        foreach (var AllSprites in SpritesList)
        {
            if (!GetAllSpritesByRoleName.ContainsKey(AllSprites.RoleName.ToString()))
            {
                GetAllSpritesByRoleName.Add(AllSprites.RoleName.ToString(), AllSprites);
            }
            foreach (var sprites in AllSprites.behaviorList)
            {
                string SpecialName = string.Concat(AllSprites.RoleName, sprites.RoleBehavior, sprites.BehaviorNumType);
                if (!GetBehaviorBySpecialNameString.ContainsKey(SpecialName))
                {
                    GetBehaviorBySpecialNameString.Add(SpecialName, sprites.RoleBehaviorSprites);
                }
            }
        }
    }
}


public enum RoleName
{
    Plyer,
    Skeleton

}


public enum RoleBehavior
{
    Idle,
    Walk,
    Run,
    Attack,
    Jump,
    Fall,
    Hit,
    Dead,
    Aim,
    Fire

}



[System.Serializable]
public class Sprites
{

    public RoleName RoleName;
    public List<RoleBehaviorListClass> behaviorList = new List<RoleBehaviorListClass>();

}

[System.Serializable]
public class RoleBehaviorListClass
{
    public RoleBehavior RoleBehavior;
    [Header("用于区分相同行为的不同动画")]
    public int BehaviorNumType;
    public Sprite[] RoleBehaviorSprites;
}
