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

    public class ColoredShape : IShape
    {
        private readonly string _color;

        private readonly IShape _decorated;

        public ColoredShape(IShape decorated, string color) 
        {
            _decorated = decorated ?? throw new ArgumentException(null, nameof(decorated));
            _color = color ?? throw new ArgumentException(null, nameof(color));
        }

        public string AsString() => $"{ _decorated.AsString() } has color {_color}";
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
            var transparentShape = new TransparentShape(coloredSqurare, 20.5);
            Console.WriteLine(transparentShape.AsString());
        }
    }
}