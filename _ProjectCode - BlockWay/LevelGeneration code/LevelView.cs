using System.Collections;
using System.Collections.Generic;
using LunarByte.MVVM;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using GameAnalyticsSDK;
using MEC;
using TMPro;

public class LevelView : View<ILevelViewModel>
{
	[Inject]         private readonly LevelObjectFactory LevelObjectFactory;
	[SerializeField] private          GameObject         LevelUI;
	[SerializeField] private          GameObject         MainMenuUI;
	[SerializeField] private          Transform          SawParent;
    [SerializeField] private Button StartButton;
    [SerializeField] private Transform Scroller;
    private int ObjectCount;

    // From VisualSettings
    [HideInInspector] public float SpawnDistance;
    [HideInInspector] public float LevelEndBlockDistance;
    [HideInInspector] public int SlotWidth;
    [HideInInspector] public float LevelWidth;

    // Background
    [SerializeField] private GameObject GroundPrefab;
	[HideInInspector] public float GroundLength;
	private GameObject[] GroundTiles;
	private float DistanceTravelled;
	private int ActiveTileIndex;

    // Feedback Text
    [SerializeField] private GameObject LevelFailTextPrefab;
    [SerializeField] private GameObject LevelCompleteTextPrefab;
    [SerializeField] private GameObject SuperSawTextPrefab;
    private bool ValuesSet;

	// Color Settings
	[SerializeField] private Material BasicObjectMaterial;
	[SerializeField] private Material SuperObjectMaterial;
	[SerializeField] private Color[] BasicColors;

	public void OnBasicColorChanged(int color)
	{
		OnColorChanged(color, BasicObjectMaterial);
	}

	public void OnSuperColorChanged(int color)
	{
		OnColorChanged(color, SuperObjectMaterial);
	}

	private void OnColorChanged(int color, Material material)
	{
		if (BasicColors.Length == 0 || color < 0)
		{
			return;
		}
		material.color = BasicColors[color % BasicColors.Length];
    }

    protected override void OnViewAwake()
	{
		StartButton.onClick.AddListener(() => OnLevelStateChange(LevelState.Play));
	}

    protected override void OnViewStart()
    {
        ValuesSet = true;

        Quaternion groundRotation = GroundPrefab.transform.rotation;
        GroundTiles = new GameObject[3]
        {
            GroundPrefab.Spawn(transform, transform.position, groundRotation),
            GroundPrefab.Spawn(transform, transform.position + Vector3.forward * GroundLength, groundRotation),
            GroundPrefab.Spawn(transform,  transform.position + Vector3.forward * GroundLength * 2, groundRotation)
        };
    }

    public void StartLevel()
	{
		ClearLevelObjects();
		SetStartButtonState(true);
		MainMenuUI.SetActive(false);
		LevelUI.SetActive(true);
		ViewModel.GenerateLevel.Dispatch();
    }

    public void ResetLevelUI(bool levelActive)
    {
	    MainMenuUI.SetActive(!levelActive);
	    LevelUI.SetActive(levelActive);
    }

    public void OnLevelStateChange(LevelState levelState)
    {
	    if (levelState == LevelState.Restart)
	    {
		    ResetLevelUI(false);
		    ClearLevelObjects();
		    SetStartButtonState(true);
		    return;
	    }
	    if (levelState.IsPaused())
	    {
		    if (ValuesSet)
		    {
			    MainMenuUI.SetActive(false);
			    LevelUI.SetActive(false);
            }
		    return;
	    }
        ResetLevelUI(true);
	    ViewModel.GenerateLevel.Dispatch();
    }

    public void SetStartButtonState(bool state)
	{
		if (state)
		{
			StopAllCoroutines();
			EndLevel();
		}
		StartButton.gameObject.SetActive(state);
	}

	private void ClearLevelObjects()
	{
		StopAllCoroutines();
		LevelObject[] levelObjects = Scroller.GetComponentsInChildren<LevelObject>();

        for (var i = 0; i < levelObjects.Length; i++)
		{
			levelObjects[i].gameObject.Recycle();
		}
	}

