# Game Architecture - Design Decisions

## Core Principle

**Controllers bridge Unity ↔ Game Logic**

```
MonoBehaviour Controllers = Unity features (Inspector, Coroutines)
Plain Classes = Game logic (Testable, No Unity dependency)
```

---

## System Types

| Type | MonoBehaviour? | Responsibility | Example |
|------|----------------|----------------|---------|
| **XController** | ✅ Yes | Unity bridge, holds SerializeFields, creates X | `WorldGeneratorController` |
| **X** | ❌ No | Game logic, no Unity dependency | `WorldGenerator` |
| **Database** | ❌ No | Stores templates (ScriptableObjects) | `ItemDatabase` |
| **Manager** | ❌ No | Owns collection, Add/Remove/Query | `ColonistManager` |
| **System** | ❌ No | Processes logic (stateless) | `PhysicsSystem` |
| **Spawner** | ❌ No | Spawns runtime entities | `ColonistSpawner` |
| **Renderer** | ❌ No | Creates/updates visuals (read-only) | `ColonistRenderer` |

---

## Naming Convention

```csharp
// MonoBehaviour Controller
public class WorldGeneratorController : MonoBehaviour
{
    [SerializeField] private WorldGenerationConfig[] configs; // Unity Inspector
    
    public WorldGenerator CreateGenerator()
    {
        return new WorldGenerator(configs[0]); // Creates logic class
    }
}

// Plain Class Logic
public class WorldGenerator
{
    private WorldGenerationConfig config;
    
    public World Generate() { /* ... */ }
}
```

**Rule:** If it needs Unity features → `XController`. Otherwise → plain class.

---

## Initialization Flow

### 1. Bootstrapper (Static)
```csharp
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
private static void InitializeBootstrap()
{
    SceneManager.LoadScene("PersistentScene", LoadSceneMode.Additive);
}
```
**Job:** Ensure PersistentScene loads first

### 2. GameCoordinator.Awake()
```csharp
private void Awake()
{
    if (instance == null)
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
```
**Job:** Setup singleton

### 3. SystemInitializer.Awake()
```csharp
private void Awake()
{
    // Get controllers from scene
    var worldGenController = GetComponent<WorldGeneratorController>();
    
    // Use controllers to create logic
    var world = worldGenController.GenerateWorld();
    
    // Create managers, systems, etc.
    var worldMgr = new WorldManager(world);
    var colonistMgr = new ColonistManager();
    
    // Register with GameCoordinator
    GameCoordinator.Instance.RegisterManager(worldMgr);
}
```
**Job:** ONE place where all game systems are created

### 4. GameCoordinator.Start()
```csharp
private void Start()
{
    // Initialize registered systems
    foreach (var db in databases)
        db.Initialize();
    
    foreach (var mgr in managers)
        mgr.Initialize();
    
    // Auto-start
    if (autoStartGame)
        NewGame();
}
```
**Job:** Initialize and start game

---

## Scene Structure

```
PersistentScene
├─ GameCoordinator (orchestrator)
├─ SystemInitializer (creation point)
│   └─ Refs: Controllers (drag from scene)
│
└─ Controllers/
    ├─ WorldGeneratorController
    ├─ AudioController
    └─ InputController
```

---

## Dependency Management

**All dependencies created in SystemInitializer:**

```csharp
// Order matters
// 1. Controllers (create logic classes)
var world = worldGenController.GenerateWorld();

// 2. Databases
var itemDB = new ItemDatabase();

// 3. Managers (depend on databases)
var colonistMgr = new ColonistManager();

// 4. Systems (depend on world)
var physics = new PhysicsSystem(world.Grid);

// 5. Spawners (depend on databases + managers)
var spawner = new ColonistSpawner(itemDB, colonistMgr);

// 6. Register
GameCoordinator.Instance.RegisterManager(colonistMgr);
```

**Dependency injection:** Pass dependencies via constructor

---

## Interface Structure

```csharp
public interface IDatabase
{
    void Initialize();
}

public interface IManager
{
    void Initialize();
    void Update(float deltaTime);
    void Clear();
    void Save(string saveName);
    void Load(string saveName);
}

public interface ISpawner
{
    void Spawn();
}
```

**Systems don't have Update()** - called on-demand by Controllers/Behaviors

---

## Managers vs Systems

```csharp
// MANAGER - Owns collection
public class ColonistManager : IManager
{
    private Dictionary<string, IColonist> colonists; // OWNS
    
    public void Update(float deltaTime)
    {
        foreach (var colonist in colonists.Values) // LOOPS
            colonist.Needs.Update(deltaTime);
    }
}

// SYSTEM - Processes on-demand
public class PhysicsSystem
{
    public void UpdatePhysics(IColonist colonist, float deltaTime)
    {
        // Process ONE colonist, don't store it
    }
}

// Called by Controller
public class ColonistBehavior
{
    public void Update(float deltaTime)
    {
        physics.UpdatePhysics(colonist, deltaTime); // Pass colonist each time
    }
}
```

---

## Configuration

**ScriptableObjects for all config:**

