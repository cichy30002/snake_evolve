using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
	[Header("Connections")]
	public GameBoard Board;
	public Transform Head;
	public GameObject BodyPrefab;
	public LineRenderer line;
	[HideInInspector]
	public State snakeState = State.start;
	[Header("Parameters")]
	public int StartLength = 3;
	public float MoveCooldown = 0.5f;
	public float InputCooldown = 0.3f;

	public enum State { start, up, down, right, left }
	[HideInInspector]
	public Vector2 headPosition;
	private List<Vector2> bodyPositions = new List<Vector2>();
	private List<Transform> bodyTransforms = new List<Transform>();
	private List<SpriteRenderer> bodySR = new List<SpriteRenderer>();

	private Vector2 lastHeadPosition;
	private List<Vector2> lastBodyPositions = new List<Vector2>();
	private int length;
	private float nextDestinationsChange = 0f;
	private float nextInputTime = 0f;
	private bool growthTime = false;
	private Quaternion lastRot;
	
	void Start()
    {
		transform.position = Vector3.zero;
		transform.localScale = new Vector3(Board.TileSize, Board.TileSize, 1);

		Head.position = Board.BoardMatrix[(int)Board.middle.x, (int)Board.middle.y];
		headPosition = Board.middle;
		GameObject tmpBody;
		for (int i = 0; i < StartLength-1; i++)
		{
			bodyPositions.Add(Board.middle - new Vector2(0,i+1));
			tmpBody = Instantiate(BodyPrefab, Board.BoardMatrix[(int)bodyPositions[i].x, (int)bodyPositions[i].y], transform.rotation, transform);
			bodyTransforms.Add(tmpBody.GetComponent<Transform>());
			bodySR.Add(tmpBody.GetComponent<SpriteRenderer>());
		}
		length = StartLength;
		lastRot = Quaternion.identity;
		line.widthMultiplier = Board.TileSize;
		line.positionCount = StartLength + 1;
	}

    // Update is called once per frame
    void Update()
    {
		InputHandler();
		MovementHandler();
		

	}

	void InputHandler()
	{
		if(Time.time > nextInputTime)
		{
			if (Input.GetKeyDown(KeyCode.W) && snakeState != State.down)
			{
				snakeState = State.up;
				nextInputTime = Time.time + InputCooldown;
			}
			else if (Input.GetKeyDown(KeyCode.S) && snakeState != State.up && snakeState != State.start)
			{
				snakeState = State.down;
				nextInputTime = Time.time + InputCooldown;
			}
			else if (Input.GetKeyDown(KeyCode.A) && snakeState != State.right)
			{
				snakeState = State.left;
				nextInputTime = Time.time + InputCooldown;
			}
			else if (Input.GetKeyDown(KeyCode.D) && snakeState != State.left)
			{
				snakeState = State.right;
				nextInputTime = Time.time + InputCooldown;
			}
		}
		
	}
	void MovementHandler()
	{
		if (Time.time > nextDestinationsChange)
		{
			switch (snakeState)
			{
				case State.up:
					ChangeDestinations(new Vector2(0, 1));
					break;
				case State.down:
					ChangeDestinations(new Vector2(0, -1));
					break;
				case State.left:
					ChangeDestinations(new Vector2(-1, 0));
					break;
				case State.right:
					ChangeDestinations(new Vector2(1, 0));
					break;
			}
			lastRot = Head.rotation;

		}
		if (snakeState != State.start)
		{
			Move();
			//CorrectDraw();
		}
	}
	void ChangeDestinations(Vector2 dir)
	{
		nextDestinationsChange = Time.time + MoveCooldown;

		lastBodyPositions = new List<Vector2>(bodyPositions);
		lastHeadPosition = headPosition;
		bodyPositions.Insert(0, headPosition);
		if (!growthTime)
		{
			bodyPositions.RemoveAt(bodyPositions.Count - 1);
		}
		else
		{
			growthTime = false;
			line.positionCount++;
		}
		
		headPosition += dir;
		if((int)headPosition.x < 0 || (int)headPosition.y < 0 || (int)headPosition.x == Board.BoardSize || (int)headPosition.y == Board.BoardSize)
		{
			GameOver();
		}
		if(bodyPositions.Contains(headPosition))
		{
			GameOver();
		}
		Vector3[] tmpLinePos = new Vector3[line.positionCount];
		Debug.Log(tmpLinePos.Length);
		tmpLinePos[0] = Head.position;
		tmpLinePos[1] = Head.position;
		tmpLinePos[line.positionCount -1] = bodyTransforms[line.positionCount - 2].position;
		for (int i = 2; i < line.positionCount-1; i++)
		{
			tmpLinePos[i] = bodyTransforms[i - 2].position;
		}
		line.SetPositions(tmpLinePos);
	}
	void Move()
	{
		
		Head.position = Vector2.Lerp(Board.BoardMatrix[(int)lastHeadPosition.x, (int)lastHeadPosition.y], 
			Board.BoardMatrix[(int)headPosition.x, (int)headPosition.y], 1 - (nextDestinationsChange - Time.time) / MoveCooldown);
		
		for (int i = 0; i < lastBodyPositions.Count; i++)
		{
			bodyTransforms[i].position = Vector2.Lerp(Board.BoardMatrix[(int)lastBodyPositions[i].x, (int)lastBodyPositions[i].y],
				Board.BoardMatrix[(int)bodyPositions[i].x, (int)bodyPositions[i].y], 1 - (nextDestinationsChange - Time.time) / MoveCooldown);
		}

		line.SetPosition(0, new Vector3(Head.position.x, Head.position.y, 0f));
		line.SetPosition(line.positionCount-1, new Vector3(bodyTransforms[lastBodyPositions.Count-1].position.x,
			bodyTransforms[lastBodyPositions.Count-1].position.y, 0f));

	}
	public void Grow()
	{
		growthTime = true;
		//bodyPositions.Add(bodyPositions.FindLastIndex - new Vector2(0, i + 1));
		GameObject tmpBody = Instantiate(BodyPrefab, Board.BoardMatrix[(int)bodyPositions[bodyPositions.Count-1].x, 
			(int)bodyPositions[bodyPositions.Count - 1].y], transform.rotation, transform);
		bodyTransforms.Add(tmpBody.GetComponent<Transform>());
		bodySR.Add(tmpBody.GetComponent<SpriteRenderer>());
		length++;
	}
	public void GameOver()
	{
		Debug.Log("gameover");
	}
	/*
	public void CorrectDraw()
	{
		Quaternion tmpRot = new Quaternion();
		switch (snakeState)
		{
			case State.up:
				tmpRot = Quaternion.Euler(0f,0f,0f);
				break;
			case State.down:
				tmpRot = Quaternion.Euler(0f, 0f, 180f);
				break;
			case State.left:
				tmpRot = Quaternion.Euler(0f, 0f, 90f);
				break;
			case State.right:
				tmpRot = Quaternion.Euler(0f, 0f, 270f);
				break;
		}
		Head.rotation = Quaternion.Lerp(lastRot, tmpRot, 1 - (nextDestinationsChange - Time.time) / MoveCooldown);
	}
	*/
}
