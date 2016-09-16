using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	public Vector2 gridPosition = Vector2.zero;

	public Vector3 moveDestination;
	public float moveSpeed = 0.1f;

	public bool attackingPhase = false;
	public bool movingPhase = false;
	public bool highlighted = false;

    public string team = "Team Default";
	public string playerName = "Default";
	public int HP = 25;
	public int MaxHP = 25;

	public float attackChance = 0.75f;
	public float damageReduction = 0.15f;
	public int damageBase = 5;
	public int rollSides = 6;

	public int actionPoints;
	public int movePoints;
	public int attackRange = 1;

	public int startingActionPoints = 2;
	public int startingMovePoints = 5;

    private Attribute attributes = new Attribute();

	//movement animation
	public List<Vector3> positionQueue = new List<Vector3>();

    public TilePath tileQueue = new TilePath();

	private bool mouseOverPlayer = false;

	void Awake () {
		moveDestination = transform.position;
		RefreshPoints ();
	}

	// Use this for initialization
	void Start () {
		this.HP = this.MaxHP;
        this.attributes = new Attribute();
        this.moveSpeed = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.instance.players [GameManager.instance.currentPlayerIndex] == this) {
			transform.GetComponent<Renderer> ().material.color = Color.green;
		} else {
			transform.GetComponent<Renderer> ().material.color = Color.white;
		}
		if (HP <= 0) {
			transform.rotation = Quaternion.Euler(new Vector3(90,0,0));
			transform.GetComponent<Renderer> ().material.color = Color.red;
		}
	}

	void OnMouseOver(){
		mouseOverPlayer = true;
		this.getTile().OnMouseOver ();
	}

	void OnMouseExit(){
		GameManager.instance.removeMapHighlights ();
        Player activePlayer = GameManager.instance.players[GameManager.instance.currentPlayerIndex];
        if (activePlayer.attackingPhase) {
            activePlayer.startAttackPhase();
		} else if (activePlayer.movingPhase) {
            activePlayer.startMovePhase();
		}
		mouseOverPlayer = false;
		this.getTile().OnMouseExit ();
	}

	void OnMouseDown(){
        this.getTile().OnMouseDown();
	}

	public virtual void OnGUI(){
		float buttonHeight = 50;
		float buttonWidth = 150;
		if (mouseOverPlayer) {
			//Show mouseOver Statistics
			Rect playerAttributesRect = new Rect (Screen.width - buttonWidth, 0, buttonWidth, buttonHeight * 2);
			GUI.TextArea (playerAttributesRect, 
			           "Name: " + playerName + "\n" +
				"HP: " + HP + "/" + MaxHP + "\n" +
				"Damage: " + damageBase + " + 1d" + rollSides + "\n" +
				"Precision: " + attackChance * 100 + "%\n" +
				"Damage Reduction: " + damageReduction * 100 + "%\n" +
			    "Range: " + attackRange
			);
			Rect textRect = new Rect (Screen.width - buttonWidth,buttonHeight * 2, buttonWidth, buttonHeight);
			GUI.TextArea (textRect, "Action Points: " + startingActionPoints + "\nMove Points: " + startingMovePoints);

			GameManager.instance.highlightTilesAt(gridPosition,GameManager.targetAttackColor,attackRange);
		}
		// show HP BEGIN
		int width = 40;
		if (MaxHP > 100) width += 20;
		Vector3 location = Camera.main.WorldToScreenPoint (transform.position) + Vector3.up * 35;
		GUI.TextArea (new Rect(location.x,Screen.height - location.y, width, 20),(HP.ToString() + "/" + MaxHP.ToString()));
		// show HP END
	}

	public virtual void TurnOnGUI () {
		
	}

	public void startMovePhase(){
		GameManager.instance.removeMapHighlights();
		movingPhase = true;
		attackingPhase = false;
		GameManager.instance.highlightTilesAt(gridPosition, GameManager.targetMoveColor, GameManager.instance.players [GameManager.instance.currentPlayerIndex].movePoints);
	}

	public void startAttackPhase(){
		GameManager.instance.removeMapHighlights();
		movingPhase = false;
		attackingPhase = true;
		GameManager.instance.highlightTilesAt(gridPosition, GameManager.targetAttackColor, GameManager.instance.players [GameManager.instance.currentPlayerIndex].attackRange);
	}

	public virtual void endPlayerTurn(){
		GameManager.instance.removeMapHighlights();
		RefreshPoints();
		positionQueue.Clear();
		movingPhase = false;
		attackingPhase = false;
		setPlayerPositionImpassable ();
	}

	public void RefreshPoints () {
		actionPoints = startingActionPoints;
		movePoints = startingMovePoints;
	}
	
	public void SetStartingActionPoints (int startingPoints) {
		startingActionPoints = startingPoints;
	}
	
	public void SetStartingMovingPoints (int startingPoints) {
		startingMovePoints = startingPoints;
	}
	
	public virtual void TurnUpdate () {
		
	}

	public void updatePositionQueue () {
		if (Vector3.Distance (positionQueue [0], transform.position) > 0.1f) {
			transform.position += (positionQueue [0] - transform.position).normalized * this.moveSpeed;
			
			if (Vector3.Distance (positionQueue [0], transform.position) <= 0.1f) {
				transform.position = positionQueue [0];
				positionQueue.RemoveAt (0);
				movePoints--;
			}
		}
	}

    public virtual void movePlayer(Tile destTile){
        if (this.gridPosition != destTile.gridPosition && !destTile.impassable)
        {
            foreach (Tile t in TilePathFinder.FindPath(this.getTile(), destTile))
            {
                this.positionQueue.Add(t.transform.position + GameManager.offsetHigh);
                destTile = t;
                if (this.positionQueue.Count >= this.startingMovePoints)
                {
                    break;
                }
            }
            this.gridPosition = destTile.gridPosition;
        }
    }

    public void doDamageTo(Player target)
    {
        int amountOfDamage = this.damageBase + this.rollDice();
        Damage damage = new Damage(amountOfDamage, DamageType.CONTUSION);
        Damage.doDamageTo(target, damage);
    }

    public int rollDice()
    {
        return Random.Range(1, this.rollSides);
    }

    public void attackWithEquipedWeapon(Tile destTile){
    }

    public Tile getTile()
    {
        return GameManager.instance.getTileByGridPosition(gridPosition);
    }

    public void setPlayerPositionPassable()
    {
        GameManager.instance.getTileByGridPosition(gridPosition).impassable = false;
    }

    public void setPlayerPositionImpassable()
    {
        GameManager.instance.getTileByGridPosition(gridPosition).impassable = true;
    }

    /*
     * AI Auxiliary functions
     */

    public Player getPriorityTarget(List<Tile> tilesOfOpponentsInRange)
    {
        /* PriorityTarget
         * 	1- Lowest HP
         *  2- Closest Player
         */
        Player targetPlayer = null;
        int lowestHP = 0;
        if (tilesOfOpponentsInRange.Count == 0)
        {
            return targetPlayer;
        }
        else
        {
            foreach (Tile t in tilesOfOpponentsInRange)
            {
                if (targetPlayer == null)
                {
                    targetPlayer = GameManager.instance.getPlayerByTile(t);
                }
                else if (targetPlayer.HP > GameManager.instance.getPlayerByTile(t).HP)
                {
                    targetPlayer = GameManager.instance.getPlayerByTile(t);
                    lowestHP = targetPlayer.HP;
                }
                else if ((targetPlayer.HP == GameManager.instance.getPlayerByTile(t).HP) && targetPlayer.HP <= lowestHP)
                {
                    List<Player> opponentsWithSameHP = new List<Player>();
                    foreach (Tile tile in tilesOfOpponentsInRange)
                    {
                        if (targetPlayer.HP == GameManager.instance.getPlayerByTile(tile).HP)
                        {
                            opponentsWithSameHP.Add(GameManager.instance.getPlayerByTile(tile));
                        }
                        targetPlayer = getClosestPlayer(opponentsWithSameHP);
                    }
                }
            }
        }
        return targetPlayer;
    }

    public Player getClosestPlayer(List<Player> players)
    {
        Player closest = null;
        int distance = -1;
        foreach (Player p in players)
        {
            if (p == this) continue;
            if (p.HP <= 0) continue;
            if (this.GetType() == p.GetType()) continue;
            if (distance > 0)
            {
                if (GameManager.getDistanceByTiles(this.getTile(), p.getTile()) < distance)
                {
                    distance = GameManager.getDistanceByTiles(this.getTile(), p.getTile());
                    closest = p;
                }
            }
            else
            {
                distance = GameManager.getDistanceByTiles(this.getTile(), p.getTile());
                closest = p;
            }
        }
        return closest;
    }

    public void moveToAttackRange(Player target)
    {
        Tile destTile = target.getTile().neighbors[0];
        foreach (Tile t in target.getTile().neighbors)
        {
            if (Vector3.Distance(transform.position, t.transform.position) < Vector3.Distance(transform.position, destTile.transform.position))
            {
                destTile = t;
            }
        }
        List<Tile> tiles = TilePathFinder.FindPath(getTile(), destTile);
        for (int i = 1; i < this.attackRange; i++)
        {
            tiles.RemoveAt(tiles.Count - 1);
        }
        destTile = tiles[tiles.Count - 1];
        movePlayer(destTile);
    }

    public void moveToClosestPositionFromOpponent(Player target)
    {
        Tile destTile = target.getTile().neighbors[0];
        foreach (Tile t in target.getTile().neighbors)
        {
            if (Vector3.Distance(transform.position, t.transform.position) < Vector3.Distance(transform.position, destTile.transform.position))
            {
                destTile = t;
            }
        }
        movePlayer(destTile);
    }

    public void moveToFartherPositionFromOpponent(Player target)
    {
        List<Tile> highlightedTiles = TileHighligth.FindHighlight(getTile(), movePoints);
        Tile destTile = getTile();
        foreach (Tile t in highlightedTiles)
        {
            if (Vector3.Distance(target.transform.position, t.transform.position) > Vector3.Distance(target.transform.position, destTile.transform.position))
            {
                destTile = t;
            }
        }
        movePlayer(destTile);
    }

    public int getStr()
    {
        return this.attributes.getStrength();
    }

    public int getAgi()
    {
        return this.attributes.getAgility();
    }

    public int getDex()
    {
        return this.attributes.getDexterity();
    }

    public int getInt()
    {
        return this.attributes.getInteligence();
    }

    public int getWis()
    {
        return this.attributes.getWisdom();
    }

    public int getVit()
    {
        return this.attributes.getVitality();
    }

    public int getPer()
    {
        return this.attributes.getPerception();
    }

    public Attribute getAttributesPure()
    {
        return attributes;
    }

    public Attribute getAttributes()
    {
        return attributes;
    }

}
