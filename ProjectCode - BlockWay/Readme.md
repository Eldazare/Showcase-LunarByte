**SuperHayContainerView.cs** (FeverBar) <br/>
Fever fill size is bound to PlayerModel property that is incremented when yellow blocks are collected.

**LevelView.cs** (LevelGeneration)<br/>
From row 214 forwards, there's code for level generation.<br/>
Main "Menu" moves forward before actual level is generated and we wanted to be able to modify and scale the total width, block width and amount of cubes in each block. <br/>
This amount of complexity is represented in the length of the generation code (compared to level generation code in Fenced for example).<br/>

**SawView.cs**<br/>
Notable functionality include collisions and Fever mode handling.<br/>
As with other views, methods are bound to Property changes at Model level.<br/>
Perhaps more interesting is...<br/>

**SawViewConfiguration.cs**<br/>
Length of this file speaks volumes to how much the SawView does.<br/>
Contains interesting notes on what the dispatchable events actually do <br/>
(Lines with viewModel....Event.AddListener())<br/>
Also good demonstration how Service calls are handled in the system.<br/>


**TimerService.cs**<br/>
A decent way to collect all uses of "Do this method after N-time".<br/>
Also implements recurring calls for certain time, basically requiring Coroutines.<br/>

**Additionally...**<br/>
I replicated level selection code from Fenced with minor changes to fit the project.<br/>
