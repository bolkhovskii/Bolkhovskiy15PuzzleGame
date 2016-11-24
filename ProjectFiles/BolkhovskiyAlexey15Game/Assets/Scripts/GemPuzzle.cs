using UnityEngine;
using System.Collections;

public class GemPuzzle : MonoBehaviour {
	public int ID; 

	void ReplaceBlocks(int x, int y, int XX, int YY)
	{
		GameManager.grid[x,y].transform.position = GameManager.position[XX,YY];
		GameManager.grid[XX,YY] = GameManager.grid[x,y];
		GameManager.grid[x,y] = null;
		GameManager.click++;
		GameManager.GameFinish();
	}

	void OnMouseDown()
	{
		for(int y = 0; y < 4; y++)
		{
			for(int x = 0; x < 4; x++)
			{
				if(GameManager.grid[x,y])
				{
					if(GameManager.grid[x,y].GetComponent<GemPuzzle>().ID == ID)
					{
						if(x > 0 && GameManager.grid[x-1,y] == null)
						{
							ReplaceBlocks(x,y,x-1,y);
							return;
						}
						else if(x < 3 && GameManager.grid[x+1,y] == null)
						{
							ReplaceBlocks(x,y,x+1,y);
							return;
						}
					}
				}
				if(GameManager.grid[x,y])
				{
					if(GameManager.grid[x,y].GetComponent<GemPuzzle>().ID == ID)
					{
						if(y > 0 && GameManager.grid[x,y-1] == null)
						{
							ReplaceBlocks(x,y,x,y-1);
							return;
						}
						else if(y < 3 && GameManager.grid[x,y+1] == null)
						{
							ReplaceBlocks(x,y,x,y+1);
							return;
						}
					}
				}
			}
		}
	}
}