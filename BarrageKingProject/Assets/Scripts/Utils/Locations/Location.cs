using MoreLinq;
using PD.UnityEngineExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Adohi
{
    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public static class DirectionExtension
    {
        public static Direction ToDirection(this Vector2 vector)
        {
            var directionVector = vector.GetDirectionVector();
            if (directionVector.x > 0)
            {
                return Direction.Right;
            }
            else if (directionVector.x < 0)
            {
                return Direction.Left;
            }
            else if (directionVector.y > 0)
            {
                return Direction.Up;
            }
            else if (directionVector.y < 0)
            {
                return Direction.Down;
            }
            else
            {
                return Direction.None;
            }
        }

        public static Direction ToDirection(this Vector3 vector)
        {
            var directionVector = vector.GetXZVector().GetDirectionVector();
            if (directionVector.x > 0)
            {
                return Direction.Right;
            }
            else if (directionVector.x < 0)
            {
                return Direction.Left;
            }
            else if (directionVector.y > 0)
            {
                return Direction.Up;
            }
            else if (directionVector.y < 0)
            {
                return Direction.Down;
            }
            else
            {
                return Direction.None;
            }
        }

        public static Vector3 ToVector(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Vector3.forward;
                case Direction.Down:
                    return Vector3.back;
                case Direction.Left:
                    return Vector3.left;
                case Direction.Right:
                    return Vector3.right;
            }
            return Vector3.zero;
        }

        public static Quaternion ToRotation(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Quaternion.Euler(0f, 0f, 0f);
                case Direction.Down:
                    return Quaternion.Euler(0f, 180f, 0f);
                case Direction.Left:
                    return Quaternion.Euler(0f, 270f, 0f);
                case Direction.Right:
                    return Quaternion.Euler(0f, 90f, 0f);
            }
            return Quaternion.identity;
        }

    }
    public enum ApproximationType
    {
        Round,
        Convex,
        Concave
    }

    public enum AreaMaskShapeType
    {
        Vertical,
        Horizontal,
        Cross,
        Rectangle,
        Square,
        Rhombus,
        Circle,
        Arc,
        Cone
    }

    [System.Serializable]
    public struct LocationRect : IEquatable<LocationRect>
    {
        public int leftX;
        public int rightX;
        public int upY;
        public int downY;
        public int Width { get => rightX - leftX + 1; }
        public int Length { get => upY - downY + 1; }

        public LocationRect(int x1, int y1, int x2, int y2)
        {
            this.leftX = x1;
            this.rightX = x2;
            this.upY = y2;
            this.downY = y1;
        }

        public LocationRect(Location location)
        {
            this.leftX = location.X;
            this.rightX = location.X;
            this.upY = location.Y;
            this.downY = location.Y;
        }

        public Location[] ToLocations()
        {
            Location[] locations = new Location[Width * Length];
            for (int i = 0; i < Width * Length; i++)
            {
                locations[i] = new Location(leftX + i % Width, downY + i / Width);
            }
            return locations;

        }

        public bool IsComprised(int x1, int y1)
        {
            return x1 >= leftX && x1 <= rightX && y1 >= downY && y1 <= upY;
        }

        public bool IsComprised(Location location)
        {
            return location.X >= leftX && location.X <= rightX && location.Y >= downY && location.Y <= upY;
        }

        public bool IsOverlap(LocationRect locationRect)
        {
            return !(leftX > locationRect.rightX || rightX < locationRect.leftX || downY > locationRect.upY || upY < locationRect.downY);
        }

        public bool Equals(LocationRect other)
        {
            return leftX == other.leftX && rightX == other.rightX && upY == other.upY && downY == other.downY;
        }
    }

    [System.Serializable]
    public class LocationArea : IEquatable<LocationArea>
    {
        public HashSet<Location> locations;

        public int AreaSize { get => this.locations.Count; }
        public int MinX { get => this.locations.Min(l => l.X); }
        public int MaxX { get => this.locations.Max(l => l.X); }
        public int MinY { get => this.locations.Min(l => l.Y); }
        public int MaxY { get => this.locations.Max(l => l.Y); }
        public int Width { get => MaxX - MinX + 1; }
        public int Length { get => MaxY - MinY + 1; }
        public int CenterX { get => (MinX + MaxX) / 2; }
        public int CenterY { get => (MinY + MaxY) / 2; }
        public LocationArea()
        {
            this.locations = new HashSet<Location>();
        }

        public LocationArea(Location location) : this()
        {
            this.locations.Add(location);
        }

        public LocationArea(IEnumerable<Location> locations) : this()
        {
            this.locations.UnionWith(locations);
        }

        public HashSet<Location> ToLocations()
        {
            return this.locations;
        }

        public void Merge(Location location)
        {
            this.locations.Add(location);
        }

        public void Merge(IEnumerable<Location> locations)
        {
            this.locations.UnionWith(locations);
        }

        public void Merge(LocationArea locationArea)
        {
            this.locations.UnionWith(locationArea.locations);
        }

        public bool IsComprised(int x1, int y1)
        {
            return this.locations.Contains(new Location(x1, y1));
        }

        public bool IsComprised(Location location)
        {
            return this.locations.Contains(location);
        }

        public bool IsOverlap(LocationArea other)
        {
            return this.locations.Any(x => other.locations.Contains(x));
        }

        public int DistanceTo(Location location)
        {
            var distance = this.locations.Min(l => l.DistanceTo(location));
            return distance;
        }

        public int DistanceTo(Location location, out Location point)
        {
            var distance = this.locations.Min(l => l.DistanceTo(location));
            point = this.locations.MinBy(l => l.DistanceTo(location)).First();
            return distance;
        }

        public int DistanceTo(LocationArea other)
        {
            return this.locations.Min(l => l.DistanceTo(other));
        }

        public int DistanceTo(LocationArea other, out Location thisPoint, out Location otherPoint)
        {
            var minDistance = int.MaxValue;
            thisPoint = Location.Zero;
            otherPoint = Location.Zero;
            foreach (var location in this.locations)
            {
                var distance = location.DistanceTo(other, out var point);
                if (distance < minDistance)
                {
                    thisPoint = location;
                    otherPoint = point;
                    minDistance = distance;
                }
            }
            return minDistance;
        }

        public void MoveArea(int x, int y)
        {
            var moveLocation = new Location(x, y);
            var calculatedArea = new HashSet<Location>();
            foreach (var item in locations)
            {
                calculatedArea.Add(item + moveLocation);
            }
            this.locations = calculatedArea;
        }
        public void MoveArea(Location moveLocation)
        {
            var calculatedArea = new HashSet<Location>();
            foreach (var item in locations)
            {
                calculatedArea.Add(item + moveLocation);
            }
            this.locations = calculatedArea;
        }

        private bool IsEdge(Location location, int edgeThreshold = 1)
        {
            var mask = Location.GenerateAreaMask(edgeThreshold, AreaMaskShapeType.Rhombus);
            var filterArea = location.GetMaskedArea(mask);

            return !(filterArea.All(l => locations.Contains(l)) && filterArea.All(l => !locations.Contains(l)));
        }

        private bool IsInside(Location location, int edgeThreshold = 1)
        {
            var mask = Location.GenerateAreaMask(edgeThreshold, AreaMaskShapeType.Rhombus);
            var filterArea = location.GetMaskedArea(mask);

            return filterArea.All(l => locations.Contains(l));
        }

        public HashSet<Location> GetEdges(int edgeThreshold)
        {
            var calculatedArea = new HashSet<Location>();
            foreach (var location in locations)
            {
                if (IsEdge(location, edgeThreshold))
                {
                    calculatedArea.Add(location);
                }
            }
            return calculatedArea;
        }

        public HashSet<Location> GetInsides(int edgeThreshold)
        {
            var calculatedArea = new HashSet<Location>();
            foreach (var location in locations)
            {
                if (IsInside(location, edgeThreshold))
                {
                    calculatedArea.Add(location);
                }
            }
            return calculatedArea;
        }

        public bool Equals(LocationArea other)
        {
            return this.locations.All(x => other.locations.Contains(x)) && this.locations.Count == other.locations.Count;
        }

    }

    [System.Serializable]
    public struct Location : IEquatable<Location>
    {

        #region static variable

        public static readonly HashSet<Location> CrossMaskExceptCenter = new HashSet<Location>()
        {
            new Location(1, 0),
            new Location(-1, 0),
            new Location(0, 1),
            new Location(0, -1)
        };

        public static readonly HashSet<Location> CrossMask = new HashSet<Location>()
        {
            new Location(0, 0),
            new Location(1, 0),
            new Location(-1, 0),
            new Location(0, 1),
            new Location(0, -1)
        };

        public static readonly HashSet<Location> SquareMask = new HashSet<Location>()
        {
            new Location(0, 0),
            new Location(1, 0),
            new Location(-1, 0),
            new Location(0, 1),
            new Location(0, -1),
            new Location(1, 1),
            new Location(1, -1),
            new Location(-1, 1),
            new Location(-1, -1)
        };

        #endregion

        #region variable

        public int X;
        public int Y;
        public int Z;

        public static readonly Location Zero = new Location();
        public static readonly Location Up = new Location(0, 1);
        public static readonly Location Down = new Location(0, -1);
        public static readonly Location Left = new Location(-1, 0);
        public static readonly Location Right = new Location(1, 0);
        public static readonly Location NonAllocated = new Location(int.MinValue, int.MinValue, int.MinValue);

        #endregion

        #region constructor

        public Location(int x, int y)
        {
            X = x;
            Y = y;
            Z = 0;
        }

        public Location(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Location(float x, float y)
        {
            X = x.ToInt();
            Y = y.ToInt();
            Z = 0;
        }

        public Location(float x, float y, float z)
        {
            X = x.ToInt();
            Y = y.ToInt();
            Z = z.ToInt();
        }

        public Location(Vector2 vector2)
        {
            X = (int)vector2.x;
            Y = (int)vector2.y;
            Z = 0;
        }
        public Location(Vector3 vector3)
        {
            X = (int)vector3.x;
            Y = (int)vector3.z;
            Z = (int)vector3.y;
        }


        #endregion

        #region static method

        public static Location RandomLocation(int maxX, int maxY, int maxZ = 0)
        {
            return new Location(UnityEngine.Random.Range(0, maxX), UnityEngine.Random.Range(0, maxY), UnityEngine.Random.Range(0, maxZ));
        }

        public static Location RandomLocation(int minX, int maxX, int minY, int maxY, int minZ = 0, int maxZ = 0)
        {
            return new Location(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY), UnityEngine.Random.Range(minZ, maxZ));
        }

        public static Location RandomLocation(Map map, int padding = 0)
        {
            return new Location(UnityEngine.Random.Range(padding, map.width - (padding + 1)), UnityEngine.Random.Range(padding, map.length - (padding + 1)), UnityEngine.Random.Range(0, map.height - 1));
        }

        public static Location RandomLocation(Map map, int horizontalPadding = 0, int verticlaPadding = 0)
        {
            return new Location(UnityEngine.Random.Range(horizontalPadding, map.width - (horizontalPadding + 1)), UnityEngine.Random.Range(verticlaPadding, map.length - (verticlaPadding + 1)), UnityEngine.Random.Range(0, map.height - 1));
        }

        public static int Distance(Location from, Location to) => Mathf.Abs((from - to).X) + Mathf.Abs((from - to).Y);

        public static int Distance(Location from, LocationArea to) => to.locations.Min(l => l.DistanceTo(from));

        public static int Distance(Location from, LocationArea to, out Location point)
        {
            point = to.locations.MinBy(l => l.DistanceTo(from)).First();
            return to.locations.Min(l => l.DistanceTo(from));

        }

        public static Location DirectionToLocation(Direction direction, int offset = 1)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Location(0, offset, 0);
                case Direction.Down:
                    return new Location(0, -offset, 0);
                case Direction.Left:
                    return new Location(-offset, 0, 0);
                case Direction.Right:
                    return new Location(offset, 0, 0);
                default:
                    return Location.Zero;
            }
        }

        public static bool IsValidLocation(Location location, Map map, int padding = 0)
            => IsValidLocation(location, map.width, map.length, 0, padding);

        public static bool IsValidLocation(Location location, int width, int length, int height = 1, int padding = 0)
            => location.X.IsInRange(padding, width - padding) && location.Y.IsInRange(padding, length - padding) && location.Z.IsInRange(0, height + 1);

        public static HashSet<Location> GetLine(Location firstLocation, Location secondLocation)
        {
            var line = new HashSet<Location>();
            var startLocation = firstLocation.X <= secondLocation.X ? firstLocation : secondLocation;
            var endLocation = firstLocation.X <= secondLocation.X ? secondLocation : firstLocation;

            if (startLocation.X != endLocation.X)
            {
                var gradient = (float)(endLocation.Y - startLocation.Y) / (endLocation.X - startLocation.X);
                var yIntercept = startLocation.Y - gradient * startLocation.X;

                for (int i = startLocation.X; i <= endLocation.X; i++)
                {
                    line.Add(new Location(i, (gradient * i + yIntercept).RoundToInt()));
                }
            }

            startLocation = firstLocation.Y <= secondLocation.Y ? firstLocation : secondLocation;
            endLocation = firstLocation.Y <= secondLocation.Y ? secondLocation : firstLocation;
            if (startLocation.Y != endLocation.Y)
            {
                var gradient = (float)(endLocation.X - startLocation.X) / (endLocation.Y - startLocation.Y);
                var xIntercept = startLocation.X - gradient * startLocation.Y;

                for (int i = startLocation.Y; i <= endLocation.Y; i++)
                {
                    line.Add(new Location((gradient * i + xIntercept).RoundToInt(), i));
                }
            }
            if (line.Count == 0) "no!!".Log();
            return line;
        }

        public static HashSet<Location> GenerateAreaMask(int firstSize, AreaMaskShapeType areaMaskShapeType)
        {
            HashSet<Location> areaMask = new HashSet<Location>();
            switch (areaMaskShapeType)
            {
                case AreaMaskShapeType.Vertical:
                    firstSize.For(i => areaMask.Add(new Location(0, i)));
                    break;
                case AreaMaskShapeType.Horizontal:
                    firstSize.For(i => areaMask.Add(new Location(i, 0)));
                    break;
                case AreaMaskShapeType.Cross:
                    (-firstSize, firstSize).ForIncludeEnd(i => areaMask.Add(new Location(0, i)));
                    (-firstSize, firstSize).ForIncludeEnd(i => areaMask.Add(new Location(i, 0)));
                    break;
                case AreaMaskShapeType.Rectangle:
                    (-firstSize, firstSize).ForIncludeEnd(x =>
                        (-firstSize, firstSize).ForIncludeEnd(y =>
                        {
                            areaMask.Add(new Location(x, y));
                        }));
                    break;
                case AreaMaskShapeType.Square:
                    (-firstSize, firstSize).ForIncludeEnd(x =>
                        (-firstSize, firstSize).ForIncludeEnd(y =>
                        {
                            areaMask.Add(new Location(x, y));
                        }));
                    break;
                case AreaMaskShapeType.Rhombus:
                    (-firstSize, firstSize).ForIncludeEnd(x =>
                        (-(firstSize - Mathf.Abs(x)), (firstSize - Mathf.Abs(x))).ForIncludeEnd(y =>
                        {
                            areaMask.Add(new Location(x, y));
                        }));
                    break;
            }
            return areaMask;
        }

        public static HashSet<Location> GenerateAreaMask(float firstSize, float secondSize, AreaMaskShapeType areaMaskShapeType, ApproximationType approximationType, float roundThreshold = 0.5f)
        {
            HashSet<Location> areaMask = new HashSet<Location>();
            switch (areaMaskShapeType)
            {
                case AreaMaskShapeType.Vertical:
                    (firstSize, secondSize - 1).For(i => areaMask.Add(new Location(0, i)));
                    break;
                case AreaMaskShapeType.Horizontal:
                    (firstSize, secondSize - 1).For(i => areaMask.Add(new Location(i, 0)));
                    break;
                case AreaMaskShapeType.Cross:
                    (-firstSize, firstSize).For(i => areaMask.Add(new Location(0, i)));
                    (-secondSize, secondSize).For(i => areaMask.Add(new Location(i, 0)));
                    break;
                case AreaMaskShapeType.Rectangle:
                    (-firstSize, firstSize)
                        .For(x =>
                            secondSize
                            .For(y =>
                            {
                                areaMask.Add(new Location(x, y));
                            }));
                    break;
                case AreaMaskShapeType.Square:
                    (-firstSize, firstSize).For(x =>
                        (-secondSize, secondSize).For(y =>
                        {
                            areaMask.Add(new Location(x, y));
                        }));
                    break;
                case AreaMaskShapeType.Rhombus:

                    switch (approximationType)
                    {

                        case ApproximationType.Round:
                            (-firstSize, firstSize)
                                .For(x =>
                                (-(secondSize * (1f - Mathf.Abs(x) / firstSize)).Round(roundThreshold),
                                (secondSize * (1f - (Mathf.Abs(x) / firstSize))).Round(roundThreshold))
                                    .For(y =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));

                            (-secondSize, secondSize)
                                .For(y =>
                                (-(firstSize * (1f - Mathf.Abs(y) / secondSize)).Round(roundThreshold),
                                (firstSize * (1f - (Mathf.Abs(y) / secondSize))).Round(roundThreshold))
                                    .For(x =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));
                            break;

                        case ApproximationType.Convex:
                            (-firstSize, firstSize)
                                .For(x =>
                                (-(secondSize * (1f - Mathf.Abs(x) / firstSize)).Ceil(),
                                (secondSize * (1f - (Mathf.Abs(x) / firstSize))).Ceil())
                                    .For(y =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));

                            (-secondSize, secondSize)
                                .For(y =>
                                (-(firstSize * (1f - Mathf.Abs(y) / secondSize)).Ceil(),
                                (firstSize * (1f - (Mathf.Abs(y) / secondSize))).Ceil())
                                    .For(x =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));
                            break;

                        case ApproximationType.Concave:
                            (-firstSize, firstSize)
                                .For(x =>
                                (-(secondSize * (1f - Mathf.Abs(x) / firstSize)).Floor(),
                                (secondSize * (1f - (Mathf.Abs(x) / firstSize))).Floor())
                                    .For(y =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));

                            (-secondSize, secondSize)
                                .For(y =>
                                (-(firstSize * (1f - Mathf.Abs(y) / secondSize)).Floor(),
                                (firstSize * (1f - (Mathf.Abs(y) / secondSize))).Floor())
                                    .For(x =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));
                            break;
                    }
                    break;

                case AreaMaskShapeType.Circle:

                    switch (approximationType)
                    {
                        case ApproximationType.Round:
                            (-firstSize, firstSize)
                                .For(x =>
                                (-(secondSize * (1f - (x / firstSize).Square()).Sqrt()).Round(roundThreshold),
                                (secondSize * (1f - (x / firstSize).Square()).Sqrt()).Round(roundThreshold))
                                    .For(y =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));

                            (-secondSize, secondSize)
                                .For(y =>
                                (-(firstSize * (1f - (y / secondSize).Square()).Sqrt()).Round(roundThreshold),
                                (firstSize * (1f - (y / secondSize).Square()).Sqrt()).Round(roundThreshold))
                                    .For(x =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));
                            break;

                        case ApproximationType.Convex:
                            (-firstSize, firstSize)
                                .For(x =>
                                (-(secondSize * (1f - (x / firstSize).Square()).Sqrt()).Ceil(),
                                (secondSize * (1f - (x / firstSize).Square()).Sqrt()).Ceil())
                                    .For(y =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));

                            (-secondSize, secondSize)
                                .For(y =>
                                (-(firstSize * (1f - (y / secondSize).Square()).Sqrt()).Ceil(),
                                (firstSize * (1f - (y / secondSize).Square()).Sqrt()).Ceil())
                                    .For(x =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));
                            break;

                        case ApproximationType.Concave:
                            (-firstSize, firstSize)
                                .For(x =>
                                (-(secondSize * (1f - (x / firstSize).Square()).Sqrt()).Floor(),
                                (secondSize * (1f - (x / firstSize).Square()).Sqrt()).Floor())
                                    .For(y =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));

                            (-secondSize, secondSize)
                                .For(y =>
                                (-(firstSize * (1f - (y / secondSize).Square()).Sqrt()).Floor(),
                                (firstSize * (1f - (y / secondSize).Square()).Sqrt()).Floor())
                                    .For(x =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));
                            break;
                    }

                    break;
                case AreaMaskShapeType.Arc:

                    switch (approximationType)
                    {
                        case ApproximationType.Round:
                            (-firstSize, firstSize)
                                .For(x =>
                                (((secondSize.Square() - firstSize.Square()).Sqrt() / firstSize * x.Abs()).Round(1f - roundThreshold),
                                (secondSize.Square() - x.Square()).Sqrt().Round(roundThreshold))
                                    .For(y =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));

                            (secondSize.Square() - firstSize.Square()).Sqrt()
                                .For(y =>
                                (-(firstSize / secondSize * y).Round(roundThreshold),
                                (firstSize / secondSize * y).Round(roundThreshold))
                                    .For(x =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));

                            ((secondSize.Square() - firstSize.Square()).Sqrt(), secondSize)
                                .For(y =>
                                (-(secondSize.Square() - y.Square()).Sqrt().Round(roundThreshold),
                                (secondSize.Square() - y.Square()).Sqrt().Round(roundThreshold))
                                    .For(x =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));

                            break;
                        case ApproximationType.Convex:
                            (-firstSize, firstSize)
                                .For(x =>
                                (((secondSize.Square() - firstSize.Square()).Sqrt() / firstSize * x.Abs()).Floor(),
                                (secondSize.Square() - x.Square()).Sqrt().Ceil())
                                    .For(y =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));

                            (secondSize.Square() - firstSize.Square()).Sqrt()
                                .For(y =>
                                (-(firstSize / secondSize * y).Ceil(),
                                (firstSize / secondSize * y).Ceil())
                                    .For(x =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));

                            ((secondSize.Square() - firstSize.Square()).Sqrt(), secondSize)
                                .For(y =>
                                (-(secondSize.Square() - y.Square()).Sqrt().Ceil(),
                                (secondSize.Square() - y.Square()).Sqrt().Ceil())
                                    .For(x =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));
                            break;
                        case ApproximationType.Concave:
                            (-firstSize, firstSize)
                                .For(x =>
                                (((secondSize.Square() - firstSize.Square()).Sqrt() / firstSize * x.Abs()).Ceil(),
                                (secondSize.Square() - x.Square()).Sqrt().Floor())
                                    .For(y =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));

                            (secondSize.Square() - firstSize.Square()).Sqrt()
                                .For(y =>
                                (-(firstSize / secondSize * y).Floor(),
                                (firstSize / secondSize * y).Floor())
                                    .For(x =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));

                            ((secondSize.Square() - firstSize.Square()).Sqrt(), secondSize)
                                .For(y =>
                                (-(secondSize.Square() - y.Square()).Sqrt().Floor(),
                                (secondSize.Square() - y.Square()).Sqrt().Floor())
                                    .For(x =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));
                            break;
                    }
                    break;
                case AreaMaskShapeType.Cone:
                    switch (approximationType)
                    {
                        case ApproximationType.Round:
                            (-firstSize, firstSize)
                                .For(x =>
                                ((secondSize / firstSize * x.Abs()).Round(roundThreshold), secondSize - 1)
                                    .For(y =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));

                            secondSize
                                .For(y =>
                                (-(firstSize / secondSize * y).Round(roundThreshold),
                                (firstSize / secondSize * y).Round(roundThreshold))
                                    .For(x =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));
                            break;
                        case ApproximationType.Convex:
                            (-firstSize, firstSize)
                                .For(x =>
                                ((secondSize / firstSize * x.Abs()).Floor(), secondSize - 1)
                                    .For(y =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));

                            secondSize
                                .For(y =>
                                (-(firstSize / secondSize * y).Ceil(),
                                (firstSize / secondSize * y).Ceil())
                                    .For(x =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));
                            break;
                        case ApproximationType.Concave:
                            (-firstSize, firstSize)
                                .For(x =>
                                ((secondSize / firstSize * x.Abs()).Ceil(), secondSize - 1)
                                    .For(y =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));

                            secondSize
                                .For(y =>
                                (-(firstSize / secondSize * y).Floor(),
                                (firstSize / secondSize * y).Floor())
                                    .For(x =>
                                    {
                                        areaMask.Add(new Location(x, y));
                                    }));
                            break;
                    }
                    break;
            }

            return areaMask;
        }

        public static HashSet<Location> GetMaskedArea(Location currentLocation,
                                              HashSet<Location> masks,
                                              int maxWidth,
                                              int maxLength,
                                              int maxHeight = 1)
        {
            var maskedArea = new HashSet<Location>();
            foreach (var mask in masks)
            {
                var maskedLocation = currentLocation + mask;
                if (maskedLocation.IsValidLocation(maxWidth, maxLength, maxHeight))
                {
                    maskedArea.Add(maskedLocation);
                }
            }
            return maskedArea;
        }

        public static HashSet<Location> GetMaskedArea(Location currentLocation, HashSet<Location> masks, Map map)
        {
            var maskedArea = new HashSet<Location>();
            foreach (var mask in masks)
            {
                var maskedLocation = currentLocation + mask;
                if (maskedLocation.IsValidLocation(map))
                {
                    maskedArea.Add(maskedLocation);
                }
            }
            return maskedArea;
        }


        public static HashSet<Location> CombineAreas(HashSet<Location> a, HashSet<Location> b)
        {
            var combinedArea = new HashSet<Location>();

            foreach (var location in a)
            {
                combinedArea.Add(location);
            }
            foreach (var location in b)
            {
                combinedArea.Add(location);
            }

            return combinedArea;
        }

        public static HashSet<Location> UnionAreas(HashSet<Location> a, HashSet<Location> b)
        {
            var unionedArea = new HashSet<Location>();

            foreach (var location in a)
            {
                unionedArea.Add(location);
            }
            foreach (var location in b)
            {
                unionedArea.Add(location);
            }

            return unionedArea;
        }

        public static HashSet<Location> DifferenceAreas(HashSet<Location> a, HashSet<Location> b)
        {
            var DifferencedAreas = new HashSet<Location>(a);

            foreach (var location in b)
            {
                DifferencedAreas.RemoveIfContain(location);
            }

            return DifferencedAreas;
        }

        public static Location RotatedLocation(Location center, Location location, Direction direction)
        {
            var movedLocation = location - center;
            switch (direction)
            {
                case Direction.Up:
                    return new Location(movedLocation.X, movedLocation.Y, movedLocation.Z) + center;
                case Direction.Down:
                    return new Location(-movedLocation.X, -movedLocation.Y, movedLocation.Z) + center;
                case Direction.Left:
                    return new Location(-movedLocation.Y, movedLocation.X, movedLocation.Z) + center;
                case Direction.Right:
                    return new Location(movedLocation.Y, -movedLocation.X, movedLocation.Z) + center;
                case Direction.None:
                    break;
                default:
                    break;
            }
            return Location.Zero;
        }

        public static HashSet<Location> RotatedArea(Location center, HashSet<Location> area, Direction direction)
        {
            return area.Select(l => RotatedLocation(center, l, direction)).ToHashSet();
        }
        #endregion

        #region method

        public Location ClampX(int minX, int maxX)
        {
            X = Mathf.Clamp(X, minX, maxX);
            return this;
        }

        public Location ClampY(int minY, int maxY)
        {
            Y = Mathf.Clamp(Y, minY, maxY);
            return this;
        }

        public Location ClampZ(int minZ, int maxZ)
        {
            Z = Mathf.Clamp(Z, minZ, maxZ);
            return this;
        }

        public Location Clamp(int minX, int maxX, int minY, int maxY, int minZ, int maxZ)
        {
            X = Mathf.Clamp(X, minX, maxX);
            Y = Mathf.Clamp(Y, minY, maxY);
            Z = Mathf.Clamp(Z, minZ, maxZ);
            return this;
        }

        public Location Clamp(Map map)
        {
            X = Mathf.Clamp(X, 0, map.width - 1);
            Y = Mathf.Clamp(Y, 0, map.length - 1);
            Z = Mathf.Clamp(Z, 0, map.height - 1);
            return this;
        }


        public Location GetLeft(int offset = 1) => this + Left;
        public Location GetRight(int offset = 1) => this + Right;
        public Location GetUp(int offset = 1) => this + Up;
        public Location GetDown(int offset = 1) => this + Down;
        public Vector3 ToVector() => new Vector3(X, Z, Y);
        public int DistanceTo(Location to) => Distance(this, to);
        public int DistanceTo(LocationArea to) => Distance(this, to);
        public int DistanceTo(LocationArea to, out Location point) => Distance(this, to, out point);
        public (Direction horizontalDirection, Direction verticalDirection) DirectionTo(Location to)
        {
            var location = to - this;
            var horizontalDirection = location.X == 0 ? Direction.None : location.X > 0 ? Direction.Right : Direction.Left;
            var verticalDirection = location.Y == 0 ? Direction.None : location.Y > 0 ? Direction.Up : Direction.Down;

            return (horizontalDirection, verticalDirection);
        }
        public Direction OptimalDirectionTo(Location to)
        {
            if (this.Equals(to))
            {
                return Direction.None;
            }

            var differX = to.X - this.X;
            var differY = to.Y - this.Y;

            var xDirection = differX > 0 ? Direction.Right : Direction.Left;
            var yDirection = differY > 0 ? Direction.Up : Direction.Down;

            if (Mathf.Abs(differX) > Mathf.Abs(differY))
            {
                return xDirection;
            }
            else if (Mathf.Abs(differY) > Mathf.Abs(differX))
            {
                return yDirection;
            }
            else
            {
                return UnityEngine.Random.value >= 0.5f ? xDirection : yDirection;
            }
        }
        public Direction RandomDirectionTo(Location to)
        {
            if (this.Equals(to))
            {
                return Direction.None;
            }

            var direction = this.DirectionTo(to);
            if (direction.horizontalDirection.Equals(Direction.None))
            {
                return direction.verticalDirection;
            }
            else if (direction.verticalDirection.Equals(Direction.None))
            {
                return direction.horizontalDirection;
            }
            else
            {
                return UnityEngine.Random.value >= 0.5f ? direction.verticalDirection : direction.horizontalDirection;
            }
        }
        public Location LocationTo(Direction direction, int offset = 1) => this + DirectionToLocation(direction, offset);
        public bool IsValidLocation(Map map, int padding = 0) => IsValidLocation(this, map, padding);
        public bool IsValidLocation(int width, int length, int height = 1, int padding = 0) => IsValidLocation(this, width, length, height, padding);

        public HashSet<Location> GetMaskedArea(HashSet<Location> masks)
        {
            var maskedArea = new HashSet<Location>();
            foreach (var mask in masks)
            {
                var maskedLocation = this + mask;
                maskedArea.Add(maskedLocation);
            }
            return maskedArea;
        }

        public HashSet<Location> GetMaskedArea(HashSet<Location> masks,
                                               int width,
                                               int length,
                                               int height = 0)
            => GetMaskedArea(this, masks, width, length, height);

        public HashSet<Location> GetMaskedArea(HashSet<Location> masks, Map map)
            => GetMaskedArea(this, masks, map);

        #endregion

        #region operator overoading

        public static Location operator +(Location a) => a;
        public static Location operator -(Location a) => new Location(-a.X, -a.Y, -a.Z);
        public static Location operator +(Location a, Location b) => new Location(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Location operator +(Location a, Direction b)
        {
            switch (b)
            {
                case Direction.Up:
                    a.Y++;
                    return a;
                case Direction.Down:
                    a.Y--;
                    return a;
                case Direction.Left:
                    a.X--;
                    return a;
                case Direction.Right:
                    a.X++;
                    return a;
                default:
                    return a;
            }
        }
        public static Location operator -(Location a, Location b) => a + (-b);
        public static Location operator -(Location a, Direction b)
        {
            switch (b)
            {
                case Direction.Up:
                    a.Y--;
                    return a;
                case Direction.Down:
                    a.Y++;
                    return a;
                case Direction.Left:
                    a.X++;
                    return a;
                case Direction.Right:
                    a.X--;
                    return a;
                default:
                    return a;
            }
        }
        public static Location operator *(Location a, int b) => new Location(a.X * b, a.Y * b, a.Z * b);
        public static bool operator ==(Location a, Location b) => a.Equals(b);
        public static bool operator !=(Location a, Location b) => !a.Equals(b);

        #endregion

        public override string ToString() => $"Location (x: {X}, y: {Y}, z: {Z})";

        public override bool Equals(object obj)
            => obj is Location location &&
                (this.X, this.Y, this.Z) == (location.X, location.Y, location.Z);

        public bool Equals(Location location)
            => (this.X, this.Y, this.Z) == (location.X, location.Y, location.Z);

        public override int GetHashCode()
        {
            var hashCode = 373119288;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }
    }

    
}