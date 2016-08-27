using System.Collections.Generic;
using Assets.Scripts.Utils;
using SQLite4Unity3d;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using System;
using System.Linq;

public class Neuron
{
    private int? id;

    [PrimaryKey, AutoIncrement]
    public int? Id
    {
        get { return id; }
        set
        {
            id = value;
            Logic.neuronsCount++;
        }
    }

    public int? ParentId { get; set; }

    public string Self { get; set; }

    public int DoActionId
    {
        get; private set;
    }
    [Ignore]
    public DoAction DoAction
    {
        get
        {
            return (DoAction)DoActionId;
        }
        set
        {
            var doAction = value;
            DoActionId = (int)doAction;
        }
    }

    public float Position { get; set; }
    public int Level { get; set; }

    public bool Edge { get; set; }

    private string _method;
    public string Method
    {
        get
        {
            return _method;
        }
        set
        {
            _method = value;
            if (string.IsNullOrEmpty(Method) == false && GlobalData.SceneManager != null)
                Action = GlobalData.SceneManager.AIUtils.SetupNeuronAction(Method);
        }
    }

    private string _keywords;
    // encapsulate by ( ... ) ;
    public string Keywords
    {
        get
        {
            return _keywords;
        }
        set
        {
            _keywords = value;
        }
    }

    public Action<Unit> Action;

    private List<string> _keywordList;
    public List<string> KeywordList
    {
        get
        {
            if (_keywordList == null || _keywordList.Count == 0)
            {
                if (string.IsNullOrEmpty(Keywords) == false)
                {
                    _keywordList = new List<string>();

                    //keywordsCount = Keywords.Count(x => x == ';');
                    //for (var i = 0; i < keywordsCount; i++)
                    //{
                        _keywordList.Add(Keywords.Split('(', ')')[1]);
                    //}
                }
            }

            return _keywordList;
        }
    }

    // Editor variables
    public Rect rect;
    public int attachIndex;
    public bool edit;
}

public class ToDo
{
    public Job SuitableFor { get; set; }

    public string Description { get; set; }
}

public class Map
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; }
    [Unique]
    public int Number { get; set; }

    //[Ignore]
    //public Transform Transform { get; set; }
    public GameObject GameObject;

    public override string ToString()
    {
        return string.Format("[Map: Id={0}, Name={1}, Number-{2}, ImportantMapTiles={3}]", Id, Name, Number);
    }
}

//public class MapTile
//{
//    [PrimaryKey, AutoIncrement]
//    public int Id { get; set; }

//    public int MapId { get; set; }

//    [Ignore]
//    public TileType TyleType
//    {
//        get { return (TileType)TyleTypeId; }
//        set
//        {
//            var tileType = value;
//            TyleTypeId = (int)tileType;
//        }
//    }
//    public int TyleTypeId
//    {
//        get; set;
//    }

//    [Ignore]
//    public PuzzleObject PuzzleObject
//    {
//        get { return (PuzzleObject)PuzzleObjectId; }
//        set
//        {
//            var puzzleObject = value;
//            PuzzleObjectId = (int)puzzleObject;
//        }
//    }
//    public int PuzzleObjectId
//    {
//        get; set;
//    }

//    public float X { get; set; }
//    public float Y { get; set; }

//    public int BridgeId { get; set; }

//    // Misc
//    [Ignore]
//    public Misc Misc
//    {
//        get { return (Misc)MiscId; }
//        set
//        {
//            var misc = value;
//            MiscId = (int)misc;
//        }
//    }
//    public int MiscId
//    {
//        get; set;
//    }
//    public float Rotation { get; set; }
//    public float Z { get; set; }
//    // Misc - END

//    public string PrintObject(TileType tileType)
//    {
//        if (tileType == TileType.Misc)
//            return string.Format("[Map: TyleType={0}({1}), Rotation={2}, XPos={3}, YPos={4}]", TyleType, TyleTypeId, Rotation, X, Y);
//        if (tileType == TileType.DeathZone)
//            return string.Format("[Map: TyleType={0}({1}), XPos={2}, YPos={3}]", TyleType, TyleTypeId, X, Y);
//        if (tileType == TileType.PuzzleObject)
//            return string.Format("[Map: TyleType={0}({1}), XPos={2}, YPos={3}, PuzzleObject={4}({5}) ]", TyleType, TyleTypeId, X, Y, PuzzleObject, PuzzleObjectId);

//        return "mapTile";
//    }

//    public string Error;

//    public MapTile GetMapTyle(Transform objT, TileType tileType)
//    {
//        MapTile mapTile = null;

//        Misc misc;

//        switch (tileType)
//        {
//            case TileType.Misc:

//                misc = GetEnum(objT.gameObject.name);

