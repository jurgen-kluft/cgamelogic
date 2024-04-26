
namespace gamelogic
{
    struct double3
    {
        public double x, y, z;
    }
    struct float3
    {
        public float x, y, z;

        public float Normalize()
        {
            var length = Math.Sqrt(x * x + y * y + z * z);
            x /= length;
            y /= length;
            z /= length;
            return length;
        }
    }
    struct float4 { public float x, y, z, w; }
    struct int3
    {
        public int x, y, z;
    }

    struct float4x4
    {
        public float4 m0;
        public float4 m1;
        public float4 m2;
        public float4 m3;
    }

    struct MeshResourceId { public long Id; }
    struct LightResourceId { public long Id; }
    struct VisualFxResourceId { public long Id; }
    struct RenderInstanceId { public long Id; }

    struct SoundResourceId { public long Id; }
    struct SoundInstanceId { public long Id; }

    struct SoundStimulusResourceId { public long Id; }
    struct SmellStimulusResourceId { public long Id; }
    struct VisualStimulusResourceId { public long Id; }
    struct StimuliInstanceId { public long Id; }

    public interface IRenderSystem : IEntitySystem
    {
        public RenderInstanceId CreateLight(LightResourceId resourceId);
        public RenderInstanceId CreateMesh(MeshResourceId resourceId);
        public RenderInstanceId CreateVisualFx(VisualFxResourceId resourceId);

        public void UpdateCreate(float deltaTime); // Finalize the requests to create meshes and visual effects
        public void UpdateDynamics(float deltaTime); // Update movement, positions and rotations
        public void UpdateLogic(float deltaTime); // Update any rendering logic
    }

    public interface ISoundSystem : IEntitySystem
    {
        public SoundInstanceId CreateSound(SoundResourceId resourceId, float3 position, float volume, float range);

        public void UpdateCreate(float deltaTime); // Finalize the requests to create meshes and visual effects
        public void UpdateDynamics(float deltaTime); // Update movement, positions and rotations
        public void UpdateLogic(float deltaTime); // Update any rendering logic
    }

    public interface IPhysicsSystem : IEntitySystem
    {
        public RigidBodyId CreateStaticAABB(float3 position, float3 size);
        public RigidBodyId CreateDynamicAABB(float3 position, float3 size, float mass, float friction);
        public RigidBodyId CreateDynamicCapsule(float3 position, float height, float width, float mass, float friction);

        public void UpdateCreate(float deltaTime); // Finalize the requests to create rigid bodies
        public void UpdateDynamics(float deltaTime);
        public void UpdateLogic(float deltaTime);
    }

    public interface IPerceptionSystem : IEntitySystem
    {
        public StimuliInstanceId CreateSoundStimuli(SoundStimulusResourceId stimulus, float3 position, float volume, float range, float duration);
        public StimuliInstanceId CreateSmellStimuli(SmellStimulusResourceId stimulus, float3 position, float strength, float range, float duration);
        public StimuliInstanceId CreateVisualStimuli(VisualStimulusResourceId stimulus, float3 position, float3 direction, float speed, float size);

        // Stimuli are send to the receiver through the EventSystem
        public void RegisterReceiver(EntityId entity, PerceptionType types);

        public void UpdateCreate(float deltaTime); // Finalize the requests to create sounds, smells and visuals
        public void UpdateDynamics(float deltaTime); // Update the stimuli, fading etc..
        public void UpdateLogic(float deltaTime); // Update the perception logic
    }

    // A game world is a 3D grid of cells. 
    // Cell size is 64m x 64m x 64m
    // Each cell can contain a list of game entities (e.g. missiles, missile launchers, trigger volumes)
    public class GameWorld
    {
        // The game world where the game is active is part of the whole world.
        // There is thus a limited area where the game is active, this is the 3D grid of cells.

        // Entities are sorted by their CellId, which is a 3D value into the grid.
        public List<EntityId> staticEntitiesSortX = new();
        public List<EntityId> staticEntitiesSortY = new();
        public List<EntityId> staticEntitiesSortZ = new();

        public List<EntityId> moveableEntitiesSortX = new();
        public List<EntityId> moveableEntitiesSortY = new();
        public List<EntityId> moveableEntitiesSortZ = new();

        // Sweep and Prune is used to detect overlaps between entities.


        // The cell size is 64 meters
        public const int cellSize = 64;

        // World size is 64 km x 64 km x 4 km
        public const int worldCellsOnAxisX = 1024; // Horizontal plane
        public const int worldCellsOnAxisY = 1024; // Horizontal plane
        public const int worldCellsOnAxisZ = 64; // Up, max height is 4 km

