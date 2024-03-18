using Azure.Data.Tables;
using Azure.Storage.Blobs;
using CustomerChat.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChat.Repository;

public class ChatRepository : IChat<TableChatMessage>
{
    private readonly BlobContainerClient _blobContainerClient;
    private readonly TableClient _tableClient;

    public static List<TableChatMessage> Messages;
    public static List<string> Names;

    public ChatRepository(TableClient tableClient, BlobContainerClient blobContainerClient)
    {
        _tableClient = tableClient;
        _blobContainerClient = blobContainerClient;
    }

    public async Task CreateMessage(TableChatMessage message, IFormFile FileUrl)
    {
        if(FileUrl != null) {
            string ext = Path.GetExtension(FileUrl.FileName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(
                DateTime.UtcNow.ToString() + "_" + message.Sender + ext);
            await blobClient.UploadAsync(FileUrl.OpenReadStream());
            message.FileUrl = blobClient.Uri.ToString();
        }
        _tableClient.AddEntity<TableChatMessage>(message);
    }

    public Task<FileResult> DownloadFile(TableChatMessage message)
    {
        throw new NotImplementedException();
    }

    public async Task<ChatViewModel> GetMessages()
    {
        ChatViewModel viewModel = new ChatViewModel()
        {
            Messages = Messages,
            Names = Names,
        };

        return viewModel;

    }
}
