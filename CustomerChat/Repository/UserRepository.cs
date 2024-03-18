using Azure.Data.Tables;
using Azure.Storage.Blobs;
using CustomerChat.Models;
using Microsoft.Extensions.Azure;

namespace CustomerChat.Repository;

public class UserRepository : IUser<TableChatUser>
{
    private readonly BlobContainerClient _blobContainerClient;
    private readonly TableClient _tableClient;

    public UserRepository(BlobContainerClient blobContainerClient, TableClient tableClient)
    {
        _blobContainerClient = blobContainerClient;
        _tableClient = tableClient;
    }

    public async Task<TableChatUser?> Login(TableChatUser user)
    {
        var userFromTable = _tableClient.Query<TableChatUser>(u => u.Nick == user.Nick
        && u.Password == user.Password).FirstOrDefault();

        if(userFromTable != null)
        {
            userFromTable.LastSeenAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            _tableClient.UpdateEntity<TableChatUser>(userFromTable, userFromTable.ETag);
        }

        return userFromTable;
    }

    public async Task<TableChatUser> Register(TableChatUser user, IFormFile AvatarUrl)
    {
        BlobClient blobClient = _blobContainerClient.GetBlobClient(user.Nick);
        await blobClient.UploadAsync(AvatarUrl.OpenReadStream());
        user.AvatarUrl = blobClient.Uri.ToString();
        user.JoinedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        user.LastSeenAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

        await _tableClient.AddEntityAsync(user);
        return user;
    }

    public bool IsNickUnique(string nick)
    {
        return _tableClient.Query<TableChatUser>(u => u.Nick == nick)
            .FirstOrDefault() is null;
    }
}
