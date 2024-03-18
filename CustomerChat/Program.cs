using Azure.Data.Tables;
using Azure.Storage.Blobs;
using CustomerChat.Models;
using CustomerChat.Repository;
using Microsoft.EntityFrameworkCore;
using CustomerChat.Data;

namespace CustomerChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string connectionString = configuration["AzureConnString"];
            string tableName = configuration.GetSection("AppSettings")["TableName"];
            string blobContainerName = configuration.GetSection("AppSettings")["BlobContainerName"];

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<CustomerChatContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CustomerChatContext") ?? throw new InvalidOperationException("Connection string 'CustomerChatContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton(provider =>
            {
                TableServiceClient tableService = new TableServiceClient(connectionString);
                TableClient tableClient = tableService.GetTableClient(tableName);
                tableClient.CreateIfNotExists();
                return tableClient;
            });

            builder.Services.AddSingleton(provider =>
            {
                BlobServiceClient blobService = new BlobServiceClient(connectionString);
                BlobContainerClient blobContainerClient = blobService.GetBlobContainerClient(blobContainerName);
                blobContainerClient.CreateIfNotExists();
                return blobContainerClient;
            });

            builder.Services.AddScoped<IUser<TableChatUser>, UserRepository>();
            builder.Services.AddScoped<IChat<TableChatMessage>, ChatRepository>();
            builder.Services.AddScoped<IUser<ChatUser>, UserRepositorySql>();

            builder.Services.AddHostedService<ChatService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=User}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
