﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeamEnigma.Models;

namespace TeamEnigma.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TeamEnigma.Models.Item> Item { get; set; } = default!;

        
    }
}
