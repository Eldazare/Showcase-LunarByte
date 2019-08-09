using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using LunarByte.MVVM;
using LunarByte.MVVM.Configurations;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

[CreateAssetMenu(fileName = nameof(TestDynamicViewConfiguration),
	menuName = "Configurations/" + nameof(TestDynamicViewConfiguration))]
public class TestDynamicViewConfiguration : DynamicViewConfiguration<DynamicModelContainer<TestModel>, DynamicViewModelContainer<TestViewModel>, TestViewContainer, TestViewFactory>
{
	protected override void Configure(DynamicModelContainer<TestModel>         mContainer,
	                                  DynamicViewModelContainer<TestViewModel> vmContainer,
	                                  TestViewContainer                        vContainer,
	                                  TestViewFactory factory)
	{
		Debug.Log("Configure " + nameof(TestDynamicViewConfiguration));

		// DEBUG REFLECTION LISTENING START ------------------------------------------------------------------------------
		//var propertyInfo =
		//    typeof(DynamicModelContainer<TestModel>).GetField("MutableCollection",
		//                                                         BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		//var fieldInfoVm = typeof(DynamicViewModelContainer<TestViewModel>).GetField("MutableCollection",
		//                                                                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

		//if (fieldInfoVm == null || propertyInfo == null)
		//{
		//    Debug.LogError("PropertyInfo is null.");
		//}
		//else
		//{
		//    var mContainerProperty =
		//        (ObservableCollection<TestModel>)propertyInfo.GetValue(mContainer);

		//    var vmContainerProperty =
		//        (ObservableCollection<TestViewModel>)fieldInfoVm.GetValue(vmContainer);

		//    mContainerProperty.CollectionChanged += MockListenerModel;

		//    vmContainerProperty.CollectionChanged += MockListenerViewModel;
		//}

		//Debug.Log("MC Listeners: " + mContainer.Listeners.Count + " | Subscriptions: " + mContainer.Subscriptions.Count);
		//Debug.Log("VMC Listeners: " + vmContainer.Listeners.Count + " | Subscriptions: " + vmContainer.Subscriptions.Count);
		// DEBUG REFLECTION LISTENING END ---------------------------------------------------------------------------------------

		vmContainer.BindContainer().ToContainer(mContainer, m => TestVMFactory(m));

		vContainer.BindContainer().ToContainer(vmContainer,
		                                       vm => TestViewFactory(
			                                       vm, factory,
			                                       vContainer.transform));
		
	}

	public void MockListenerModel(object sender, NotifyCollectionChangedEventArgs args)
	{
		Debug.Log("MODEL COLLECTION CHANGED: " + args.Action);
	}

	public void MockListenerViewModel(object sender, NotifyCollectionChangedEventArgs args)
	{
		Debug.Log("VIEWMODEL COLLECTION CHANGED: " + args.Action);
	}

	private TestView TestViewFactory(TestViewModel                                  vm,
	                                 TestViewFactory factory,
	                                 Transform                                      parent)
	{
		TestView retVal = factory.Create(vm.Value,vm); 
		retVal.transform.SetParent(parent);
		retVal.someValue = vm.Value;

		retVal.Bind<TestView, int>((int x) => retVal.ChangeSomeValue(x)).ToProperty(vm, v => v.Value, nameof(TestViewModel));

		return retVal;
	}

	private TestViewModel TestVMFactory(TestModel model)
	{
		var retVal = new TestViewModel { Value = model.Value };
		retVal.Bind<TestViewModel, int>(Value => retVal.Value = Value).ToProperty(model, m => m.Value, nameof(TestModel));

		return retVal;
	}



}

public class ConfMockup<T, TViewModel>
{

	public ConfMockup(ViewPoolFactory<T, TViewModel> factor)
	{
		factory = factor;
	}

	private ViewPoolFactory<T, TViewModel> factory;
	private T ViewFactory(int index, TViewModel vm)
	{
		return factory.Create(index, vm);
	}

}





