using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Model
{
    public class BSNSContext : DbContext
    {
        public BSNSContext() : base("BSNSContext") { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<FriendInvitation> FriendInvitations { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<AccountPermission> AccountPermissions { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<AlertType> AlertTypes { get; set; }
        public DbSet<StatusUpdate> StatusUpdates { get; set; }
    }
}
