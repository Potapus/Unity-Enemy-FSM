# Changelog — Enemy FSM

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-03-20

### Added
- Generic `StateMachine<TStateID, TContext>` core — reusable for any game object
- `IState<TContext>` interface with `Enter`, `Tick`, and `Exit` lifecycle methods
- `EnemyStateMachine` MonoBehaviour — drop-in component for enemy GameObjects
- `EnemyContext` data bag passed through every state transition
- `SO_EnemyStats` ScriptableObject for data-driven enemy configuration
- Seven built-in enemy states: `Idle`, `Wander`, `Chase`, `Attack`, `Staggered`, `ReturnHome`, `Dead`
- `IEnemyAttack` interface with `EnemyMeleeAttack` built-in implementation
- `IEnemyHealth` and `IKnockbackHandler` interfaces for decoupled component design
- Hitbox-based combat system: `Hitbox`, `HitboxSpawner`, `HitData`, `IDamageable`
- NavMesh integration via `UnityEngine.AI.NavMeshAgent`
- `OnStateChanged` event for external observers (UI, animation, audio)
