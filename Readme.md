**Links to games:**<br/>
[Fenced!]()<br/>
[BlockWay]()<br/>

**About:**<br/>
Type: Code Showcase<br/>
Span: 3.6.-9.8.2019.<br/>
Organization: LunarByte<br/>
(Actual repositories are private for obvious reasons.)<br/>
Files: Unedited and organized for viewing

**Notes on infrastructure and different configurations:**<br/>
LunarByte MVVM (Model-ViewModel-View) infrastructure and Zenject.<br/>
Game configurations(ScriptableObjectInstallers): Are run first. They mostly include Type bindings (pools, settings, etc.).<br/>
Model Configurations: Are run second. They can be thought as corresponding Model initialization scripts.<br/>
View Configurations: Are run last. They configure events and bind View methods to Model Property changes. <br/>
Dataflow: (Model -> ViewModel -> View).<br/>

**Content apart from game files:**<br/>
-Dynamic Content. Extension to LunarByte MVVM infrastructure to include Dynamic Content (multimple similar models bound to multiple similiar views), of which Unity-side implementation is available in the folders.<br/>
-LevelReader tool. Convert Exel CSV files to usable ScriptableObject assets inside the game.<br/>
-Zenject pooling extensions. Implementing Disposable and IPoolable patterns.<br/>
