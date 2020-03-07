using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for Counter Measure Comments entity
    /// </summary>
    internal class CounterMeasureCommentConfig : BaseEntityTypeConfiguration<CounterMeasureComment>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CounterMeasureCommentConfig() : base()
        {
            this.ToTable("CounterMeasureComments").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.CounterMeasureId).HasColumnName("CounterMeasureID").IsRequired();
            this.Property(e => e.Comment).HasColumnName("Comment").HasMaxLength(4000).IsRequired();
            this.Property(e => e.CreatedOn).HasColumnName("CreatedOn").IsRequired();
            this.Property(e => e.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            this.Property(e => e.LastModifiedOn).HasColumnName("LastModifiedOn").IsRequired();
            this.Property(e => e.LastModifiedBy).HasColumnName("LastModifiedBy").IsRequired();
            // Configure the foreign key with "CounterMeasures" table here
            this.HasRequired<CounterMeasure>(s => s.CounterMeasure)
                .WithMany(s => s.CounterMeasureComments)
                .HasForeignKey(s => s.CounterMeasureId)
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

        }
        #endregion
    }
}
