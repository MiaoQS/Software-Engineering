using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AutoOffice.Models;
using AutoOffice.Models.HomeViewModels;

namespace AutoOffice.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<HumanManage> HumanManages { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<JobRemind> JobReminds { get; set; }
        public DbSet<OfficialPaper> OfficialPapers { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            builder.Entity<HumanManage>().ToTable("HumanManage");
            builder.Entity<Message>().ToTable("Message");
            builder.Entity<JobRemind>().ToTable("JobRemind");
            builder.Entity<OfficialPaper>().ToTable("OfficialPaper");

      // Add your customizations after calling base.OnModelCreating(builder);
    }
    }
}
