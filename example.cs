
namespace gamelogic
{
    struct Vector3
    {
        public float x, y, z;
    }

    struct Matrix4x4
    {
        public float m00, m01, m02, m03;
        public float m10, m11, m12, m13;
        public float m20, m21, m22, m23;
        public float m30, m31, m32, m33;
    }

    struct MeshID
    {
        public long ID;
    }
    struct RenderID
    {
        public long ID;
    }

    // The RenderSystem interface can be used to implement different rendering systems.
    public interface IRenderSystem : IEngineSystem
    {
        public RenderID CreateStaticMesh(MeshID meshPath);
    }

    // A game world is a 3D grid of cells. 
    // Cell size is 64m x 64m x 64m
    // Each cell can contain a list of game entities (e.g. missiles, missile launchers, trigger volumes)
    public class GameWorld
    {
        public class Cell
        {
            
        }

        // The game world where the game is active is part of the whole world.
        // There is thus a limited area where the game is active, this is the 3D grid of cells.
        public class ActiveGameWorld
        {
            // The active area is 1 km x 1 km x 1 km, and each array element is a reference to a cell.
            // We do this because not every cell is active, and we want to save memory.
            // When the player moves, we move the cell references around.
            // When a cell is unloaded we save the entity data.
            public List<Cell> cells = new List<Cell>();
            public int[,,] grid = new int[16, 16, 16];
        }

    }

    // The EventSystem is responsible for distributing events to game entities.
    public class EventSystem : IEngineSystem
    {
        public void SendEvent(EntityID from, EventType e)
        {
            
        }

        public void RegisterEventReceiver(EntityID entity, EventType e)
        {
            
        }

        public void Update()
        {
            
        }
    }


    // A trigger volume is a 3D volume that can detect when a game entity enters/touches/leaves it.
    public class TriggerVolume  : IEntity
    {
        public Vector3 position;
        public Vector3 size;

        public static void OnEnter(EntityID entity, EventSystem eventSystem)
        {
            eventSystem.SendEvent(entity, EventType.OnEnterTriggerVolume);
        }

        public static void OnLeave(EntityID entity, EventSystem eventSystem)
        {
            eventSystem.SendEvent(entity, EventType.OnLeaveTriggerVolume);
        }

    }

    // A missile has a position, direction, velocity and can move. It also
    // has a visual representation (static mesh) and a collision volume.
    public class MissileResource : IEntityResource
    {
        public Vector3 size; // Size of the missile
        public float mass; // Mass of the missile
        public float friction; // Friction of the missile
        public MeshID meshID; // The mesh to render the missile
        public VisualFX explosionID; // Visual Effect to Spawn upon impact
    }

    public class DynamicTransformComponent : IEntityComponent
    {
        public Matrix4x4 transform;
        public Vector3 direction;
        public float acceleration;
        public float velocity;
    }

    public class StaticTransformComponent : IEntityComponent
    {
        public Matrix4x4 transform;
    }

    public class Missile : IEntity
    {
        public DynamicTransformComponent dynamicTransform;
        public EntityID triggerVolumeID; // "Entity <-> Entity" Link

        // This should be the logic for each missile processed by MissileSystem::Update
        public void Move(float deltaTime)
        {
            // Move the missile, taking gravity and acceleration into account
            var gravity = new Vector3(0, -9.81f, 0);
            var force = gravity * mass;
            var acceleration = force / mass;
            velocity += acceleration * deltaTime;
            position += velocity * deltaTime;
        }
    }

    public class MissileSystem : IEntitySystem
    {
        private List<EntityID> missiles = new List<EntityID>();

        public void Update()
        {
            // Update all missiles
        }
    }

    // A Missile Launcher has a position, orientation and can fire missiles
    public class MissileLauncher : IEntity
    {

    }
}