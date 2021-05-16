using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTrail : MonoBehaviour
{
	[Header("Connections")]
	public TrailRenderer Renderer;
	public GameBoard Board;
	public Snake Snake;
	[Header("Parameters")]
	public float DeathAnimTime = 0.15f;
    // Start is called before the first frame update
    void Start()
    {
		Renderer.widthMultiplier = Board.TileSize;
    }

    // Update is called once per frame
    void Update()
    {
		switch(Snake.snakeState)
		{
			case Snake.State.start:
				Renderer.enabled = false;
				break;
			case Snake.State.dead:
				
				break;
			default:
				Renderer.enabled = true;
				Renderer.time = Snake.MoveCooldown * (Snake.length - 1);
				break;
		}
		
    }
	
}
