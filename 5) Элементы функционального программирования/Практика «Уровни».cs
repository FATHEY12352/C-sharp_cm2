using System;
using System.Collections.Generic;

namespace func_rocket
{
    public class LevelsTask
    {
        private static readonly Physics standardPhysics = new();

        private static readonly Vector standardTarget = new(600, 200);

        private static readonly Rocket standardRocket =
            new(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);

        private static readonly Gravity whiteGravity = (size, v) =>
        {
            var d = (standardTarget - v).Length;
            return (standardTarget - v).Normalize() * (-140 * d / (d * d + 1));
        };

        private static readonly Gravity blackGravity = (size, v) =>
        {
            var blackHolePosition = new Vector(
                (standardTarget.X + standardTarget.Y) / 2,
                (standardRocket.Location.X + standardRocket.Location.Y) / 2);
            var d = (blackHolePosition - v).Length;
            return new Vector(blackHolePosition.X - v.X, blackHolePosition.Y - v.Y)
            .Normalize() *
                   (300 * d / (d * d + 1));
        };

        private static Level CreateLevel(string name, Rocket rocket, Vector target,
            Gravity gravity, Physics physics)
        {
            return new Level(name, rocket, target, gravity, physics);
        }

        private static Vector GetUpGravity(Vector size, Vector v)
        {
            return new Vector(0, -300 / (size.Y - v.Y + 300));
        }

        private static Gravity GetBlackAndWhiteGravity(Gravity whiteGravity
            , Gravity blackGravity)
        {
            return (size, v) => (whiteGravity(size, v) + blackGravity(size, v)) / 2;
        }

        public static IEnumerable<Level> CreateLevels()
        {
            yield return CreateLevel("Zero", standardRocket, standardTarget,
                                     (size, v) => Vector.Zero, standardPhysics);

            yield return CreateLevel("Heavy", standardRocket, standardTarget,
                                     (size, v) => new Vector(0, 0.9), standardPhysics);

            yield return CreateLevel("Up", standardRocket, new Vector(700, 500),
                                     GetUpGravity, standardPhysics);

            yield return CreateLevel("WhiteHole", standardRocket, standardTarget,
                                     whiteGravity, standardPhysics);

            yield return CreateLevel("BlackHole", standardRocket, standardTarget,
                                     blackGravity, standardPhysics);

            yield return CreateLevel("BlackAndWhite", standardRocket, standardTarget,
                                     GetBlackAndWhiteGravity(whiteGravity, blackGravity) 
                                     , standardPhysics);
        }
    }
}
