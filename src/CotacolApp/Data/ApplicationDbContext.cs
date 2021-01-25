using System;
using System.Collections.Generic;
using System.Text;
using CotacolApp.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CotacolApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<CotacolUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}