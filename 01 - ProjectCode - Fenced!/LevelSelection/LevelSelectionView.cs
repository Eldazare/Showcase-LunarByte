using LunarByte.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class LevelSelectionView : View<ILevelSelectionViewModel>
{
    [SerializeField] private Transform GridLayoutParent;
    [SerializeField] private Button OpenButton;
    [SerializeField] private Button CloseButton;

    [SerializeField] public UnityEvent OpenButtonFunctionality;
    [SerializeField] public UnityEvent CloseButtonFunctionality;


    private List<LevelSelectButton> ButtonList;

    //From Inject
    private ILevelDatasContainer LevelDatasContainer;
    private LevelSelectButtonPool PrefabPool;

    [Inject]
    public void SetLevelSelectButtons(ILevelDatasContainer levelDatasContainer, LevelSelectButtonPool pool)
    {
        LevelDatasContainer = levelDatasContainer;
        PrefabPool = pool;
    }

    protected override void OnViewStart()
    {
        InitializeLevelSelectionButtons();
        InitializeOpenAndCloseButtons();
        CloseButtonFunctionality.Invoke();
    }

    private void InitializeLevelSelectionButtons()
    {
        ButtonList = new List<LevelSelectButton>();
        for (int i = 0; i < LevelDatasContainer.LevelDatas.Length; i++)
        {
            int index = i;
            LevelSelectButton button = PrefabPool.Spawn(() => LevelSelectAction(index, true), $"{i+1}");
            button.transform.SetParent(GridLayoutParent, false);
            ButtonList.Add(button);
        }
    }

    private void InitializeOpenAndCloseButtons()
    {
        OpenButton.onClick.RemoveAllListeners();
        OpenButton.onClick.AddListener(OpenButtonFunctionality.Invoke);
        CloseButton.onClick.RemoveAllListeners();
        CloseButton.onClick.AddListener(CloseButtonFunctionality.Invoke);
    }

    private void LevelSelectAction(int index, bool autoGenerateLevel = false)
    {
        ViewModel.LoadLevelData.Dispatch(index);
        if (autoGenerateLevel)
        {
            ViewModel.GenerateLevel.Dispatch();
        }
        CloseButtonFunctionality.Invoke();
    }
}



public class LevelSelectButtonPool : PoolableMonoPool<Action, string, LevelSelectButton> { }