using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Protomedia;

namespace Client {
    class Options {
        public MediaCommand? Command { get; set; }
        public string Host { get; set; } = "localhost";
    }

    class Program {
        static async Task Main(string[] args) {
            var options = ParseOptions(args);
            if (options.Command == null) {
                var names = string.Join(", ", Enum.GetNames<MediaCommand>());
                Console.WriteLine("Usage: client [hostname_or_ip] COMMAND");
                Console.WriteLine("Valid commands: " + names);
                return;
            }
            var command = options.Command.Value;
            string host = $"http://{options.Host}:5000";
            Console.WriteLine($"Connecting to: {host}");
            using var channel = GrpcChannel.ForAddress(host);
            var client =  new Media.MediaClient(channel);
            await client.ExecAsync(new CommandRequest { Command = command });
            Console.WriteLine($"Command sent: {command}");
        }

        static Options ParseOptions(string[] args) {
            var opt = new Options();
            if (args.Length >= 2)
                opt.Host = args[0];
            var cmdArg = args.LastOrDefault();
            if (Enum.TryParse<MediaCommand>(cmdArg, true, out var command))
                opt.Command = command;
            return opt;
        }
    }
}
