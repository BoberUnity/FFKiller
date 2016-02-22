using UnityEngine;
using Tiled2Unity;
using System.Collections;
using System.Collections.Generic;
using Assets.GameEngine.BattleSystem;
using Assets.GameEngine.Map;
using Assets.GameEngine.Units;

public class GameManager : MonoBehaviour
{
    public static TiledMap CurrentMap;

    public List<TiledMap> Maps;

    public TiledMap Village;

    public static BattleManager BattleManager;

    public Assets.GameEngine.Units.PlayerController Player;

    public MapCollision WarpPoint;
    public TiledMap WarpDest;
    public int WarpEntry;

	// Use this for initialization
	void Awake ()
	{
        BattleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
	    Maps = new List<TiledMap>();
        Maps.Add(Village);
	    CurrentMap = Village;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartBattle()
    {
        Player.Camera.enabled = false;
        BattleManager.InitiateBattle();
        BattleManager.Camera.enabled = true;
    }

    public void EndBattle()
    {
        BattleManager.Camera.enabled = false;
        Player.Camera.enabled = true;

    }


}