	public void EndLevel()
	{
		SetStartButtonState(false);
		MainMenuUI.SetActive(true);
		LevelUI.SetActive(false);
	}

    public void ShowEndText(bool active)
    {
        if (!ValuesSet || !active)
            return;

        if(ViewModel.LevelState == LevelState.PauseComplete)
        {
            var feedbackObject = Instantiate(LevelCompleteTextPrefab, LevelUI.transform);
            feedbackObject.GetComponent<FeedbackText>().SetText(ViewModel.LevelCompleteText);
        } else if(ViewModel.LevelState == LevelState.PauseFail)
        {
            var feedbackObject = Instantiate(LevelFailTextPrefab, LevelUI.transform);
            feedbackObject.GetComponent<FeedbackText>().SetText(ViewModel.LevelFailText);
        }
    }

    public void ShowSuperSawText(bool active)
    {
        if (!ValuesSet || !active)
            return;

        var feedbackObject = Instantiate(SuperSawTextPrefab, LevelUI.transform);
        feedbackObject.GetComponent<FeedbackText>().SetText(ViewModel.SuperSawText);
    }

    private void Update()
	{
		if (ViewModel.LevelState != LevelState.PauseFail)
		{
			Move();
		}
		CheckGround();
	}

	private void Move()
	{
		float distance = ViewModel.ScrollSpeed * Time.deltaTime;
		DistanceTravelled += distance;
		SawParent.position += Vector3.forward * distance;
    }

	private void CheckGround()
	{
		if (DistanceTravelled >= GroundLength)
		{
			DistanceTravelled -= GroundLength;
			GroundTiles[ActiveTileIndex].transform.position += Vector3.forward * GroundLength * GroundTiles.Length;
			ActiveTileIndex = (ActiveTileIndex + 1) % GroundTiles.Length;
		}
	}

	public void OnLevelChanged(LevelObjectType[] levelObjectTypeContainer)
	{
		if (ViewModel == null)
		{
			return;
		}
		Vector3 spawnOrigin = SawParent.transform.position;
		spawnOrigin.y = 0;

        // Timing.RunCoroutine(_SpawnLevelObjects(levelObjectTypeContainer, spawnOrigin));
        if (levelObjectTypeContainer != null)
        {
	        SpawnLevelObjects(levelObjectTypeContainer, spawnOrigin);
        }
    }

	private void SpawnLevelObjects(LevelObjectType[] levelObjectTypeContainer, Vector3 spawnOrigin)
	{
        int objectCount = levelObjectTypeContainer.Length;

        int gridWidth = ViewModel.GridWidth; // Number of objects per slot in X-axis
        float widthMultiplier = LevelWidth / ViewModel.GridWidth;
        float edge = widthMultiplier / 2.0f; // from center to "edge" of a slot, essentially half a slot
        float gridLeftStartPos = -LevelWidth / 2.0f + edge; // Start "Left" X-axis position of Slots


        LevelObjectSpawnData spawnData = new LevelObjectSpawnData();
        spawnData.SetIndependentData(widthMultiplier, SlotWidth, edge);

        for (var i = 0; i < objectCount; i++)
		{
            spawnData.SetIndexDependentData(i, gridLeftStartPos, gridWidth, widthMultiplier);
            SpawnLevelObject(levelObjectTypeContainer[i], spawnOrigin, spawnData);
        }

        spawnData.SetEndBlockIndexDependentData(levelObjectTypeContainer.Length / gridWidth, widthMultiplier);
        SpawnLevelObject(LevelObjectType.EndLevelBlock, spawnOrigin, spawnData);
    }

