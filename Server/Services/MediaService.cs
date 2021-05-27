using System.Diagnostics;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Protomedia;

namespace Server {
    public class MediaService : Media.MediaBase {
        private readonly ILogger<MediaService> _logger;
        public MediaService(ILogger<MediaService> logger) {
            _logger = logger;
        }

        public override Task<CommandReply> Exec(CommandRequest request, ServerCallContext context) {
            _logger.LogInformation("Executing command: {Command}", request.Command);
            Process.Start("nircmd", $"sendkeypress {GetVirtualKeyCode(request.Command)}");
            return Task.FromResult(new CommandReply());
        }

        private static string GetVirtualKeyCode(MediaCommand command) =>
            command switch {
                MediaCommand.PlayPause => "0xB3",
                MediaCommand.Stop => "0xB2",
                MediaCommand.Previous => "0xB1",
                MediaCommand.Next => "0xB0",
                _ => ""
            };
    }
}
