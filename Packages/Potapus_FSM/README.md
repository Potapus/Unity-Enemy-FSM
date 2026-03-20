# Enemy FSM — `com.potapus.enemyfsm`

A Unity package I built for enemy AI. It has a generic FSM core you can use for anything, plus a ready-made enemy setup with NavMesh, hitbox combat, and 7 states out of the box.

**Unity 2022.3+ | v1.0.0 | MIT**

---

## Install

**Window → Package Manager → + → Add package from git URL**

```
https://github.com/Potapus/Unity-Enemy-FSM.git?path=Packages/Potapus_FSM#v1.0.0
```

Or add it directly in `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.potapus.enemyfsm": "https://github.com/Potapus/Unity-Enemy-FSM.git?path=Packages/Potapus_FSM#v1.0.0"
  }
}
```

---

## What's included

```
Runtime/
  Core/
    IState.cs              — the interface every state implements
    StateMachine.cs        — generic FSM, works with any enum + context
  Enemy/
    EnemyStateMachine.cs   — the main MonoBehaviour, drop it on your enemy
    EnemyContext.cs        — data passed into every state
    EnemyStateID.cs        — enum of all states
    SO_EnemyStats.cs       — ScriptableObject for tuning values
    States/                — 7 ready-made states
    Attack/
      IEnemyAttack.cs      — implement this to make your own attack
      EnemyMeleeAttack.cs  — built-in melee attack
    Interfaces/
      IEnemyHealth.cs
      IKnockbackHandler.cs
  Combat/
    IDamageable.cs
    HitData.cs
    Hitbox.cs
    HitboxSpawner.cs
```

---

## Using the generic FSM

The core `StateMachine` is not locked to enemies — you can use it for anything:

```csharp
var fsm = new StateMachine<MyStateID, MyContext>();

fsm.RegisterState(MyStateID.Idle, new IdleState());
fsm.RegisterState(MyStateID.Run,  new RunState());

fsm.OnStateChanged += id => Debug.Log($"Switched to {id}");

fsm.TransitionTo(MyStateID.Idle, ctx); // call once to start
fsm.Tick(ctx);                          // call in Update
```

To make a state just implement `IState<TContext>`:

```csharp
public class MyState : IState<MyContext>
{
    public void Enter(MyContext ctx) { }
    public void Tick(MyContext ctx)  { }
    public void Exit(MyContext ctx)  { }
}
```

---

## EnemyStateMachine

Drop this on an enemy that has a `NavMeshAgent`. Then assign in the Inspector:

| Field | Description |
|-------|-------------|
| Stats | Your `SO_EnemyStats` asset — required |
| Home Point | Where the enemy returns to if it wanders too far — defaults to spawn position |
| Attack Behaviour | Any MonoBehaviour that implements `IEnemyAttack` |

```csharp
// You can also control it from code
enemy.TransitionTo(EnemyStateID.Chase);
enemy.SetTarget(playerTransform);
enemy.OnStateChanged += id => { /* hook up animations etc */ };
```

---

## EnemyContext

This is the data bag every state gets on Enter/Tick/Exit:

| Property | Type | |
|----------|------|-|
| `Agent` | `NavMeshAgent` | movement |
| `Self` | `Transform` | enemy's own transform |
| `Health` | `IEnemyHealth` | |
| `Knockback` | `IKnockbackHandler` | |
| `Stats` | `SO_EnemyStats` | all tuning values |
| `Attack` | `IEnemyAttack` | |
| `Target` | `Transform` | mutable |
| `LastHit` | `HitData` | mutable |
| `DistanceToTarget` | `float` | helper |
| `DistanceFromHome` | `float` | helper |
| `IsAgentReady` | `bool` | checks agent is valid and on NavMesh |

---

## SO_EnemyStats fields

| Field | What it does |
|-------|-------------|
| Max Health | |
| Death Destroy Delay | Seconds before the GameObject gets destroyed |
| Aggro Range | Player within this → Chase |
| Attack Range | Player within this → Attack |
| Lose Aggro Range | Player beyond this → start the lose aggro timer |
| Lose Aggro After | Seconds before giving up and going home |
| Home Radius | How far from home before ReturnHome kicks in |
| Wander Radius | How far from home the enemy can wander |
| Min / Max Wander Delay | Random wait time in Idle before wandering |

---

## Interfaces

The package uses interfaces for health, knockback and attack so it doesn't depend on your specific MonoBehaviours. You implement them however you want.

```csharp
public interface IEnemyHealth
{
    bool  IsDead        { get; }
    float CurrentHealth { get; }
    float MaxHealth     { get; }
    event Action<HitData> OnHitReceived;
    event Action<float>   OnDamaged;
    event Action          OnDied;
}

public interface IKnockbackHandler
{
    void ApplyKnockback(Vector3 force);
    bool IsActive { get; }
}

public interface IEnemyAttack
{
    bool IsReady { get; }
    void Execute(EnemyContext ctx);
}

public interface IDamageable
{
    void ApplyHit(HitData hit);
}
```

---

## Combat

`HitData` is a struct that gets passed around when something is hit:

```csharp
public struct HitData
{
    public float      Damage;
    public Vector3    Force;       // knockback
    public Vector3    HitPoint;
    public GameObject Attacker;    // so the hitbox doesn't hit whoever spawned it
}
```

`Hitbox` is a trigger collider component. When it overlaps an `IDamageable` it calls `ApplyHit`. Each target only gets hit once per activation, and it skips the attacker and their teammates.

`HitboxSpawner` just spawns a Hitbox at a position and destroys it after a set time:

```csharp
_hitboxSpawner.SpawnHitbox(origin, radius, damage, force, duration, attacker);
```

---

## License

MIT
