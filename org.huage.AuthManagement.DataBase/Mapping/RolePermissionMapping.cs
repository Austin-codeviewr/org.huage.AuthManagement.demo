using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using org.huage.AuthManagement.DataBase.Table;

namespace org.huage.AuthManagement.DataBase.Mapping;

public class RolePermissionMapping : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("Role_Permission");
    }
}