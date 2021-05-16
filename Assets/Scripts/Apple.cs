using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
	[Header("Connections")]
	public gameManager gameMan;
	public Snake snake;
	public GameBoard Board;
	public ParticleSystem particle;

	public float ThresholdNewSpawnDist = 2f;
	private Vector2 applePosition;
	private bool gameStarted = false;

	private void Start()
	{
		transform.localScale = new Vector3(Board.TileSize, Board.TileSize, 0);
		transform.position = new Vector3(100, 100, 0);
	}
	void Update()
    {
        if(gameStarted)
		{
			if(snake.headPosition == applePosition)
			{
				SpawnParticle(Board.BoardMatrix[(int)applePosition.x, (int)applePosition.y]);
				SpawnApple();
				snake.Grow();
				gameMan.AddPoint();
				gameMan.Bite.Play();
			}
			if (snake.snakeState == Snake.State.dead)
			{
				gameStarted = false;
				transform.position = new Vector3(100, 100, 0);
			}
		}
		else
		{
			if(snake.snakeState != Snake.State.start && snake.snakeState != Snake.State.dead)
			{
				gameStarted = true;
				SpawnApple();
			}
			
		}
    }
	void SpawnApple()
	{
		Vector2 newApplePosition;
		do
		{
			newApplePosition = new Vector2(Random.Range(0, Board.BoardSize), Random.Range(0, Board.BoardSize));
		} while (Vector2.Distance(newApplePosition, applePosition) < ThresholdNewSpawnDist && !snake.bodyPositions.Contains(newApplePosition));
		applePosition = newApplePosition;
		transform.position = Board.BoardMatrix[(int)applePosition.x, (int)applePosition.y];
	}
	void SpawnParticle(Vector2 pos)
	{
		Instantiate(particle, pos, Quaternion.identity);
	}
}
