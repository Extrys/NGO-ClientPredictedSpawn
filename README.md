# NGO ClientPredictedSpawn
Use client prediction spawning for Netorking for GameObjects

This repository implements a client-predicted object spawning system for Unity projects using Netcode for GameObjects. 
It includes two implementations of a spawner system: basic and advanced

**Basic**: Contains a simple script for handling client-predicted object spawning.  
**Advanced**: is a more advanced implementation that offers greater control by integrating a customizable spawner factory.

you can use both methods independently, there is no dependences between folders

### Installation
- You need Unity 6 LTS. (you can use in other NGO compatible versions, but need reflections or modify the package, explained in comments)  
- Add the files (from, Basic, Advanced or both folders) to your Unity project and ensure Netcode for GameObjects is installed. 


### Basic Implementation
The basic system (MonoPredictedSpawnerBasic) is ideal for scenarios where you need a quick and straightforward way to spawn objects with client prediction. 
This system uses a simple queue to instantiate and synchronize objects across the network.
(The prefabs to be spawned needs to have "NetworkObject" component)

- Easy to set up: You only need to define the prefab you want to spawn.
- Requires no additional structure or advanced instance management.
- Less flexible, as you need an instance of this for each type of prefab you need to instantiate.


### Advanced Implementation
The advanced method provides greater flexibility. 
It uses a spawner factory to manage spawner instances, each spawner handles the spawning of runtime specified prefabs to be network compatible.
(The prefabs to be spawned needs to have "NetworkObject" component)

- Allows the use of multiple spawners managed by a factory, allowing spawn different prefabs in runtime.
- The spawner factory can be easily integrated with dependency injection systems for dynamically managing object creation.
