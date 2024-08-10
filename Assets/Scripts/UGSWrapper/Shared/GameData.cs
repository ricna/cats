using System;
using UnityEngine;

public enum Map
{
     Deafult,
}
public enum GameMode
{
    Default,
    Tag,
}
public enum GameQueue
{
    Solo,
    Team,
}

[Serializable]
public class UserData
{
    public string UserName; 
    public string UserAuthId;
    public GameInfo UserGamePreferences;
}

[Serializable]
public class GameInfo
{
    public Map Map;
    public GameMode Mode;
    public GameQueue Queue;

    public string ToMultiplayQueue()
    {
        return "";
    }
}


public class GameData : MonoBehaviour
{

}
