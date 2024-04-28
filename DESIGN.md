# Game Logic

## Languages

* Go
* C#
* C / C++

## Objective

Define the data structures and logic for the game in such a way that the data build pipeline can connect data.

Current Engines (Unity, Unreal) are not designed to do this very efficiently. In Unreal Engine you can create a gameplay actor or ability system, but it is not designed to be really data driven, it doesn't even provide means to easily create asset bundles. So you can have an actor link to many assets by file but it stops there, we would like to introduce a way to link data more intelligently. This means that automatically there should be some 'collection' of which assets are used by a particular entity, level or even world grid cell. If you place enemy spawners in a level, the spawner should have a palette of enemy types that can be spawned, this means that the compiled world grid cell should list all necessary resources.
Each enemy is also defined by the resources it used like, skeletal mesh, control rig, animation graph, animations, sound effects etc. This means that the enemy type should also have a palette of resources that it uses. This way we can create a tree of dependencies that can be used to build asset bundles. The high level goal is to provide tools/code that can be used to walk this tree and inspect the asset bundles.

## Automaticity

* Structure of Array's
* Entity and Components
* Many booleans into bits
* De-duplication of data

