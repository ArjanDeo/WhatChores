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
        #endregion
    }
}
