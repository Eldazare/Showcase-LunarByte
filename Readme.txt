Links to playable games "Fenced!" and "BlockWay" can be found......

Code parts from projects coded under LunarByte, between 3.6.-7.8.2019.
This repository is not a compilable project, but a showcase of my work during this time.
(Actual repositories are private for obvious reasons).
All files are unedited parts of either game, foldered for convenience.

Projects utilized mostly Zenject and LunarByte MVVM (Model-ViewModel-View) infrastructure.
Notes on Configurations:
Game configurations are run first. They mostly include Type bindings (pools, settings, etc.).
Model Configurations are Model initialization scripts.
View Configurations configure events and Model Property changes to View Methods (Model -> ViewModel-> View).


In addition to the projects themselves, I worked on extension to the LunarByte MVVM model to include Dynamic Content (multimple similar models bound to multiple similiar views), of which Unity-side implementation is available in the folders.
Notable systems separated from game scripts:
-LevelReader tool (used in both games), to convert Exel CSV files to usable ScriptableObject assets inside the game
-Zenject pooling extensions, implementing Disposable pattern