using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
	[Header("Colors")]
	public Color A;
	public Color B;
	[Header("Parameters")]
	public int BoardSize = 9;
	public float TileSize = 1;
	public GameObject tile;

	public Vector2[,] BoardMatrix;
	[HideInInspector]
	public Vector2 middle;

	private int nextColor = 0;
	private void Awake()
	{
		BoardMatrix = new Vector2[BoardSize, BoardSize];
		nextColor = 0;
		for (int i = 0; i < BoardSize; i++)
		{
			for (int j = 0; j < BoardSize; j++)
			{
				Vector3 newTilePos = transform.position + new Vector3((i - BoardSize / 2) * TileSize, (j - BoardSize / 2) * TileSize);
				BoardMatrix[i, j] = new Vector2(newTilePos.x, newTilePos.y);
				GameObject newTile = Instantiate(tile, newTilePos, transform.rotation, transform);
				SpriteRenderer sr = newTile.GetComponent<SpriteRenderer>();
				Transform trans = newTile.GetComponent<Transform>();
				if(nextColor++%2 == 0)
				{
					sr.color = A;
				}
				else
				{
					sr.color = B;
				}
				trans.localScale = new Vector3(TileSize, TileSize, 1);
			}
		}
		middle = new Vector2(BoardSize / 2, BoardSize / 2);
	}
}