        // The active area is 1 km x 1 km x 1 km, and each array element is a reference to a cell.
        // We do this because not every cell is active, and we want to save memory.
        // When the player moves, we move the cell center.
        // When a cell is unloaded we save the entity data.
        public const int gridCellsOnAxisX = 17;
        public const int gridCellsOnAxisY = 17;
        public const int gridCellsOnAxisZ = 17;

        public float3 cellBounds = new(cellSize, cellSize, cellSize);
        public float3 gridBounds = new(cellSize * gridCellsOnAxisX, cellSize * gridCellsOnAxisY, cellSize * gridCellsOnAxisZ);
        public int3 gridCenter = new(gridCellsOnAxisX / 2, gridCellsOnAxisY / 2, gridCellsOnAxisZ / 2);
        public double3 worldCenter = new(0, 0, 0);
    }

    public class BulletSystem : IEntitySystem
    {
        // A bullet is a fast moving projectile that can hit a target, we need to check for overlaps
        // with other entities and the world geometry.
        // So this requires 'continues collision detection', in one frame a bullet becomes a OOBB which
        // we query against the collision world.
        // The goal is to be able to process many bullets at the same time, so we need to be able to
        // process many bullets in parallel.

        public BulletId SpawnBullet(float3 position, float3 direction, float speed, float friction, float mass, float damage)
        {
            // Create a bullet with initial position, direction, speed, friction, mass and damage
        }

        public void UpdateDynamics()
        {

        }

        public void UpdateLogic()
        {

        }
    }


    // The EventSystem is responsible for distributing events to game entities.
    public class EventSystem : IEngineSystem
    {
        public PropertyId RegisterEntityIdProperty(string name) { }
        public PropertyId RegisterFloatProperty(string name) { }
        public PropertyId RegisterIntProperty(string name) { }

        // Standard properties
        public static PropertyId sFrom = RegisterEntityIdProperty("From");

        public EventId BeginEventWriting()
        {
        }

        public void WriteEventEntityId(PropertyId propertyId, EntityId entityId) { }
        public void WriteEventPropertyFloat(PropertyId propertyId, float value) { }
        public void WriteEventPropertyInt(PropertyId propertyId, int value) { }

        public void EndEventWriting()
        {
        }

        public void BeginEventReading(EventId eventId)
        {
        }

        public EntityId ReadEventPropertyAsEntityId()
        {
            // Make sure this property is an EntityId
        }

        public float ReadEventPropertyAsFloat()
        {
            // Make sure this property is a float
        }

        public int ReadEventPropertyAsInt()
        {
            // Make sure this property is an int
        }

        public void EndEventReading()
        {
        }

        public void SendEvent(EntityId to, EventId eventId)
        {
        }

        public void RegisterEventReceiver(EntityId receiver, EventType e)
        {

        }

        public void Update()
        {

        }
    }

    public class AABBComponent : IEntityComponent
    {
        public float3 position;
        public float3 size;
    }

    public class TriggerVolumeResource : IEntityResource
    {
        public float3 size;
        public EventType onEnterEventType;
        public EventType onLeaveEventType;
        public PropertyId triggerOnEnterPropertyId;
        public PropertyId triggerOnLeavePropertyId;
    }

    public class TriggerPropertiesComponent : IEntityComponent
    {
        public TriggerVolumeResource resource;
    }

    // A trigger volume is a 3D volume that can detect when an entity enters/touches/leaves it.
    public class TriggerVolume : IEntity
    {
        // Entity components
        public AABBComponent aabb;
        public TriggerPropertiesComponent properties;

        // Notes:
        // You want to execute some logic when an entity enters/leaves the trigger volume.

        // Example:
        // A door opens when a player enters a trigger volume and it closes when the player leaves.
        // How do we associate the trigger volume with the door and with the logic to open/close it?

        // Reasoning:
        // We could also have this trigger volume send an event to the event system when an entity enters/leaves it, but
        // we also include the target entity (the door).

        // Then we just need the DoorSystem to register itself to the EventSystem and listen for the 'door open/close' event.
        // The DoorSystem::Update can then process the events and open/close a door by playing a door open/close animation.
        // The DoorSystem can have many different doors, and each door can have a different animation, sound, etc.

        public void OnEnter(EntityId entity, EventSystem eventSystem)
        {
            var eventId = eventSystem.BeginEventWriting();
            eventSystem.WriteEventEntityId(EventSystem.sFrom, entity);
            eventSystem.WriteEventPropertyInt(triggerOnEnterPropertyId, 1);
            eventSystem.EndEventWriting();
            eventSystem.SendEvent(entity, eventId);
        }

