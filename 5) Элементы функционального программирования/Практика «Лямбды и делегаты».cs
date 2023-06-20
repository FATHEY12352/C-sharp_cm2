using System;

namespace func_rocket
{
    public static class ForcesTask
    {
        public static RocketForce GetThrustForce(double forceValue)
        {
            return rocket => new Vector(
                forceValue * Math.Cos(rocket.Direction),
                forceValue * Math.Sin(rocket.Direction)
            );
        }

        public static RocketForce ConvertGravityToForce(Gravity gravity, Vector spaceSize)
        {
            return rocket => gravity(spaceSize, rocket.Location);
        }

        public static RocketForce Sum(params RocketForce[] forces)
        {
            return rocket => {
                var totalForce = Vector.Zero;
                foreach (var force in forces)
                {
                    totalForce += force(rocket);
                }
                return totalForce;
            };
        }
    }
}
