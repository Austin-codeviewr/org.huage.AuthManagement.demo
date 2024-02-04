using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using org.huage.AuthManagement.DataBase.Table;

namespace org.huage.AuthManagement.DataBase.Mapping;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(_ => _.Phone).IsRequired().HasMaxLength(11);
        builder.Property(_ => _.UserName).IsRequired().HasMaxLength(30);
        builder.Property(_ => _.PassWord).IsRequired().HasMaxLength(30);
        builder.Property(_ => _.Remark).HasMaxLength(50);
        builder.HasIndex(_ => _.Phone).IsUnique();
    }
}