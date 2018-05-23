using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISCore
{
    public class GISContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 使用 SQLServer 数据库
            optionsBuilder.UseSqlServer(GISConst.IsSql == "true" ? GISConst.sql : GISConst.local/*, b => b.MigrationsAssembly("GISWebApp")*/);

            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<Info> Infoes { get; set; }

        public virtual DbSet<User> Users { get; set; }
    }
}
