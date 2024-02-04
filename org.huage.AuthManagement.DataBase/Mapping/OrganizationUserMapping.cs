using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using org.huage.AuthManagement.DataBase.Table;

namespace org.huage.AuthManagement.DataBase.Mapping;

public class OrganizationUserMapping : IEntityTypeConfiguration<OrganizationUser>
{
    public void Configure(EntityTypeBuilder<OrganizationUser> builder)
    {
        builder.ToTable("Organization_User");
    }
}