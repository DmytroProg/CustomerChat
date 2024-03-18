using Azure.Data.Tables;
using CustomerChat.Models;
using CustomerChat.Repository;

namespace CustomerChat;

public class ChatService : BackgroundService
{
    private readonly TableClient _tableClient;

    public ChatService(TableClient tableClient)
    {
        _tableClient = tableClient;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            ChatRepository.Messages = _tableClient.Query<TableChatMessage>(
                        m => m.PartitionKey == nameof(ChatMessage))
                                   .OrderBy(m => m.CreatedAt).ToList();
            ChatRepository.Names = _tableClient.Query<TableChatUser>(
                        m => m.PartitionKey == nameof(ChatUser))
                                    .Select(m => m.Nick).ToList();
            await Task.Delay(10000);
        }
    }
}
