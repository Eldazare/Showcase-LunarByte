using LunarByte.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelViewWall : View<ILevelViewModelWall>
{
    [SerializeField] private PositionResetComponent PlayerPositionResetComponent;
    [SerializeField] private Transform ObjectParent;
    [SerializeField] private Transform TileParent;
    [SerializeField] private Transform WallParent;
    [SerializeField] private Transform BlackFloorParent;

    // Injected
    private TileBaseFactory TileFactory; // PoolContainer
    private WallBaseFactory WallFactory; // PoolContainer


    private float SpacingMultiplier = 0.7f;
    

    // Parsed LevelData
    private int Size;
    private Tuple<int, int> StartPos;
    private int[] Tiles;
    private int[] Walls;
    Vector3 ResetPosition;

    private WallEndLevelDoor EndDoor;

    // Containers for Active Components
    private List<TileBase> LevelTiles = new List<TileBase>();
    private List<WallBase> LevelWalls = new List<WallBase>();


    [Inject]
    public void GetFactories(TileBaseFactory tileFactory, WallBaseFactory wallFactory)
    {
        TileFactory = tileFactory;
        WallFactory = wallFactory;
    }


    protected override void OnViewAwake()
    {
        DisposeLevel();
        ObjectParent.localScale = new Vector3(SpacingMultiplier, 1, SpacingMultiplier);
        PlayerPositionResetComponent.transform.localScale *= SpacingMultiplier;
	GenerateLevel();
    }

    public void Resetlevel()
    {
	if (LevelTiles.Count > 0)
        {
            ResetPlayerToStartPosition();
            foreach (var tile in LevelTiles)
            {
                tile.ResetToDefault();
            }

            foreach (var wall in LevelWalls)
            {
                wall.ResetToDefault();
            }
        }
    }

    private void GenerateLevel()
    {
	if (LevelTiles.Count > 0)
        {
            DisposeLevel();
        }
        GenerateTiles();
        GenerateWalls();
	SearchForLevelEndDoor();
        SetTileObjectMaximums();
        ResetPlayerToStartPosition();
    }

    private void DisposeLevel()
    {
	foreach (var tile in LevelTiles)
        {
            tile.Dispose();
        }

        foreach (var wall in LevelWalls)
        {
            wall.Dispose();
        }
        PlayerPositionResetComponent.gameObject.SetActive(false);
        BlackFloorParent.gameObject.SetActive(false);
        LevelTiles.Clear();
        LevelWalls.Clear();
    }

    private void SetTileObjectMaximums()
    {
        int[] keysMax = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int collectiblesMax = 0;
        int index = 0;
        foreach (var tile in LevelTiles)
        {

            switch(tile.TileObjectType(out index))
            {
                case TileItemEnum.None:
                    break;
                case TileItemEnum.Collectible:
                    collectiblesMax++;
                    break;
                case TileItemEnum.Key:
                    keysMax[index]++;
                    break;
            }
        }

        ViewModel.UpdateMaxTileObjects.Dispatch(keysMax, collectiblesMax);
    }

    private void ResetPlayerToStartPosition()
    {
        ViewModel.ResetInput.Dispatch();
        ViewModel.ResetPlayerModel.Dispatch();
        PlayerPositionResetComponent.gameObject.SetActive(true);
        PlayerPositionResetComponent.ResetPositionTo(ResetPosition);
    }

    private void GenerateTiles()
    {
        float leftBegin = (float)Size / 2 - 0.5f;
        for (int i = 0; i < Size; i++)
        {
            float zPos = leftBegin - i;
            for (int j = 0; j < Size; j++)
            {
                float xPos = -leftBegin + j;
                var tile = TileFactory.Create(Tiles[i * Size + j]);
                tile.transform.SetParent(TileParent, false); 
                tile.transform.localPosition = new Vector3(xPos, 0, zPos);
                LevelTiles.Add(tile);
            }
        }
	BlackFloorParent.gameObject.SetActive(true);
	BlackFloorParent.transform.localScale = new Vector3(Size, 1, Size);
    }

    private void GenerateWalls()
    {
        float leftBegin = Size / 2.0f;
        for (int i = 0; i < 2*Size+1; i++)
        {
            float adjustX = ((i + 1) % 2);
            float zPos = leftBegin - (i / 2.0f);
            int AdjustedJTop = Size + (i % 2);
            float adjustXPos = adjustX * 0.5f;
            for (int j = 0; j < AdjustedJTop; j++)
            {
                float xPos = -leftBegin + j + adjustXPos;
                var wallIndex = Walls[i * (Size+1) + j];
                if (wallIndex >= 0)
                {
                    var wall = WallFactory.Create(wallIndex);
                    wall.transform.SetParent(WallParent, false); 
                    wall.transform.localPosition = new Vector3(xPos, 0, zPos);
                    wall.transform.localEulerAngles = new Vector3(0, 90.0f * adjustX, 0) ;
                    LevelWalls.Add(wall);
                }
            }
        }
    }

    private void SearchForLevelEndDoor()
    {
	foreach (var wall in LevelWalls)
	{
		if (wall.GetType() == typeof(WallEndLevelDoor))
		{
			EndDoor = (WallEndLevelDoor)wall;

			return;
		}
	}
	Debug.LogError("Level did not contain EndLevelDoor.");
    }

    public void AttemptRemoveEndDoorKeys(int levelEndKeyIndex)
    {
	ViewModel.AttemptRemoveAllKeys.Dispatch(levelEndKeyIndex, OpenEndDoor);
    }

    private void OpenEndDoor()
    {
	EndDoor.OpenDoor();
    }

    public void LevelDataChanged(ILevelDataWall levelData)
    {
        Size = levelData.Size;
        StartPos = new Tuple<int, int>(levelData.StartPositionX, levelData.StartPositionY);
        Tiles = levelData.Tiles;
        Walls = levelData.Walls;
        float leftBegin = (float)Size / 2 - 0.5f;
        ResetPosition = new Vector3(-leftBegin + StartPos.Item1, 0, leftBegin - StartPos.Item2) * SpacingMultiplier;
    }

    public void ReceiveLevelCommand(LevelCommand command)
    {
        switch (command)
        {
            case LevelCommand.None:
                break;
            case LevelCommand.Generate:
                GenerateLevel();
                break;
            case LevelCommand.Reset:
                Resetlevel();
                break;
            case LevelCommand.Dispose:
                DisposeLevel();
                break;
        }
    }
}