	private IEnumerator<float> _SpawnLevelObjects(LevelObjectType[] levelObjectTypeContainer, Vector3 spawnOrigin)
	{
        int objectCount = levelObjectTypeContainer.Length;

        int gridWidth = ViewModel.GridWidth; // Number of objects per slot in X-axis
        float widthMultiplier = LevelWidth / ViewModel.GridWidth;
        float edge = widthMultiplier / 2.0f; // from center to "edge" of a slot, essentially half a slot
        float gridLeftStartPos = -LevelWidth / 2.0f + edge; // Start "Left" X-axis position of Slots


        LevelObjectSpawnData spawnData = new LevelObjectSpawnData();
        spawnData.SetIndependentData(widthMultiplier, SlotWidth, edge);

        for (var i = 0; i < objectCount; i++)
        {
            spawnData.SetIndexDependentData(i, gridLeftStartPos, gridWidth, widthMultiplier);
            SpawnLevelObject(levelObjectTypeContainer[i], spawnOrigin, spawnData);
            yield return 0;
        }
        spawnData.SetEndBlockIndexDependentData(levelObjectTypeContainer.Length / gridWidth, widthMultiplier);
        SpawnLevelObject(LevelObjectType.EndLevelBlock, spawnOrigin, spawnData);
    }

    private void SpawnLevelObject(LevelObjectType levelObjectType, Vector3 spawnOrigin, LevelObjectSpawnData sd)
    {
        switch (levelObjectType)
        {
            case LevelObjectType.Hay:
            case LevelObjectType.SuperHay:
                for (float xAdjust = sd.startAdjustment; xAdjust < sd.edge; xAdjust += sd.distanceBetween)
                {
                    for (float zAdjust = sd.startAdjustment; zAdjust < sd.edge; zAdjust += sd.distanceBetween)
                    {
                        sd.positionModifications = new Vector3(
                                             sd.modifiedPositionIndex + xAdjust,
                                             0,
                                             SpawnDistance + sd.modifiedHeight + zAdjust);
                        CreateLevelObject(spawnOrigin, sd.positionModifications, levelObjectType, sd.objectIndex);
                    }
                }

                break;
            case LevelObjectType.EndLevelBlock:
                sd.positionModifications = new Vector3(sd.modifiedPositionIndex,
                                                    0,
                                                    SpawnDistance + sd.modifiedHeight + LevelEndBlockDistance);
                CreateLevelObject(spawnOrigin, sd.positionModifications, levelObjectType, sd.objectIndex);

                break;
            case LevelObjectType.Rock:
                sd.positionModifications = new Vector3(sd.modifiedPositionIndex,
                                                    0,
                                                    SpawnDistance + sd.modifiedHeight);
                CreateLevelObject(spawnOrigin, sd.positionModifications, levelObjectType, sd.objectIndex);

                break;
        }
    }

    private void CreateLevelObject(Vector3 spawnOrigin, Vector3 positionModifications, LevelObjectType objectType, int objectIndex)
    {
        var objectPrefab = LevelObjectFactory.Create(objectType);
        Vector3 objectPosition = spawnOrigin + positionModifications + new Vector3(0, objectPrefab.transform.localPosition.y, 0);
        var levelObject = objectPrefab.Spawn(Scroller, objectPosition);
        levelObject.GetComponent<LevelObject>().ResetValues(objectIndex);
    }
}

// We want to pass by reference always
public class LevelObjectSpawnData
{
    public int objectIndex; // Index of LevelObject
    public float modifiedPositionIndex; // Scaled X-axis Center of current slot
    public float modifiedHeight; // Scaled Z-axis of current slot

    public float distanceBetween; // Distance between actual objects inside a slot
    public float edge; // Half X-axis length of a slot
    public float startAdjustment; // Beginning "Left" X-axis position inside a slot

    public Vector3 positionModifications; // Variable for modifications assigned during spawning

    public void SetIndependentData(float widthMultiplier, float SlotWidth, float slotEdge)
    {
        distanceBetween = widthMultiplier / SlotWidth;
        edge = slotEdge;
        startAdjustment = -edge + distanceBetween / 2.0f;
    }

    public void SetIndexDependentData(int index, float gridLeftStartPos, int gridWidth, float widthMultiplier)
    {
        objectIndex = index;
        modifiedPositionIndex = gridLeftStartPos + (index % gridWidth) * widthMultiplier;
        modifiedHeight = (index / gridWidth) * widthMultiplier;
    }

    public void SetEndBlockIndexDependentData(int finalHeight, float widthMultiplier)
    {
        objectIndex = -1;
        modifiedPositionIndex = 0;
        modifiedHeight = finalHeight * widthMultiplier;
    }
}
