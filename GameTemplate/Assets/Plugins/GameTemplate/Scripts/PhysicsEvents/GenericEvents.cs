using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]  public class ColliderEvent : UnityEvent<Collider> { }
[Serializable] public class Collider2DEvent : UnityEvent<Collider2D> { }

[Serializable] public class CollisionEvent : UnityEvent<Collision> { }
[Serializable] public class Collision2DEvent : UnityEvent<Collision2D> { }