//                if (misc != Misc.None)
//                {
//                    mapTile = new MapTile
//                    {
//                        TyleType = tileType,
//                        Misc = misc,
//                        Rotation = objT.eulerAngles.z,
//                        X = objT.position.x,
//                        Y = objT.position.y,
//                        Z = objT.position.z
//                    };
//                }
//                break;

//            case TileType.PuzzleObject:

//                switch (objT.tag)
//                {
//                    case "Player":

//                        mapTile = new MapTile
//                        {
//                            TyleType = tileType,
//                            X = objT.position.x,
//                            Y = objT.position.y,
//                            PuzzleObject = PuzzleObject.Player
//                        };
//                        break;

//                    case "Box":

//                        mapTile = new MapTile
//                        {
//                            TyleType = tileType,
//                            X = objT.position.x,
//                            Y = objT.position.y,
//                            PuzzleObject = PuzzleObject.Box
//                        };
//                        break;

//                    case "Bridge":

//                        mapTile = new MapTile
//                        {
//                            TyleType = tileType,
//                            X = objT.position.x,
//                            Y = objT.position.y,
//                            PuzzleObject = PuzzleObject.Bridge,
//                            BridgeId = objT.GetComponent<Bridge>().Id
//                        };

//                        if (mapTile.BridgeId == 0) mapTile.Error = "Bridge has no Id.";

//                        break;

//                    case "Trigger":

//                        mapTile = new MapTile
//                        {
//                            TyleType = tileType,
//                            X = objT.position.x,
//                            Y = objT.position.y,
//                            PuzzleObject = PuzzleObject.Trigger,
//                            BridgeId = objT.GetComponent<Trigger>().Bridge.Id
//                        };

//                        if (mapTile.BridgeId == 0) mapTile.Error = "Trigger has no BridgeId.";

//                        break;

//                    case "Finish":

//                        mapTile = new MapTile
//                        {
//                            TyleType = tileType,
//                            X = objT.position.x,
//                            Y = objT.position.y,
//                            PuzzleObject = PuzzleObject.Finish
//                        };

//                        break;
//                }
//                break;

//            default:
//                mapTile.Error = "ArgumentOutOfRangeException";
//                break;
//        }
//        return mapTile;
//    }

//    private Misc GetEnum(string goName, bool isPit = false)
//    {
//        if (goName.Contains("Pit00"))
//            return Misc.Pit00;
//        if (goName.Contains("Pit01"))
//            return Misc.Pit01;
//        if (goName.Contains("Pit02"))
//            return Misc.Pit02;
//        if (goName.Contains("Pit10"))
//            return Misc.Pit10;
//        if (goName.Contains("Pit11"))
//            return Misc.Pit11;
//        if (goName.Contains("Pit12"))
//            return Misc.Pit12;
//        if (goName.Contains("Pit20"))
//            return Misc.Pit20;
//        if (goName.Contains("Pit21"))
//            return Misc.Pit21;
//        if (goName.Contains("Pit22"))
//            return Misc.Pit22;
//        if (goName.Contains("PitA00"))
//            return Misc.PitA00;
//        if (goName.Contains("PitA22"))
//            return Misc.PitA22;

//        if (goName.Contains("PitS00"))
//            return Misc.PitS00;
//        if (goName.Contains("PitS01"))
//            return Misc.PitS01;
//        if (goName.Contains("PitS02"))
//            return Misc.PitS02;
//        if (goName.Contains("PitS20"))
//            return Misc.PitS20;
//        if (goName.Contains("PitS21"))
//            return Misc.PitS21;
//        if (goName.Contains("PitS22"))
//            return Misc.PitS22;

//        if (goName.Contains("Hill00"))
//            return Misc.Hill00;
//        if (goName.Contains("Hill01"))
//            return Misc.Hill01;
//        if (goName.Contains("Hill02"))
//            return Misc.Hill02;
//        if (goName.Contains("Hill10"))
//            return Misc.Hill10;
//        if (goName.Contains("Hill11"))
//            return Misc.Hill11;
//        if (goName.Contains("Hill12"))
//            return Misc.Hill12;
//        if (goName.Contains("Hill20"))
//            return Misc.Hill20;
//        if (goName.Contains("Hill21"))
//            return Misc.Hill21;
//        if (goName.Contains("Hill22"))
//            return Misc.Hill22;

//        switch (goName)
//        {
//            case "PipeConnector":
//                return Misc.PipeConnector;
//            case "Tutorial1":
//                return Misc.Tutorial1;
//            case "PipeHorizontal":
//                return Misc.PipeHorizontal;
//        }

//        if (goName.Contains("HillS00"))
//            return Misc.HillS00;
//        if (goName.Contains("HillS02"))
//            return Misc.HillS02;
//        if (goName.Contains("HillS20"))
//            return Misc.HillS20;
//        if (goName.Contains("HillS22"))
//            return Misc.HillS22;

//        if (goName.Contains("Hill"))
//            return Misc.Hill;

//        return Misc.None;
//    }
//}