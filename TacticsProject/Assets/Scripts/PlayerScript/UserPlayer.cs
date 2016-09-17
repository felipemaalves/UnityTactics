using UnityEngine;
using System.Collections;

public class UserPlayer : Player {

	public override void TurnUpdate ()
	{
		if(Input.GetMouseButtonDown(1) || Input.GetButtonDown("Cancel")){
			GameManager.instance.removeMapHighlights();
			positionQueue.Clear();
			highlighted = false;
			movingPhase = false;
			attackingPhase = false;
			return;
		}
		if (positionQueue.Count > 0 && movePoints > 0) {
			updatePositionQueue();
			GameManager.instance.removeMapHighlights();
			highlighted = false;
		} else if(movingPhase && !highlighted){
			GameManager.instance.highlightTilesAt(gridPosition,GameManager.targetMoveColor, movePoints);
			highlighted = true;
		}

		base.TurnUpdate ();
	}

    public override void movePlayer(Tile destTile)
    {
        Color destTileMatColor = destTile.transform.GetComponent<Renderer>().material.color;

        if ((destTileMatColor != Color.white && destTileMatColor != GameManager.mouseOverColor))
        {
            base.movePlayer(destTile);
        }
        else
        {
            Debug.Log("Destination invalid");
        }
    }
}
