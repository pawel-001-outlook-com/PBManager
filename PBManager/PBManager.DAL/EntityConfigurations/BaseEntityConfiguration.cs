using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBManager.Core.Models;

namespace PBManager.DAL.EntityConfigurations
{
    public class BaseEntityConfiguration : EntityTypeConfiguration<BaseEntity>
    {
        // public BaseEntityConfiguration()
        // {
        //     HasKey(t => t.Id);
        //     Property(t => t.Id)
        //         .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        // }

    }
}
