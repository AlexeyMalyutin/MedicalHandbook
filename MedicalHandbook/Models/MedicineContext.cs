using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalHandbook.Models
{
    public class MedicineContext : DbContext
    {
        public MedicineContext(DbContextOptions<MedicineContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Medicine> Medicines { get; set; }
    }
}
