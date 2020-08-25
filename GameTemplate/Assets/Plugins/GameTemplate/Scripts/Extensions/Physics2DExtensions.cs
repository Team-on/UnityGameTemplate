using UnityEngine;

 public static class Physics2DExtensions {
     public static void AddForce (this Rigidbody2D rigidbody2D, Vector2 force, ForceMode2D mode = ForceMode2D.Force) {
         switch (mode) {
         case ForceMode2D.Force:
             rigidbody2D.AddForce (force);
             break;
         case ForceMode2D.Impulse:
             rigidbody2D.AddForce (force / Time.fixedDeltaTime);
             break;
         case ForceMode2D.Acceleration:
             rigidbody2D.AddForce (force * rigidbody2D.mass);
             break;
         case ForceMode2D.VelocityChange:
             rigidbody2D.AddForce (force * rigidbody2D.mass / Time.fixedDeltaTime);
             break;
         }
     }
     
     public static void AddForce (this Rigidbody2D rigidbody2D, float x, float y, ForceMode2D mode = ForceMode2D.Force) {
         rigidbody2D.AddForce (new Vector2 (x, y), mode);
     }
 }

public enum ForceMode2D : byte {
    Force,
    Impulse,
    Acceleration,
    VelocityChange,
}