        public void OnLeave(EntityId entity, EventSystem eventSystem)
        {
            var eventId = eventSystem.BeginEventWriting();
            eventSystem.WriteEventEntityId(EventSystem.sFrom, entity);
            eventSystem.WriteEventPropertyInt(triggerOnLeavePropertyId, 1);
            eventSystem.EndEventWriting();
            eventSystem.SendEvent(entity, eventId);
        }
    }

    // A missile has a position, direction, velocity and can move. It also
    // has a visual representation (static mesh) and a collision volume.

    public class ParticleEffectResource : IEntityResource
    {
        public float duration; // Duration of the particle effect
        public VisualFxId fxID; // The visual effect to spawn
    }

    public class SoundEffectResource : IEntityResource
    {
        public SoundInstanceId soundId; // The sound to play
        public float volume; // Volume of the sound
        public float range; // Range of the sound
        public float duration; // Duration of the sound
    }

    public class ExplosiveResource : IEntityResource
    {
        public float3 size; // Size of the explosive
        public MeshResourceId meshResourceId; // The mesh to render the explosive (landmine, C4, etc.
        public SoundResourceId soundResourceId; // Sound Effect to Play upon impact
        public VisualFxResourceId visualFxResourceId; // Visual Effect to Spawn upon impact
    }

    public class MotionComponent : IEntityComponent
    {
        public float3 direction;
        public float acceleration;
        public float velocity;
    }

    public class TransformComponent : IEntityComponent
    {
        public float4x4 transform;

        public float3 position
        {
            get
            {
                return new float3(transform.m03, transform.m13, transform.m23);
            }
            set
            {
                transform.m03 = value.x;
                transform.m13 = value.y;
                transform.m23 = value.z;
            }
        }
    }

    public class MissilePropertiesComponent : IEntityComponent
    {
        public MissileResource resource;
    }

    // The MissileResource is a resource that contains the missile's properties and can be used to spawn missiles.
    public class MissileResource : IEntityResource
    {
        public float mass; // Mass of the missile
        public float friction; // Friction of the missile
        public ExplosiveResource explosiveResource; // The explosive to spawn upon impact
    }

    public class Missile : IEntity
    {
        public MotionComponent motion;
        public TransformComponent transform;
        public MissilePropertiesComponent properties;

        // This should be the logic for each missile processed by MissileSystem::Update
        public void Move(float deltaTime)
        {
            var gravity = new float3(0, -9.81f, 0); // This is a direction vector + acceleration

            // The missile moves in a direction with a velocity
            var position = transform.position + (motion.velocity * motion.direction) * deltaTime;

            motion.velocity = motion.velocity + motion.acceleration * deltaTime;

            // Calculate the direction since it is influenced by gravity
            motion.direction = (motion.velocity * motion.direction);
            motion.direction.y += gravity.y * deltaTime;
            motion.velocity = motion.direction.Normalize();

            // Update transform
            transform.position = position;
        }
    }


    public interface ECS : IEngineSystem
    {
        public EntityComponentId RegisterComponent(IEntityComponent component);
        public EntityFlagId RegisterFlag(string name);


        public EntityId CreateEntity();
        public void DestroyEntity(EntityId entity);

        public void AddComponent(EntityId entity, EntityComponentId component);
        public void RemoveComponent(EntityId entity, EntityComponentId component);
        public void AddFlag(EntityId entity, E, EntityFlagId flag);
        public void RemoveFlag(EntityId entity, EntityFlagId flag);

    }

    public interface IEntitySystem : IEngineSystem
    {
        public void UpdateCreate(float deltaTime); // Finalize the requests to create sounds, smells and visuals
        public void UpdateDynamics(float deltaTime);
        public void UpdateLogic(float deltaTime);
    }


    public class MissileSystem : IEntitySystem
    {
        private List<EntityId> missiles = new List<EntityId>();

        public void SpawnMissile(MissileResource resource, float3 position, float3 direction, float speed)
        {
            var missile = new Missile();
            missile.motion.direction = direction;
            missile.motion.velocity = speed;
            missile.motion.acceleration = friction / mass;
            missile.transform.position = position;
            missile.properties.resource = resource;

            missiles.Add(missile);
        }

        public void UpdateDynamics(float deltaTime)
        {
            foreach (var missile in missiles)
            {
                missile.Move(deltaTime);
            }

            // Missiles have moved, but any attached entities (Visual FX, Sound) are not updated.
            // We need to update the RenderSystem and SoundSystem to update the visual and sound effects.
        }
    }

    // A Missile Launcher has a position, orientation and can fire missiles
    public class MissileLauncher : IEntity
    {

    }
}