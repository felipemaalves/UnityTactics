using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AIPlayer : Player {

	bool finnishMoving = false;
	bool finnishActing = false;

	int iddleCounter = 0;

	public override void TurnUpdate ()
	{
		if (positionQueue.Count > 0 && movePoints > 0) {
			updatePositionQueue();
		} else {
			List<Tile> tilesOfOpponentsInRange = new List<Tile>();
			List<Tile> playersTile = GameManager.instance.findLivePlayersByTile();
			List<Player> livePlayers = GameManager.instance.getLivePlayers();
			List<Tile> highLightedTiles = AItargetHighlight();

			foreach(Tile tile in playersTile){
				if(GameManager.instance.getPlayerByTile(tile).GetType() != typeof(AIPlayer)){
					foreach(Tile t in highLightedTiles){
						if( tile == t) {
							tilesOfOpponentsInRange.Add(t);
							break;
						}
					}
				}
			}

			List<Tile> highlightedInRange = TileHighligth.FindAttackHighlight(getTile(),attackRange);
			List<Tile> tilesOfTargetsInAttackRange = new List<Tile>();

			foreach(Tile tile in playersTile){
				if(GameManager.instance.getPlayerByTile(tile).GetType() != typeof(AIPlayer)){
					foreach(Tile t in highlightedInRange){
						if( tile == t) {
							tilesOfTargetsInAttackRange.Add(t);
							break;
						}
					}
				}
			}

			if(movePoints > 0 && tilesOfTargetsInAttackRange.Count == 0 && actionPoints > 0) {
				iddleCounter = 0;
				if(tilesOfOpponentsInRange.Count > 0) {
					iddleCounter = 0;
					Player target = getPriorityTarget(tilesOfOpponentsInRange);
					moveToAttackRange(target);
					movingPhase = false;
					if(positionQueue.Count == 0) {
						finnishMoving = true;
					} else {
						finnishMoving = false;
					}
				} else {
					iddleCounter = 0;
					Player target = getClosestPlayer(livePlayers);
					if(target != null  && target.GetType() != typeof(AIPlayer)) moveToClosestPositionFromOpponent(target);
					if(positionQueue.Count == 0) {
						finnishMoving = true;
					} else {
						finnishMoving = false;
					}
					finnishActing = true;
				}
			} else if (tilesOfTargetsInAttackRange.Count > 0 && actionPoints > 0){
				iddleCounter = 0;
				Player target = getPriorityTarget(tilesOfOpponentsInRange);
				for(int i = 0; i < actionPoints; i++){
					if (target != null  && target.GetType() != typeof(AIPlayer)){
						if(target.HP > 0){
							GameManager.instance.attackWithCurrentPlayer(GameManager.instance.getTileByGridPosition(target.gridPosition));
						}
					}
				}
				if(movePoints > 0 && target.HP > 0){
					iddleCounter = 0;
					target = getClosestPlayer(livePlayers);
					if(target != null && target.GetType() != typeof(AIPlayer)) moveToFartherPositionFromOpponent(target);
					if(positionQueue.Count == 0) {
						finnishMoving = true;
					} else {
						finnishMoving = false;
					}
					finnishActing = true;
				} else if(movePoints > 0 && target.HP <= 0){
					iddleCounter = 0;
					target = getClosestPlayer(livePlayers);
					if(target != null && target.GetType() != typeof(AIPlayer)) moveToClosestPositionFromOpponent(target);
					if(positionQueue.Count == 0) {
						finnishMoving = true;
					} else {
						finnishMoving = false;
					}
				}
			}
		}

		if(positionQueue.Count == 0) {
			finnishMoving = true;
		} else {
			finnishMoving = false;
		}

		if (((movePoints <= 0 || finnishMoving) && (actionPoints <= 0 || finnishActing)) || iddleCounter >= 60){
			finnishActing = true;
			finnishMoving = true;
			GameManager.instance.nextTurn();
		}

		iddleCounter++;

		base.TurnUpdate ();
	}

	public List<Tile> AItargetHighlight(){
		List<Tile> highLightedTiles = new List<Tile>();
		foreach(Player p in GameManager.instance.players){
			p.setPlayerPositionPassable();
		}
		highLightedTiles = TileHighligth.FindHighlight(getTile(),movePoints + attackRange,false, movePoints);
		foreach(Player p in GameManager.instance.players){
			if( p != this ) p.setPlayerPositionImpassable();
		}
		return highLightedTiles;
	}

	public override void TurnOnGUI () {
		base.TurnOnGUI ();
	}

	public override void endPlayerTurn () {
		iddleCounter = 0;
		finnishActing = false;
		finnishMoving = false;
		base.endPlayerTurn ();
	}
}
