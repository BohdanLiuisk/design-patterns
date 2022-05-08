using System.Collections;
using System.Text;

namespace CompositePatternt
{
    public class GraphicalObject
    {
        private Lazy<List<GraphicalObject>> _children = new();

        public string Color { get; set; }

        public List<GraphicalObject> Children => _children.Value;

        public virtual string Name { get; set; } = "Group";

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            Print(stringBuilder, 0);
            return stringBuilder.ToString();
        }

        private void Print(StringBuilder stringBuilder, int depth)
        {
            stringBuilder.Append('*', depth)
                .Append(string.IsNullOrWhiteSpace(Color) ? string.Empty : $"{Color} ")
                .AppendLine(Name);
            foreach (var child in Children)
            {
                child.Print(stringBuilder, depth + 1);
            }
        }
    }

    public class Square : GraphicalObject
    {
        public override string Name => "Square";
    }

    public class Circle : GraphicalObject
    {
        public override string Name => "Circle";
    }

    public interface IUnitOfGoods
    {
        Guid Id { get; }

        double Price { get; }
    }

    public class Box : IEnumerable<IUnitOfGoods>, IUnitOfGoods
    {
        public List<IUnitOfGoods> UnitOfGoods = new();

        public double Price => UnitOfGoods.Select(uofg => uofg.Price).Sum();

        private readonly Guid _id = Guid.NewGuid();

        public Guid Id => _id;

        public IEnumerator<IUnitOfGoods> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Product : IUnitOfGoods
    {
        private readonly Guid _id = Guid.NewGuid();

        private double _price;

        public Guid Id => _id;

        public double Price
        {
            get => _price;
            set { _price = value; }
        }
    }

    static class Program
    {
        public static int Main(string[] args)
        {
            var drawing = new GraphicalObject() { Name = "Super Drawing", Color = "BLACK" };
            drawing.Children.Add(new Circle() { Color = "RED" });
            drawing.Children.Add(new Square() { Color = "GREEN" });

            var group = new GraphicalObject() { Name = "Super Drawing internal group" };
            group.Children.Add(new Circle() { Color = "BLUE" });
            group.Children.Add(new Square() { Color = "BLUE" });
            var subGroup = new GraphicalObject() { Name = "sub internal group" };
            subGroup.Children.Add(new Circle() { Color = "Yellow" });
            group.Children.Add(subGroup);
            drawing.Children.Add(group);
            Console.WriteLine(drawing);

            var box1 = new Box();
            box1.UnitOfGoods.Add(new Product() { Price = 45 });
            box1.UnitOfGoods.Add(new Product() { Price = 30 });
            var box2 = new Box();
            box2.UnitOfGoods.Add(new Product() { Price = 20 });
            box2.UnitOfGoods.Add(new Product() { Price = 10 });
            var box3 = new Box();
            box3.UnitOfGoods.Add(new Product() { Price = 100 });
            box3.UnitOfGoods.Add(new Product() { Price = 200 });
            box2.UnitOfGoods.Add(box3);
            box1.UnitOfGoods.Add(box2);
            Console.WriteLine(box1.Price);
            return 0;
        }
    }
}