using System;
using System.Collections;
using System.Collections.Generic;
using LunarByte.MVVM;
using UnityEngine;
using Zenject;


[CreateAssetMenu(fileName = nameof(ContainerConfiguration), menuName = ("Configurations/"+nameof(ContainerConfiguration)))]
public class ContainerConfiguration : ScriptableObjectInstaller
{
    [SerializeField] private PrefabSettings prefabSettings;

    public override void InstallBindings()
    {
	Debug.Log("Configure " + nameof(ContainerConfiguration));
	DynamicModelContainer<TestModel> modelContainer = new DynamicModelContainer<TestModel>();
	modelContainer.Add(new TestModel(){Value = 0});
	modelContainer.Add(new TestModel() { Value = 1 });


	Container.Bind<PrefabSettings>().FromInstance(prefabSettings);
	Container.Bind<DynamicModelContainer<TestModel>>().FromInstance(modelContainer).AsSingle();
	Container.BindPoolContainer<TestView, ITestViewModel, TestViewFactory>(prefabSettings.PrefabList);	
    }
}

public class ViewPoolFactory<T, TViewModel> : PlaceholderFactory<int, TViewModel, T> { }

public class TestViewFactory : ViewPoolFactory<TestView, ITestViewModel> { }


public static class ContainerExtensionVerTest
{
    public static void BindPoolContainer<TView, TViewModel, ViewPoolFactory>(this DiContainer container, GameObject[] prefabList)
 	    where TView: DynamicView<TViewModel>
	    where ViewPoolFactory : ViewPoolFactory<TView,TViewModel>
    {
	ViewPoolContainer<TView, TViewModel> viewPoolContainer = new ViewPoolContainer<TView, TViewModel>(prefabList, container);

        container.BindFactory<int, TViewModel, TView, ViewPoolFactory>()
		 .FromMethod((cont, index, viewModel) => viewPoolContainer.SpawnFromPool(index, viewModel));
    }

    public static void BindPoolContainer<T, TViewModel>(this DiContainer container, GameObject[] prefabList)
	    where T : DynamicView<TViewModel>
    {
	ViewPoolContainer<T, TViewModel> viewPoolContainer = new ViewPoolContainer<T, TViewModel>(prefabList, container);

	container.BindFactory<int, TViewModel, T, ViewPoolFactory<T, TViewModel>>()
		 .FromMethod((cont, index, viewModel) => viewPoolContainer.SpawnFromPool(index, viewModel));
    }
}
