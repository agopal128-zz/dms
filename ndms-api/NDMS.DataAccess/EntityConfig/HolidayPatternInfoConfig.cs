using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for Holiday Pattern entity
    /// </summary>

    internal class HolidayPatternInfoConfig : BaseEntityTypeConfiguration<HolidayPatternInfo>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public HolidayPatternInfoConfig() : base()
        {
            this.ToTable("HolidayPatternInfo").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.HolidayPatternId).HasColumnName("HolidayPatternID").IsRequired();
            this.Property(e => e.Date).HasColumnName("Date").IsRequired();
            this.Property(e => e.CreatedOn).HasColumnName("CreatedOn").IsRequired();
            this.Property(e => e.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            this.Property(e => e.LastModifiedOn).HasColumnName("LastModifiedOn").IsRequired();
            this.Property(e => e.LastModifiedBy).HasColumnName("LastModifiedBy").IsRequired();
            this.Property(e => e.IsActive).HasColumnName("IsActive").IsRequired();

            // Configure the foreign key with "HolidayPatterns" table here
            this.HasRequired<HolidayPattern>(s => s.HolidayPattern)
                .WithMany()
                .HasForeignKey(s => s.HolidayPatternId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "Users" table here
            this.HasRequired<User>(s => s.CreatedByUser)
                .WithMany()
                .HasForeignKey(s => s.CreatedBy)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "Users" table here
            this.HasRequired<User>(s => s.LastModifiedByUser)
                .WithMany()
                .HasForeignKey(s => s.LastModifiedBy)
                .WillCascadeOnDelete(true);

            this.HasRequired<HolidayPattern>(s => s.HolidayPattern)
                .WithMany(s => s.Holidays)
                .HasForeignKey(s => s.HolidayPatternId)
                .WillCascadeOnDelete(true);

        }
        #endregion
    }
}