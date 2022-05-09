using System.Text;

namespace DecoratorPattern
{
    public interface IShape
    {
        string AsString();
    }

    public class Square : IShape
    {
        private readonly double _side;

        public Square(double side)
        {
            _side = side;
        }

        public string AsString() => $"Square with { _side } cm side lenght";
    }

    public class Circle : IShape
    {
        private readonly double _radius;

        public Circle(double radius)
        {
            _radius = radius;
        }

        public string AsString() => $"Circle with radius { _radius }";
    }

    public abstract class ShapeDecorator : IShape
    {
        protected internal readonly List<Type> Types = new();

        protected internal readonly IShape Shape;

        public ShapeDecorator(IShape shape)
        {
            Shape = shape ?? throw new ArgumentException(null, nameof(shape));
            if(shape is ShapeDecorator shapeDecorator)
            {
                Types.AddRange(shapeDecorator.Types);
            }
        }

        public abstract string AsString();
    }

    public abstract class ShapeDecorator<TSelf, TCyclePolicy> : ShapeDecorator
        where TCyclePolicy : ShapeDecoratorCyclePolicy, new()
    {
        protected TCyclePolicy CyclePolicy = new();

        protected ShapeDecorator(IShape shape) : base(shape)
        {
            if (CyclePolicy.TypeAdditionAllowed(typeof(TSelf), Types))
            {
                Types.Add(typeof(TSelf));
            }
        }
    }

    public class ColoredShape : ShapeDecorator<ColoredShape, ThrowOnCyclePolicy>
    {
        private readonly string _color;

        public ColoredShape(IShape decorated, string color) : base(decorated) 
        {
            _color = color ?? throw new ArgumentException(null, nameof(color));
        }

        public override string AsString() 
        {
            var stringBuilder = new StringBuilder()
                .Append($"{ Shape.AsString() }");
            if (CyclePolicy.ApplicationAllowed(Types[0], Types.Skip(1).ToList()))
            {
                stringBuilder.Append($" has the color { _color }");
            }
            return stringBuilder.ToString();
        }
    }

    public class TransparentShape : IShape
    {
        private readonly double _transparency;

        private readonly IShape _decorated;

        public TransparentShape(IShape decorated, double transparency)
        {
            _decorated = decorated ?? throw new ArgumentException(null, nameof(decorated));
            _transparency = transparency;
        }

        public string AsString() => $"{ _decorated.AsString() } with transparency { _transparency }%";
    }

    public abstract class ShapeDecoratorCyclePolicy
    {
        public abstract bool TypeAdditionAllowed(Type type, IList<Type> allTypes);

        public abstract bool ApplicationAllowed(Type type, IList<Type> allTypes);
    }

    public class CyclesAllowedPolicy : ShapeDecoratorCyclePolicy
    {
        public override bool ApplicationAllowed(Type type, IList<Type> allTypes)
        {
            return true;
        }

        public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes)
        {
            return true;
        }
    }

    public class ThrowOnCyclePolicy : ShapeDecoratorCyclePolicy
    {
        private bool Handle(Type type, IList<Type> allTypes)
        {
            if (allTypes.Contains(type))
            {
                throw new InvalidOperationException($"Cycle detected! Type is already { type.FullName }");
            }
            return true;
        }

        public override bool ApplicationAllowed(Type type, IList<Type> allTypes)
        {
            return Handle(type, allTypes);
        }

        public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes)
        {
            return Handle(type, allTypes);
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            var square = new Square(4);
            Console.WriteLine(square.AsString());
            var circle = new Circle(5);
            Console.WriteLine(circle.AsString());
            var coloredSqurare = new ColoredShape(square, "red");
            Console.WriteLine(coloredSqurare.AsString());
            var coloredSqurare2 = new ColoredShape(coloredSqurare, "green");
            Console.WriteLine(coloredSqurare2.AsString());

            var transparentShape = new TransparentShape(coloredSqurare, 20.5);
            Console.WriteLine(transparentShape.AsString());
        }
    }
}