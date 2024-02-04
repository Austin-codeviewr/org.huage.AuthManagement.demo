﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using org.huage.AuthManagement.DataBase.Table;

namespace org.huage.AuthManagement.DataBase.Mapping;

public class PermissionMapping : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.Property(_ => _.Name).IsRequired();
        builder.HasIndex(_ => _.Name).IsUnique();
    }
}