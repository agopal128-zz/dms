using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{
    public class HolidayConfig : BaseEntityTypeConfiguration<Holiday>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public HolidayConfig() : base()
        {
            this.ToTable("Holidays").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.Date).HasColumnName("Date").IsRequired();
            this.Property(e => e.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
            this.Property(e => e.YearId).HasColumnName("YearID").IsRequired();
            // Configure the foreign key with "Years" table here
            this.HasRequired<Year>(s => s.Year)
                .WithMany()
                .HasForeignKey(s => s.YearId)
                .WillCascadeOnDelete(true);
        }
        #endregion
    }
}