using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Difficulty
{
	EASY,
	NORMAL,
	HARD
};

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public static Color mouseOverColor = new Color (1.0f, 0.91f, 0.27f);
	public static Color targetAttackColor = new Color(1.0f,0.25f,0.0f);
	public static Color targetSplashColor = new Color(0.85f,0.23f,0.23f);
	public static Color targetMoveColor = new Color (0.33f, 0.835f, 1.0f);

    public static Vector3 offsetHigh = 1.5f * Vector3.up;

	public GameObject TilePreFab;
	public GameObject UserPlayerPreFab;
	public GameObject AIPlayerPreFab;

	public Difficulty difficulty = Difficulty.EASY;

	// MapManager Variables
	public int mapSizeX = 22;
	public int mapSizeY = 22;
	public List<List<Tile>> map = new List<List<Tile>>();
	public List<Player> players = new List<Player>();

	public int currentPlayerIndex = 0;

	void Awake () {
		instance = this;
	}

	// Use this for initialization
	void Start () {
        Debug.Log("Inicializing map...");
		generateMap ();
        Debug.Log("Inicializing Players...");
		generatePlayers ();
        Debug.Log("Ajusting camera...");
		adjustCamera ();
	}
	
	// Update is called once per frame
	void Update () {
		if (players [currentPlayerIndex].HP > 0) {
			if(getTileByGridPosition(players[currentPlayerIndex].gridPosition).impassable){
				players[currentPlayerIndex].setPlayerPositionPassable();
			}
			players [currentPlayerIndex].TurnUpdate ();	
		} else {
			nextTurn();
		}
	}

	void OnGUI () {
		if (players [currentPlayerIndex].HP > 0) {
			players [currentPlayerIndex].TurnOnGUI ();
		} else {
			nextTurn();
		}
	}

	public void nextTurn () {
		players [currentPlayerIndex].endPlayerTurn ();

		if (currentPlayerIndex + 1 < players.Count) {
			currentPlayerIndex++;
		} else {
			currentPlayerIndex =0;
		}
	}

	public void highlightTilesAt( Vector2 originLocation, Color highlightColor, int range){
		List<Tile> highlightedTiles;
		if (players [currentPlayerIndex].attackingPhase) {
			highlightedTiles =  TileHighligth.FindAttackHighlight (map [(int)originLocation.x] [(int)originLocation.y], range);
		} else {
			highlightedTiles =  TileHighligth.FindHighlight (map [(int)originLocation.x] [(int)originLocation.y], range);
		}

		foreach (Tile t in highlightedTiles) {
			t.transform.GetComponent<Renderer>().material.color = highlightColor;
			t.previousColor = highlightColor;
		}
	}

	public void removeMapHighlights(){
		for (int i=0; i < mapSizeX; i++) {
			for (int j=0; j < mapSizeY; j++){
				map[i][j].transform.GetComponent<Renderer>().material.color = Color.white;
				map[i][j].previousColor = Color.white;
			}
		}
	}

	public void attackWithCurrentPlayer(Tile destTile) {
        Color destTileMatColor = destTile.transform.GetComponent<Renderer>().material.color;
        if ((destTileMatColor != Color.white && destTileMatColor != mouseOverColor) || (players[currentPlayerIndex].GetType() == typeof(AIPlayer)))
        {
			Player target = null;
			foreach (Player p in players) {
				if(p.gridPosition == destTile.gridPosition) {
					target = p;
				}
			}
			if (target != null) {
				if(target != players[currentPlayerIndex]){
					players[currentPlayerIndex].actionPoints--;

					bool hit = Random.Range(0.0f,1.0f) <= players[currentPlayerIndex].attackChance;
					if (hit) {
						int amountOfDamage = players[currentPlayerIndex].damageBase + Random.Range(1,players[currentPlayerIndex].rollSides);
						amountOfDamage = Mathf.FloorToInt(amountOfDamage * (target.damageReduction + 1));
						Debug.Log(players[currentPlayerIndex].playerName + " succefully hit " + target.playerName + " for " + amountOfDamage + " damage.");
						players[currentPlayerIndex].doDamageTo(target);
						
					} else {
						Debug.Log(players[currentPlayerIndex].playerName + " missed " + target.playerName);
					}
				} else{
					Debug.Log ("You must not hit yourself!");
				}
			}
		} else {
			Debug.Log("Out of Range");
		}
	}

	public Tile getTileByGridPosition(Vector2 desiredPosition){
		return map [(int)desiredPosition.x] [(int)desiredPosition.y];
	}

	public Player getPlayerByTile( Tile tile){
		foreach (Player p in players) {
			if(p.gridPosition == tile.gridPosition){
				return p;
			}
		}
		return null;
	}

	public List<Player> getLivePlayers(){
		List<Player> livePlayers = new List<Player> ();
		foreach (Player p in players) {
			if(p.HP > 0) livePlayers.Add(p);
		}
		return livePlayers;
	}

	public static int getDistanceByTiles(Tile originTile, Tile destTarget){
		int distance;
		distance = Mathf.FloorToInt( Vector3.Distance (originTile.transform.position, destTarget.transform.position));
		return distance;
	}

	public List<Vector2> findPlayersByGridPosition(){
		List<Vector2> positions = new List<Vector2> ();
		foreach(Player p  in players){
			positions.Add(p.gridPosition);
		}
		return positions;
	}

	public List<Tile> findPlayersByTile(){
		List<Tile> tiles = new List<Tile> ();
		foreach (Player p in players) {
			tiles.Add(p.getTile());
		}
		return tiles;
	}

	public List<Tile> findLivePlayersByTile(){
		List<Tile> tiles = new List<Tile> ();
		foreach (Player p in players) {
			if(p.HP > 0) tiles.Add(p.getTile());
		}
		return tiles;
	}

	void adjustCamera(){
		int offset = 5;
		Camera.main.transform.localPosition = getTileByGridPosition (new Vector2(mapSizeX / 2,mapSizeY / 2)).transform.position + Vector3.up * (Mathf.Max(mapSizeX,mapSizeY) + offset);
		Camera.main.transform.localRotation = Quaternion.Euler (new Vector3 (90, 0, 0));
	}

	void generateMap() {
		map = new List<List<Tile>>();
		for (int i = 0; i < mapSizeX; i++) {
			List<Tile> row = new List<Tile>();
			for (int j = 0; j < mapSizeY; j++) {
				Tile tile;
				tile = ((GameObject)Instantiate(
					TilePreFab, 
					new Vector3(i - Mathf.Floor((mapSizeX/2)),0,-j + Mathf.Floor((mapSizeY/2))), 
					Quaternion.Euler(new Vector3())
				)).GetComponent<Tile>();
				tile.gridPosition = new Vector2(i,j);
				row.Add(tile);
			}
			map.Add(row);
		}
	}

	void generatePlayers () {
		UserPlayer player;
		AIPlayer aiplayer;

		player = ((GameObject)Instantiate(
			UserPlayerPreFab,
			new Vector3(0 - Mathf.Floor(mapSizeX/2),1.5f,-0 + Mathf.Floor(mapSizeY/2)), 
			Quaternion.Euler(new Vector3())
		)).GetComponent<UserPlayer>();
		player.gridPosition = new Vector2 (0, 0);
		player.playerName = "Player1";
		players.Add (player);

		player = ((GameObject)Instantiate(
			UserPlayerPreFab,
			new Vector3((4) - Mathf.Floor(mapSizeX/2),1.5f,-(0) + Mathf.Floor(mapSizeY/2)), 
			Quaternion.Euler(new Vector3())
			)).GetComponent<UserPlayer>();
		player.gridPosition = new Vector2 (4, 0);
		player.playerName = "Player2";
		player.attackRange = 4;
		players.Add (player);

		player = ((GameObject)Instantiate(
			UserPlayerPreFab,
			new Vector3((8) - Mathf.Floor(mapSizeX/2),1.5f,-(0) + Mathf.Floor(mapSizeY/2)), 
			Quaternion.Euler(new Vector3())
			)).GetComponent<UserPlayer>();
		player.gridPosition = new Vector2 (8, 0);
		player.playerName = "Player3";
		player.attackRange = 4;
		players.Add (player);

		player = ((GameObject)Instantiate(
			UserPlayerPreFab,
			new Vector3((mapSizeX-1) - Mathf.Floor(mapSizeX/2),1.5f,-(0) + Mathf.Floor(mapSizeY/2)), 
			Quaternion.Euler(new Vector3())
			)).GetComponent<UserPlayer>();
		player.gridPosition = new Vector2 (mapSizeX-1, 0);
		player.playerName = "Player4";
		players.Add (player);

		aiplayer = ((GameObject)Instantiate(
			AIPlayerPreFab,
			new Vector3((mapSizeX-1) - Mathf.Floor(mapSizeX/2),1.5f,-(mapSizeY-1) + Mathf.Floor(mapSizeY/2)), 
			Quaternion.Euler(new Vector3())
			)).GetComponent<AIPlayer>();
		aiplayer.gridPosition = new Vector2 (mapSizeX-1, mapSizeY-1);
		aiplayer.playerName = "PlayerAI1";
		players.Add (aiplayer);

		aiplayer = ((GameObject)Instantiate(
			AIPlayerPreFab,
			new Vector3((mapSizeX-1 -4) - Mathf.Floor(mapSizeX/2),1.5f,-(mapSizeY-1) + Mathf.Floor(mapSizeY/2)), 
			Quaternion.Euler(new Vector3())
			)).GetComponent<AIPlayer>();
		aiplayer.gridPosition = new Vector2 (mapSizeX-1 -4, mapSizeY-1);
		aiplayer.playerName = "PlayerAI2";
		aiplayer.attackRange = 7;
		aiplayer.startingMovePoints = 4;
		aiplayer.MaxHP = 12;
		players.Add (aiplayer);

		aiplayer = ((GameObject)Instantiate(
			AIPlayerPreFab,
			new Vector3((mapSizeX-1 -8) - Mathf.Floor(mapSizeX/2),1.5f,-(mapSizeY-1) + Mathf.Floor(mapSizeY/2)), 
			Quaternion.Euler(new Vector3())
			)).GetComponent<AIPlayer>();
		aiplayer.gridPosition = new Vector2 (mapSizeX-1 -8, mapSizeY-1);
		aiplayer.playerName = "PlayerAI3";
		players.Add (aiplayer);

		aiplayer = ((GameObject)Instantiate(
			AIPlayerPreFab,
			new Vector3((0) - Mathf.Floor(mapSizeX/2),1.5f,-(mapSizeY-1) + Mathf.Floor(mapSizeY/2)), 
			Quaternion.Euler(new Vector3())
			)).GetComponent<AIPlayer>();
		aiplayer.gridPosition = new Vector2 (0, mapSizeY-1);
		aiplayer.playerName = "PlayerAI4";
		aiplayer.startingActionPoints = 1;
		aiplayer.startingMovePoints = 4;
		aiplayer.damageBase = 12;
		aiplayer.rollSides = 12;
		aiplayer.MaxHP = 125;
		players.Add (aiplayer);
	}
}
