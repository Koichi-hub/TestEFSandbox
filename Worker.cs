using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            //var chat = await CreateChat(databaseContext, "Chat_1");
            //await AddMessage(databaseContext, chat.Uid, "Keke");
            //await AddMessage(databaseContext, chat.Uid, "Lols");
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
