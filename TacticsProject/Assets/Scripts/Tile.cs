using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	public Vector2 gridPosition = Vector2.zero;
	public List<Tile> neighbors = new List<Tile>();

	public List<Material> surfacesMaterials = new List<Material>();
	public int movementCost = 1;
	public bool impassable = false;

	public Color previousColor;

	// Use this for initialization
	void Start () {
		previousColor = Color.white;
		generateNeighbors();
	}

	void generateNeighbors () {
		neighbors = new List<Tile>();
		//up
		if (gridPosition.y > 0) {
			Vector2 n = new Vector2(gridPosition.x,gridPosition.y - 1);
			neighbors.Add(GameManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
		}
		//down
		if (gridPosition.y < GameManager.instance.map.Count - 1) {
			Vector2 n = new Vector2(gridPosition.x,gridPosition.y + 1);
			neighbors.Add(GameManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
		}
		//left
		if (gridPosition.x > 0) {
			Vector2 n = new Vector2(gridPosition.x - 1,gridPosition.y);
			neighbors.Add(GameManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
		}
		//rigth
		if (gridPosition.x < GameManager.instance.map.Count -1) {
			Vector2 n = new Vector2(gridPosition.x + 1,gridPosition.y);
			neighbors.Add(GameManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseOver () {
		Color mouseOverColor = GameManager.mouseOverColor;
		Color overlapColor = new Color (1.0f, 1.0f, 1.0f);

		if (previousColor == Color.white) {
			overlapColor = mouseOverColor;
		} else {
			overlapColor.r = (previousColor.r + mouseOverColor.r) / 2.0f;
			overlapColor.g = (previousColor.g + mouseOverColor.g) / 2.0f;
			overlapColor.b = (previousColor.b + mouseOverColor.b) / 2.0f;
		}

		transform.GetComponent<Renderer> ().material.color = overlapColor;

		//GetMouseButtonDown(1) right mouse button
		if (Input.GetMouseButtonDown (1)) {
			if (!impassable) {
				impassable = true;
				transform.GetComponent<Renderer> ().material = surfacesMaterials [1];
			} else {
				impassable = false;
				transform.GetComponent<Renderer> ().material = surfacesMaterials [0];
			}
		}
	}

	public void OnMouseExit () {
		transform.GetComponent<Renderer> ().material.color = previousColor;	
	}

	void OnMouseDown () {
		Player instancePlayer = GameManager.instance.players [GameManager.instance.currentPlayerIndex];

		if (instancePlayer.movingPhase && instancePlayer.movePoints > 0) {
			GameManager.instance.moveCurrentPlayer (this);
		} else if (instancePlayer.attackingPhase && instancePlayer.actionPoints > 0) {;
			GameManager.instance.attackWithCurrentPlayer (this);
		}
	}
}
