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
			GameManager.instance.highlightTilesAt(gridPosition,new Color(0.33f,0.835f,1.0f),movePoints);
			highlighted = true;
		}

		base.TurnUpdate ();
	}

	public override void TurnOnGUI () {
		float buttonHeight = 50;
		float buttonWidth = 150;

		Rect textRect = new Rect (0, Screen.height - buttonHeight * 4, buttonWidth, buttonHeight);
		GUI.TextArea (textRect, "Action Points: " + actionPoints + "\nMove Points: " + movePoints);

		Rect playerAttributesRect = new Rect (0, 0, buttonWidth, buttonHeight*2);
		GUI.TextArea (playerAttributesRect, 
		    "Name: " + playerName + "\n" +
			"HP: " + HP + "/" + MaxHP + "\n" +
			"Damage: " + damageBase + " + 1d" + damageRollSides + "\n" +
			"Precision: " + attackChance * 100 + "%\n" +
            "Damage Reduction: " + damageReduction * 100 + "%\n" +
            "Range: " + attackRange
		);

		Rect buttonRect = new Rect (0, Screen.height - buttonHeight * 3, buttonWidth, buttonHeight);

		if (GUI.Button (buttonRect, " Move ") || Input.GetButtonDown("Move")) {
			startMovePhase();
		}

		buttonRect = new Rect (0, Screen.height - buttonHeight * 2, buttonWidth, buttonHeight);

		if (GUI.Button (buttonRect, " Attack ") || Input.GetButtonDown("Attack")) {
			startAttackPhase();
		}

		buttonRect = new Rect (0, Screen.height - buttonHeight * 1, buttonWidth, buttonHeight);

		if (GUI.Button (buttonRect, " End Turn ")) {
			GameManager.instance.nextTurn();
		}

		base.TurnOnGUI ();
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
