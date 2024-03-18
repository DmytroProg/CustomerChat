using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CustomerChat.Models;

namespace CustomerChat.Data
{
    public class CustomerChatContext : DbContext
    {
        public CustomerChatContext (DbContextOptions<CustomerChatContext> options)
            : base(options)
        {
        }

        public DbSet<ChatUser> ChatUser { get; set; } = default!;
    }
}
