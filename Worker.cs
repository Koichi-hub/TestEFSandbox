using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using TestEF.Entities;

namespace TestEF
{
    public class Worker(IHostApplicationLifetime hostApplicationLifetime, IServiceProvider serviceProvider) 
        : IHostedService, IHostedLifecycleService
    {
        Task IHostedLifecycleService.StartingAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        async Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            await DoWork(cancellationToken);
        }

        Task IHostedLifecycleService.StartedAsync(CancellationToken cancellationToken)
        {
            hostApplicationLifetime.StopApplication();
            return Task.CompletedTask;
        }

        Task IHostedLifecycleService.StoppingAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        Task IHostedLifecycleService.StoppedAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            var scope = serviceProvider.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            await databaseContext.Database.MigrateAsync(cancellationToken);

            var guids = new List<Guid>()
            {
                Guid.Parse("83B58C48-A296-450B-BD76-CE0A658EC38E")
            };
            var messages = await databaseContext.Messages
                .Where(x => guids.Contains(x.Uid))
                .ToListAsync(cancellationToken);
            foreach (var message in messages)
            {
                Console.WriteLine(JsonConvert.SerializeObject(message));
            }
        }

        private async Task<ChatEntity> CreateChat(DatabaseContext databaseContext, string name)
        {
            var chat = new ChatEntity
            {
                Uid = Guid.NewGuid(),
                Name = name
            };
            databaseContext.Chats.Add(chat);
            await databaseContext.SaveChangesAsync();
            return chat;
        }

        private async Task AddMessage(DatabaseContext databaseContext, Guid chatUid, string text)
        {
            databaseContext.Messages.Add(new MessageEntity
            {
                Uid = Guid.NewGuid(),
                Text = text,
                ChatUid = chatUid,
            });
            await databaseContext.SaveChangesAsync();
        }
    }
}
