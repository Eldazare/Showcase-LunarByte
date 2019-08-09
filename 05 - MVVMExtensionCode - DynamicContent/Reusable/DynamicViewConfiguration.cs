using System.Collections;
using System.Collections.Generic;
using LunarByte.MVVM;
using LunarByte.MVVM.Configurations;
using UnityEngine;

public abstract class DynamicViewConfiguration<TModelContainer, TViewModelContainer, TViewContainer> : ViewConfiguration
{
    public override void InstallBindings()
    {
        Container.Bind<TViewContainer>().FromComponentOnRoot();

        Container.Bind<TViewModelContainer>().ToSelf().AsCached()
                 .AfterCreation<TViewModelContainer, TModelContainer, TViewContainer>(
                     (vmContainer, modelContainer, viewContainer) =>
                     {
	                     Configure(modelContainer, vmContainer, viewContainer);
                     }).NonLazy();
    }

    protected abstract void Configure(TModelContainer modelContainer,
                                      TViewModelContainer viewModelContainer,
                                      TViewContainer viewContainer);
}

public abstract class DynamicViewConfiguration<TModelContainer, TViewModelContainer, TViewContainer, TDependency1> : ViewConfiguration
{
    public override void InstallBindings()
    {
        Container.Bind<TViewContainer>().FromComponentOnRoot();

        Container.Bind<TViewModelContainer>().ToSelf().AsCached()
                 .AfterCreation<TViewModelContainer, TModelContainer, TViewContainer, TDependency1
                 >((vmContainer, modelContainer, viewContainer, dep1) =>
                 {
                     Configure(modelContainer, vmContainer, viewContainer, dep1);
                 }).NonLazy();
    }

    protected abstract void Configure(TModelContainer modelContainer,
                                      TViewModelContainer viewModelContainer,
                                      TViewContainer viewContainer,
                                      TDependency1 dependency1);
}

public abstract class DynamicViewConfiguration<TModelContainer, TViewModelContainer, TViewContainer, TDependency1, TDependency2> : ViewConfiguration
{
    public override void InstallBindings()
    {
        Container.Bind<TViewContainer>().FromComponentOnRoot();

        Container.Bind<TViewModelContainer>().ToSelf().AsCached()
                 .AfterCreation<TViewModelContainer, TModelContainer, TViewContainer, TDependency1,
                     TDependency2>((vmContainer, modelContainer, viewContainer, dep1, dep2) =>
                     {
                         Configure(modelContainer, vmContainer, viewContainer, dep1,
                                   dep2);
                     }).NonLazy();
    }

    protected abstract void Configure(TModelContainer modelContainer,
                                      TViewModelContainer viewModelContainer,
                                      TViewContainer viewContainer,
                                      TDependency1 dependency1,
                                      TDependency2 factory);
}

public abstract class DynamicViewConfiguration<TModelContainer, TViewModelContainer,
                             TViewContainer, TDependency1, TDependency2,
                             TDependency3> : ViewConfiguration
{
    public override void InstallBindings()
    {
        Container.Bind<TViewContainer>().FromComponentOnRoot();

        Container.Bind<TViewModelContainer>().ToSelf().AsCached()
                 .AfterCreation<TViewModelContainer, TModelContainer, TViewContainer, TDependency1,
                     TDependency2, TDependency3>(
                     (vmContainer, modelContainer, viewContainer, dep1, dep2, dep3) =>
                     {
                         Configure(modelContainer, vmContainer, viewContainer, dep1, dep2, dep3);
                     }).NonLazy();
    }

    protected abstract void Configure(TModelContainer modelContainer,
                                      TViewModelContainer viewModelContainer,
                                      TViewContainer viewContainer,
                                      TDependency1 dependency1,
                                      TDependency2 dependency2,
                                      TDependency3 dependency3);
}

public abstract class DynamicViewConfiguration<TModelContainer, TViewModelContainer,
                             TViewContainer, TDependency1, TDependency2, TDependency3,
                             TDependency4> : ViewConfiguration
{
    public override void InstallBindings()
    {
        Container.Bind<TViewContainer>().FromComponentOnRoot();

        Container.Bind<TViewModelContainer>().ToSelf().AsCached()
                 .AfterCreation<TViewModelContainer, TModelContainer, TViewContainer, TDependency1,
                     TDependency2, TDependency3, TDependency4>(
                     (vmContainer, modelContainer, viewContainer, dep1, dep2, dep3, dep4) =>
                     {
                         Configure(modelContainer, vmContainer, viewContainer, dep1, dep2, dep3,
                                   dep4);
                     }).NonLazy();
    }

    protected abstract void Configure(TModelContainer modelContainer,
                                      TViewModelContainer viewModelContainer,
                                      TViewContainer viewContainer,
                                      TDependency1 dependency1,
                                      TDependency2 dependency2,
                                      TDependency3 dependency3,
                                      TDependency4 dependency4);
}

public abstract class DynamicViewConfiguration<TModelContainer, TViewModelContainer,
                             TViewContainer, TDependency1, TDependency2, TDependency3, TDependency4,
                             TDependency5> : ViewConfiguration
{
    public override void InstallBindings()
    {
        Container.Bind<TViewContainer>().FromComponentOnRoot();

        Container.Bind<TViewModelContainer>().ToSelf().AsCached()
                 .AfterCreation<TViewModelContainer, TModelContainer, TViewContainer, TDependency1,
                     TDependency2, TDependency3, TDependency4, TDependency5>(
                     (vmContainer, modelContainer, viewContainer, dep1, dep2, dep3, dep4, dep5) =>
                     {
                         Configure(modelContainer, vmContainer, viewContainer, dep1, dep2, dep3,
                                   dep4, dep5);
                     }).NonLazy();
    }

    protected abstract void Configure(TModelContainer modelContainer,
                                      TViewModelContainer viewModelContainer,
                                      TViewContainer viewContainer,
                                      TDependency1 dependency1,
                                      TDependency2 dependency2,
                                      TDependency3 dependency3,
                                      TDependency4 dependency4,
                                      TDependency5 dependency5);
}

