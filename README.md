# Enemy FSM — Demo Project

This is a Unity 6 project I made to showcase my [Enemy FSM package](Packages/Potapus_FSM). The package is a reusable enemy AI system built on a generic Finite State Machine. The demo has a simple scene where you can walk around and fight a melee enemy.

---

## How to install the package

In Unity go to **Window → Package Manager → + → Add package from git URL** and paste:

```
https://github.com/Potapus/Unity-Enemy-FSM.git?path=Packages/Potapus_FSM#v1.0.0
```

Or if you just cloned this repo, go to **Add package from disk** and select `Packages/Potapus_FSM/package.json`.

---

## How to use it

**1. Add components to your enemy GameObject:**
- `NavMeshAgent` (Unity built-in)
- `EnemyStateMachine` (from the package)
- Something that implements `IEnemyHealth` and `IDamageable` — you can copy `EnemyHealth.cs` from this demo
- Something that implements `IKnockbackHandler` — optional, copy `EnemyKnockback.cs`
- Something that implements `IEnemyAttack` — the package includes `EnemyMeleeAttack`

**2. Create an enemy stats asset:**

Right-click in the Project window → **Create → EnemyFSM → Enemy Stats**

This is a ScriptableObject where you set aggro range, attack range, health, speed etc.

**3. Assign in the Inspector:**

On the `EnemyStateMachine` component assign the stats asset and your attack component. That's it — the enemy will start working.

---

## What states does the enemy have?

```
Idle ──► Wander           (no player nearby)
Idle ──► Chase            (player enters aggro range)
Chase ──► Attack          (player is close enough)
Chase ──► ReturnHome      (wandered too far from spawn)
Attack ──► Chase          (player moved out of range)
Any ──► Staggered         (got hit with knockback)
Any ──► Dead              (health reached zero)
```

---

## Demo controls

- **WASD** — move
- **Mouse** — camera

---

## What's in this repo

| Path | What it is |
|------|-----------|
| `Packages/Potapus_FSM/` | The actual package |
| `Assets/Scripts/` | Demo implementations of the package interfaces |
| `Assets/Scenes/SampleScene` | The playable demo scene |
| `Assets/ScriptableObjects/` | Pre-configured enemy stats |

---

## Requirements

- Unity 2022.3 or newer
- `com.unity.ai.navigation` for NavMesh

---

## License

MIT
