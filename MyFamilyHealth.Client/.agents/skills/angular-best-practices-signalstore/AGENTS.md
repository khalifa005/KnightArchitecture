# Angular Signalstore Best Practices

> Use with the core `angular-best-practices` skill.

---

## 1. SignalStore

**Impact: HIGH** (Local/feature state)

### 1.1 Use rxMethod for RxJS Integration

**Impact: MEDIUM** (Debounce, switchMap, and other RxJS operators)

Use `rxMethod` from `@ngrx/signals/rxjs-interop` for RxJS-based side effects like debounced search.

**Example:**

```typescript
searchUsers: rxMethod<string>(pipe(
  debounceTime(300), switchMap(q => http.get<User[]>(`/api/users?q=${q}`)),
  tap(users => patchState(store, { users })),
))
```

### 1.2 Use SignalStore for Shared State

**Impact: HIGH** (Simpler than NgRx, signal-based reactivity)

Use `signalStore()` from `@ngrx/signals` for shared state across components without full NgRx overhead. Combine `withState()`, `withComputed()`, and `withMethods()` to define state shape, derived values, and actions.

**Example:**

```typescript
export const UsersStore = signalStore(
  withState({ users: [] as User[], loading: false }),
  withMethods((store) => ({ load: () => patchState(store, { loading: true }) })),
);
```

### 1.3 Use withComputed for Derived State

**Impact: MEDIUM** (Memoized derivation, cleaner store API)

Add derived state to SignalStore using `withComputed` instead of computing in components. Keeps derivation logic centralized and memoized.

**Example:**

```typescript
withComputed((store) => ({
  filteredUsers: computed(() => store.users().filter(u => u.name.includes(store.filter()))),
}))
```

### 1.4 Use withEntities for Collections

**Impact: MEDIUM** (O(1) lookups, standardized CRUD)

Use `withEntities<T>()` from `@ngrx/signals/entities` for collection management with O(1) lookups and standardized CRUD operations.

**Example:**

```typescript
withEntities<User>(),
withMethods((store) => ({
  update(id: string, changes: Partial<User>) { patchState(store, updateEntity({ id, changes })); },
}))
```

---
