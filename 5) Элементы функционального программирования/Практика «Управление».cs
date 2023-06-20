using System;

namespace func_rocket
{
    public class ControlTask
    {
        private static double angle;

        public static Turn ControlRocket(Rocket rocket, Vector target)
        {
            var direction = new Vector(target.X - rocket.Location.X, target.Y - rocket.Location.Y);

            var currentVelocity = rocket.Velocity;
            var currentDirection = rocket.Direction;

            if (Math.Abs(direction.Angle - currentDirection)
             < 0.5 || Math.Abs(direction.Angle
              - currentVelocity.Angle) < 0.5)
            {
                angle = (direction.Angle - currentDirection + direction.Angle - currentVelocity.Angle) / 2;
            }
            else
            {
                angle = direction.Angle - currentDirection;
            }

            return angle < 0 ? Turn.Left : angle > 0 ? Turn.Right : Turn.None;
        }
    }
}
