using Microsoft.Extensions.Configuration;
using System.Security.Principal;

namespace SimpleDI
{
    public interface IMessageWriter
    {
        void Write(string message);
    }

    public class ConsoleMessageWriter : IMessageWriter
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class SecuredMessageWriter : IMessageWriter
    {
        private readonly IMessageWriter _messageWriter;

        private readonly IIdentity _identity;

        public SecuredMessageWriter(IIdentity identity, IMessageWriter messageWriter)
        {
            _identity = identity ?? throw new ArgumentException(null, nameof(identity));
            _messageWriter = messageWriter ?? throw new ArgumentException(null, nameof(messageWriter));
        }

        public void Write(string message)
        {
            if (_identity.IsAuthenticated)
            {
                _messageWriter.Write(message);
            }
        }
    }

    public class Salutation
    {
        private readonly IMessageWriter _messageWriter;

        public Salutation(IMessageWriter messageWriter)
        {
            _messageWriter = messageWriter ?? throw new ArgumentException(null, nameof(messageWriter));
        }

        public void Exclaim()
        {
            _messageWriter.Write("Hello DI!");
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath("D:\\GitHub\\design-patterns\\SimpleDI")
                .AddJsonFile("appsettings.json")
                .Build();
            var msgWriterName = configuration["messageWriter"];
            var msgWriterType = Type.GetType(typeName: msgWriterName, throwOnError: true);
            var msgWriter = Activator.CreateInstance(msgWriterType) as IMessageWriter;
            var securedMsgWriter = new SecuredMessageWriter(WindowsIdentity.GetCurrent(), msgWriter);
            var salutation = new Salutation(securedMsgWriter);
            salutation.Exclaim();
        }
    }
}
