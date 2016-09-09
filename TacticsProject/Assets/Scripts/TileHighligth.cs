using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileHighligth {

	public TileHighligth () {

	}

	public static List<Tile> FindHighlight (Tile originTile, int movementPoints){
		return FindHighlight (originTile, movementPoints, false, movementPoints+1);
	}

	public static List<Tile> FindHighlight (Tile originTile, int movementPoints, bool targetSelf){
		return FindHighlight (originTile, movementPoints, false, movementPoints+1);
	}

	public static List<Tile> FindHighlight (Tile originTile, int movementPoints, bool targetSelf, int ignoreImpassableAfter) {
		List<Tile> closed = new List<Tile>();
		List<TilePath> open = new List<TilePath>();

		TilePath originPath = new TilePath ();
		originPath.addTile (originTile);

		open.Add (originPath);

		while (open.Count >0) {
			TilePath current = open[0];
			open.Remove(open[0]);

			if(closed.Contains(current.lastTile)) {
				continue;
			}
			if(current.costOfPath > movementPoints+1) {
				continue;
			}

			closed.Add(current.lastTile);

			foreach(Tile t in current.lastTile.neighbors){
				if(ignoreImpassableAfter > current.costOfPath){
					if(t.impassable) continue;
				}
				TilePath newTilePath = new TilePath(current);
				newTilePath.addTile(t);
				open.Add(newTilePath);
			}
		}
		if (!targetSelf) {
			closed.Remove (originTile);
		}

		return closed;
	}

	public static List<Tile> FindAttackHighlight (Tile originTile, int movementPoints){
		return FindAttackHighlight (originTile, movementPoints, false);
	}

	public static List<Tile> FindAttackHighlight (Tile originTile, int movementPoints, bool targetSelf) {

		List<Tile> closed = new List<Tile>();
		List<TilePath> open = new List<TilePath>();
		
		TilePath originPath = new TilePath ();
		originPath.addTile (originTile);
		
		open.Add (originPath);
		
		while (open.Count >0) {
			TilePath current = open[0];
			open.Remove(open[0]);
			
			if(closed.Contains(current.lastTile)) {
				continue;
			}
			if(current.costOfPath > movementPoints+1) {
				continue;
			}
			
			closed.Add(current.lastTile);
			
			foreach(Tile t in current.lastTile.neighbors){
				TilePath newTilePath = new TilePath(current);
				newTilePath.addTile(t);
				open.Add(newTilePath);
			}
		}
		if (!targetSelf) {
			closed.Remove (originTile);
		}
		
		return closed;
	}

}
