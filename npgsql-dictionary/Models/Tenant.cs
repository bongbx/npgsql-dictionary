using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace npgsql_dictionary.Models
{
    public class Tenant
    {
        public Tenant(string name)
        {
            Name = name;
            ExtraProperties = new Dictionary<string, object>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, object> ExtraProperties { get; set; }
    }


    public class TenantEntityConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.Property(x => x.ExtraProperties).HasColumnType("jsonb");
        }
    }
}
