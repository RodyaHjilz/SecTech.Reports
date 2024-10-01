using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecTech.Reports.Domain.Entity;


namespace SecTech.Reports.DAL.Infrastructure.Configurations
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x=>x.UserId).IsRequired();
            builder.Property(x=>x.EventId).IsRequired();
            builder.Property(x=>x.IsOnTime).IsRequired();
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.UserName).IsRequired();
            builder.Property(x=>x.EventName).IsRequired();
            builder.Property(x=>x.CheckInTime).IsRequired();
        }
    }
}
