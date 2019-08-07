using LunarByte.MVVM.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = nameof(LevelGameConfig), menuName = "LevelGeneration/GameConfig")]
public class LevelGameConfig : ScriptableObjectInstaller
{
    [SerializeField] private PrefabSettingsWall PrefabSettings;
    [SerializeField] private LevelDatasContainer LevelDatasContainer; // Has IsInitializedCorrectly() checker script
    public override void InstallBindings()
    {
        Debug.Log($"Configuring {nameof(LevelGameConfig)}");
        PrefabSettings.AssertNotNull();
		PrefabSettings.OnValidate();

        Container.BindPoolContainer<TileBase, TileBaseFactory>(PrefabSettings.TilePrefabList, PrefabSettings.TilePoolInitialSizes);
        Container.BindPoolContainer<WallBase, WallBaseFactory>(PrefabSettings.WallPrefabList, PrefabSettings.WallPoolInitialSizes);

        LevelDatasContainer.AssertNotNull();
        LevelDatasContainer.IsInitializedCorrectly();
        Container.Bind<ILevelDatasContainer>().FromInstance(LevelDatasContainer);

        // Should be a test of it's own
        foreach (var tile in PrefabSettings.TilePrefabList)
        {
            tile.GetComponent<ICheckable>().Check();
        }
    }
}

public class TileBaseFactory : PlaceholderFactory<int, TileBase> { }

public class WallBaseFactory : PlaceholderFactory<int, WallBase> { }

public static class ContainerExtensionsWall
{
    public static void BindPoolContainer<TPoolable, PoolFactory>(this DiContainer container, GameObject[] prefabList, int[] initialPoolSizes)
        where TPoolable : PoolableMonoBehaviour
    where PoolFactory : PlaceholderFactory<int, TPoolable>
    {

        PoolablePoolContainer<TPoolable> poolContainer = new PoolablePoolContainer<TPoolable>(prefabList, container, initialPoolSizes);

        container.BindFactory<int, TPoolable, PoolFactory>()
                 .FromMethod((cont, index) =>
                                 poolContainer.SpawnFromPool(index));

        container.Bind<IInitializable>().To<PoolablePoolContainer<TPoolable>>().FromInstance(poolContainer);
    }
}