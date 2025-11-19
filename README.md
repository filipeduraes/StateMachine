# HFSM

A hierarchical finite state machine system for **Unity (C#)** designed for structured behaviors, deterministic transitions and strongly typed state logic.  
It provides hierarchical state stacks, automatic state path resolution and controlled execution of `Enter`, `FixedUpdate`, and `Exit` across the entire active chain.

Repository: https://github.com/filipeduraes/StateMachine

---

## Features

- Hierarchical finite state machine with parent–child relationships.
- Automatic calculation of the active state path.
- Deterministic transitions between hierarchical states.
- Strongly typed states discovered through reflection.
- Automatic execution of `EnterState`, `FixedUpdate`, and `ExitState`.
- Coroutine helpers directly available inside states.

---

## Installation

### Using UPM (Unity Package Manager)

1. Open **Window > Package Manager** in Unity.
2. Click **+** → **Add package from git URL...**
3. Enter:

```
https://github.com/filipeduraes/StateMachine.git
```

Unity will fetch and install the package automatically.

### Using `manifest.json`

You can also add the dependency directly:

```json
{
  "dependencies": {
    "com.ideatogame.state-machine": "https://github.com/filipeduraes/StateMachine.git"
  }
}
```

---

## Usage

### Create a State Machine

```csharp
public class EnemyStateMachine : StateMachine<EnemyStateMachine>
{
    protected override void SetInitialState()
    {
        SetState<EnemyIdleState>();
    }
}
```

### Create States

```csharp
public class EnemyBaseState : State<EnemyStateMachine>
{
    public override Type ParentState => null;

    public EnemyBaseState(EnemyStateMachine machine) : base(machine) { }
}

public class EnemyIdleState : State<EnemyStateMachine>
{
    public override Type ParentState => typeof(EnemyBaseState);

    public EnemyIdleState(EnemyStateMachine machine) : base(machine) { }

    public override void EnterState()
    {
    }

    public override void FixedUpdate()
    {
    }
}
```

### Change States

```csharp
SetState<EnemyChaseState>();
```

The system will:
- Exit states not present in the new hierarchy.
- Enter all states required for the new state's path.

---

## Execution Flow

During `FixedUpdate`:
1. The machine iterates the current state path from root to leaf.
2. Calls `FixedUpdate` on each state in order.

This ensures layered, predictable behavior.

---

## State API

Each state exposes:

- `EnterState()`
- `FixedUpdate()`
- `ExitState()`
- `SetState<TState>()`
- `StartCoroutine(...)`
- `StopCoroutine(...)`

All states receive the state machine reference.

---

## Defining Hierarchy

Use the `ParentState` property:

```csharp
public override Type ParentState => typeof(MovementState);
```

Root states must return:

```csharp
public override Type ParentState => null;
```

---

## Example Hierarchy

```
EnemyBaseState
 ├── EnemyIdleState
 ├── EnemyChaseState
 │     └── EnemyChaseAggressiveState
 └── EnemySearchState
```

Transitioning to `EnemyChaseAggressiveState` triggers:

- Enter: `EnemyBaseState` (if not active)
- Enter: `EnemyChaseState`
- Enter: `EnemyChaseAggressiveState`

---

## License

MIT License  
See LICENSE for details.