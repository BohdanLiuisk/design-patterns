using System.Collections;
using System.Collections.ObjectModel;

namespace CompositeNeuron
{
    public static class EnumerableExtensions
    {
        public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other)
        {
            if (ReferenceEquals(self, other)) return;
            foreach (var from in self)
            {
                foreach (var to in other)
                {
                    from.Out.Add(to);
                    to.In.Add(from);
                }
            }
        }
    }

    public class Neuron : IEnumerable<Neuron>
    {
        public float Value;
        public List<Neuron> In, Out;

        public Neuron()
        {
            In = new List<Neuron>();
            Out = new List<Neuron>();
        }

        public void ConnectTo(Neuron other)
        {
            Out.Add(other);
            other.In.Add(this);
        }

        public IEnumerator<Neuron> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class NeuronLayer : Collection<Neuron>
    {

    }

    static class Program
    {
        public static int Main(string[] args)
        {
            var neuron1 = new Neuron() { Value = 31 };
            var neuron2 = new Neuron() { Value = 45 };
            var neuron3 = new Neuron() { Value = 235 };
            neuron1.ConnectTo(neuron2);
            neuron2.ConnectTo(neuron3);

            var layer1 = new NeuronLayer();
            var layer2 = new NeuronLayer();

            neuron1.ConnectTo(layer1);
            layer1.ConnectTo(layer2);
            return 0;
        }
    }
}