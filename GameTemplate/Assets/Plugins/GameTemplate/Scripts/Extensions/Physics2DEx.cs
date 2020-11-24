using UnityEngine;

 public static class Physics2DEx {
     public static void AddForce (this Rigidbody2D rigidbody2D, Vector2 force, ForceMode2DEx mode = ForceMode2DEx.Force) {
         switch (mode) {
         case ForceMode2DEx.Force:
             rigidbody2D.AddForce (force);
             break;
         case ForceMode2DEx.Impulse:
             rigidbody2D.AddForce (force / Time.deltaTime);
             break;
         case ForceMode2DEx.Acceleration:
             rigidbody2D.AddForce (force * rigidbody2D.mass);
             break;
         case ForceMode2DEx.VelocityChange:
             rigidbody2D.AddForce (force * rigidbody2D.mass / Time.deltaTime);
             break;
         }
     }

    public static void AddRelativeForce(this Rigidbody2D rigidbody2D, Vector2 force, ForceMode2DEx mode = ForceMode2DEx.Force) {
        switch (mode) {
            case ForceMode2DEx.Force:
                rigidbody2D.AddRelativeForce(force);
                break;
            case ForceMode2DEx.Impulse:
                rigidbody2D.AddRelativeForce(force / Time.deltaTime);
                break;
            case ForceMode2DEx.Acceleration:
                rigidbody2D.AddRelativeForce(force * rigidbody2D.mass);
                break;
            case ForceMode2DEx.VelocityChange:
                rigidbody2D.AddRelativeForce(force * rigidbody2D.mass / Time.deltaTime);
                break;
        }
    }

    public static void AddForce (this Rigidbody2D rigidbody2D, float x, float y, ForceMode2DEx mode = ForceMode2DEx.Force) {
         rigidbody2D.AddForce (new Vector2 (x, y), mode);
     }
 }

public enum ForceMode2DEx : byte {
    Force,
    Impulse,
    Acceleration,
    VelocityChange,
}