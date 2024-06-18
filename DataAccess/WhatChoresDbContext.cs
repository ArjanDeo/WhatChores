using DataAccess.Tables;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class WhatChoresDbContext(DbContextOptions<WhatChoresDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        #region Tables
        public DbSet<tbl_USRealms> tbl_USRealms { get; set; }
        public DbSet<tbl_MythicPlusValues> tbl_MythicPlusValues { get; set; }
        public DbSet<tbl_ClassData> tbl_ClassData { get; set; }
        public DbSet<tbl_VaultRaidBosses> tbl_VaultRaidBosses { get; set; }

        #endregion
    }
}