```csharp
[CreateAssetMenu(fileName = "WorldConfig", menuName = "Game/World Config")]
public class WorldGenerationConfig : ScriptableObject
{
    public int width = 100;
    public int depth = 200;
    public int seed = 12345;
}

// Controller holds reference
public class WorldGeneratorController : MonoBehaviour
{
    [SerializeField] private WorldGenerationConfig[] configs;
}
```

---

## Generation Pipeline

```csharp
// Controller
public class WorldGeneratorController : MonoBehaviour
{
    [SerializeField] private bool generateTerrain = true;
    [SerializeField] private bool generatePOIs = true;
    
    public World GenerateWorld()
    {
        var generator = new WorldGenerator(config);
        
        if (generateTerrain)
            generator.AddStep(new TerrainGenerationStep());
        
        if (generatePOIs)
            generator.AddStep(new POIGenerationStep());
        
        return generator.Generate();
    }
}

// Logic
public class WorldGenerator
{
    private List<IWorldGenerationStep> steps;
    
    public void AddStep(IWorldGenerationStep step) => steps.Add(step);
    
    public World Generate()
    {
        var context = new WorldGenerationContext(config);
        
        foreach (var step in steps)
            step.GenerateSync(context);
        
        return context.BuildWorld();
    }
}
```

---

## Stateless Systems

**Created once, stored in ServiceLocator:**

```csharp
// In SystemInitializer
var physicsSystem = new PhysicsSystem(world.Grid);
var pathfinder = new Pathfinder(world.Grid);

ServiceLocator.Register(physicsSystem);
ServiceLocator.Register(pathfinder);

// Usage anywhere
var physics = ServiceLocator.Get<PhysicsSystem>();
physics.UpdatePhysics(colonist, deltaTime);
```

---

## Renderers

```csharp
public class ColonistRenderer
{
    private ColonistManager manager;
    
    public ColonistRenderer(ColonistManager manager)
    {
        this.manager = manager;
        
        // Subscribe to events
        manager.OnColonistAdded += CreateVisual;
        manager.OnColonistDied += DestroyVisual;
    }
    
    public void Update()
    {
        foreach (var colonist in manager.GetAll())
        {
            UpdateVisual(colonist); // READ ONLY
        }
    }
}
```

**Rule:** Renderers NEVER modify game data, only read and visualize

---

## Key Rules

1. **Only Controllers are MonoBehaviours** - everything else is plain C#
2. **SystemInitializer creates everything** - one creation point
3. **Managers own collections** - Systems process on-demand
4. **Dependencies via constructor** - no hidden dependencies
5. **ScriptableObjects for config** - designer-friendly
6. **Renderers read-only** - never modify game data
7. **Events for loose coupling** - managers notify, renderers listen

---

## When to Create New Types

### Need Unity features? → Controller
```csharp
// YES: Needs Inspector, coroutines
public class XController : MonoBehaviour
```

### Pure logic? → Plain class
```csharp
// YES: No Unity dependency
public class X
```

### Manages collection? → Manager
```csharp
// YES: Owns Dictionary<id, entity>
public class XManager : IManager
```

### Stateless processing? → System
```csharp
// YES: No state, just processes
public class XSystem
```

---

## Common Mistakes to Avoid

❌ Making Managers MonoBehaviours (no need for Unity lifecycle)
❌ Systems with Update() loops (should be called on-demand)
❌ Multiple creation points (only SystemInitializer creates)
❌ Renderers modifying game data (read-only!)
❌ Hidden dependencies (always inject via constructor)

---

## Quick Reference

**Need to add new feature?**

1. Does it need Unity features (Inspector/Coroutines)?
    - YES → Create `XController` (MonoBehaviour)
    - NO → Create `X` (plain class)

2. Does it own a collection?
    - YES → It's a **Manager** (implements IManager)
    - NO → Continue

3. Is it stateless processing?
    - YES → It's a **System** (no interface needed)
    - NO → Continue

4. Does it spawn entities?
    - YES → It's a **Spawner** (implements ISpawner)
    - NO → It's probably just a **utility class**

5. Add to SystemInitializer.Awake() for creation and wiring

---

## File Organization

```
Scripts/
├─ Core/
│   ├─ Bootstrapper.cs
│   ├─ GameCoordinator.cs
│   ├─ SystemInitializer.cs
│   └─ ServiceLocator.cs
│
├─ Controllers/ (MonoBehaviours)
│   ├─ WorldGeneratorController.cs
│   ├─ AudioController.cs
│   └─ InputController.cs
│
├─ Managers/ (Plain classes)
│   ├─ ColonistManager.cs
│   ├─ TaskManager.cs
│   └─ WorldManager.cs
│
├─ Systems/ (Plain classes)
│   ├─ PhysicsSystem.cs
│   └─ PathfindingSystem.cs
│
├─ Generation/ (Plain classes)
│   ├─ WorldGenerator.cs
│   ├─ TerrainGenerationStep.cs
│   └─ POIGenerationStep.cs
│
└─ Interfaces/
    ├─ IManager.cs
    ├─ ISpawner.cs
    └─ GameInterfaces.cs
```

---

**That's it. Simple, clear, predictable.** 🎯