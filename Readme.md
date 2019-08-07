**Links to two games:**<br/>
[Fenced!]()<br/>
[BlockWay]()<br/>

**About:**<br/>
Type: Code Showcase<br/>
Timeperiod: 3.6.-9.8.2019.<br/>
Organization: LunarByte<br/>
(Actual repositories are private for obvious reasons.)<br/>
Files: Unedited and organized for viewing

**Additional notes on infrastructure used:**<br/>
LunarByte MVVM (Model-ViewModel-View) infrastructure and Zenject.<br/>
Game configurations(ScriptableObjectInstallers): Are run first. They mostly include Type bindings (pools, settings, etc.).<br/>
Model Configurations: Are run second. They can be thought as corresponding Model initialization scripts.<br/>
View Configurations: Are run last. They configure events and bind Model Property changes to View Methods (Model -> ViewModel-> View).<br/>

**Content apart from game files:**<br/>
-Dynamic Content, extension to LunarByte MVVM model to include Dynamic Content (multimple similar models bound to multiple similiar views), of which Unity-side implementation is available in the folders.<br/>
-LevelReader tool (used in both games), to convert Exel CSV files to usable ScriptableObject assets inside the game<br/>
-Zenject pooling extensions, implementing Disposable pattern<br/>
