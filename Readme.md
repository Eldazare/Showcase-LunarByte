**Links to two games:**<br/>
[Fenced!]()<br/>
[BlockWay]()<br/>

**About Contents:**<br/>
Code parts from projects coded under LunarByte, between 3.6.-7.8.2019.<br/>
This repository is not a compilable project, but a showcase of my work during this time.<br/>
(Actual repositories are private for obvious reasons).<br/>
All files are unedited parts of either game, foldered for convenience.<br/>

**Additional notes on infrastructure used:**<br/>
Projects utilized mostly Zenject and LunarByte MVVM (Model-ViewModel-View) infrastructure.<br/>
Notes on Configurations:<br/>
Game configurations are run first. They mostly include Type bindings (pools, settings, etc.).<br/>
Model Configurations are Model initialization scripts.<br/>
View Configurations configure events and Model Property changes to View Methods (Model -> ViewModel-> View).<br/>

**Content apart from game files:**<br/>
-Dynamic Content, extension to LunarByte MVVM model to include Dynamic Content (multimple similar models bound to multiple similiar views), of which Unity-side implementation is available in the folders.<br/>
-LevelReader tool (used in both games), to convert Exel CSV files to usable ScriptableObject assets inside the game<br/>
-Zenject pooling extensions, implementing Disposable pattern<br/>
