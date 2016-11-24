using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public GameObject[] _gameOfFifteen;
	public float startPosX = -6f;
	public float startPosY = 6f;
	public float outX = 1.1f;
	public float outY = 1.1f;
	public Text _textyText;
	public static int click;
	public static GameObject[,] grid;
	public static Vector3[,] position;
	private GameObject[] puzzleRandom;
	public static bool youwin;

	void Start () 
	{
		puzzleRandom = new GameObject[_gameOfFifteen.Length];
		float posXreset = startPosX;
		position = new Vector3[4,4];
		for(int j = 0; j < 4; j++)
		{
			startPosY -= outY;
			for(int i = 0; i < 4; i++)
			{
				startPosX += outX;
				position[i,j] = new Vector3(startPosX, startPosY, 0);
			}
			startPosX = posXreset;
		}

		if(!PlayerPrefs.HasKey("Puzzle")) StartNewGame(); else Load();
	}

	public void StartNewGame()
	{
		youwin = false;
		click = 0;
		RandomPuzzle();
		Debug.Log("New Game");
	}

	public void ExitGame()
	{
		Save();
		Application.Quit();
	}

	void Save()
	{
		string content = string.Empty;
		for(int y = 0; y < 4; y++)
		{
			for(int x = 0; x < 4; x++)
			{
				if(content.Length > 0) content += "|";
				if(grid[x,y]) content += grid[x,y].GetComponent<GemPuzzle>().ID.ToString(); else content += "null";
			}
		}
		PlayerPrefs.SetString("Puzzle", content);
		PlayerPrefs.SetString("PuzzleInfo", click.ToString());
		Debug.Log(this + " saving Game of 15.");
	}

	void Load()
	{
		string[] content = PlayerPrefs.GetString("Puzzle").Split(new char[]{'|'});

		if(content.Length == 0 || content.Length != 16) return;

		if(PlayerPrefs.HasKey("PuzzleInfo")) click = Parse(PlayerPrefs.GetString("PuzzleInfo"));

		grid = new GameObject[4,4];
		int i = 0;
		for(int y = 0; y < 4; y++)
		{
			for(int x = 0; x < 4; x++)
			{
				int j = FindPuzzle(Parse(content[i]));

				if(j >= 0)
				{
					grid[x,y] = Instantiate(_gameOfFifteen[j], position[x,y], Quaternion.identity) as GameObject;
					grid[x,y].name = "ID-"+i;
					grid[x,y].transform.parent = transform;

				}
				i++;
			}
		}
	}

	int FindPuzzle(int index)
	{
		int j = 0;
		foreach(GameObject e in _gameOfFifteen)
		{
			if(e.GetComponent<GemPuzzle>().ID == index) return j;
			j++;
		}
		return -1;
	}

	int Parse(string text)
	{
		int value;
		if(int.TryParse(text, out value)) return value;
		return -1;
	}

	void CreatePuzzle()
	{
		if(transform.childCount > 0)
		{
			for(int j = 0; j < transform.childCount; j++)
			{
				Destroy(transform.GetChild(j).gameObject);
			}
		}
		int i = 0;
		grid = new GameObject[4,4];
		int h = Random.Range(0,3);
		int v = Random.Range(0,3);
		GameObject clone = new GameObject();
		grid[h,v] = clone;
		for(int y = 0; y < 4; y++)
		{
			for(int x = 0; x < 4; x++)
			{
				if(grid[x,y] == null)
				{
					grid[x,y] = Instantiate(puzzleRandom[i], position[x,y], Quaternion.identity) as GameObject;
					grid[x,y].name = "ID-"+i;
					grid[x,y].transform.parent = transform;
					i++;
				}
			}
		}
		Destroy(clone); 
		for(int q = 0; q < _gameOfFifteen.Length; q++)
		{
			Destroy(puzzleRandom[q]);
		}
	}

	static public void GameFinish()
	{
		int i = 1;
		for(int y = 0; y < 4; y++)
		{
			for(int x = 0; x < 4; x++)
			{
				if(grid[x,y]) { if(grid[x,y].GetComponent<GemPuzzle>().ID == i) i++; } else i--;
			}
		}
		if(i == 15)
		{
			for(int y = 0; y < 4; y++)
			{
				for(int x = 0; x < 4; x++)
				{
					if(grid[x,y]) Destroy(grid[x,y].GetComponent<GemPuzzle>());
				}
			}
			youwin = true;
			Debug.Log("Finish!");
		}
	}
		
	void RandomPuzzle()
	{
		int[] tmp = new int[_gameOfFifteen.Length];
		for(int i = 0; i < _gameOfFifteen.Length; i++)
		{
			tmp[i] = 1;
		}
		int c = 0;
		while(c < _gameOfFifteen.Length)
		{
			int r = Random.Range(0, _gameOfFifteen.Length);
			if(tmp[r] == 1)
			{ 
				puzzleRandom[c] = Instantiate(_gameOfFifteen[r], new Vector3(0, 10, 0), Quaternion.identity) as GameObject;
				tmp[r] = 0;
				c++;
			}
		}
		CreatePuzzle();
	}

	void LateUpdate () 
	{
		if(!youwin)
		{
			_textyText.text = "Ходов: " + click;
		}
		else
		{
			click = 0;
			_textyText.text = "Властелин пятнашек -- Lord of 15-puzzle";
		}
	}
}