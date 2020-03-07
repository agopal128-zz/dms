using NDMS.DomainModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for User entity
    /// </summary>
    internal class UserConfig:BaseEntityTypeConfiguration<User>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public UserConfig() : base()
        {
            this.ToTable("Users").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.AccountName).HasColumnName("AccountName").HasMaxLength(100)
                .IsRequired().HasColumnAnnotation(IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("[IX_AccountName]", 1) { IsUnique = true })); 
            this.Property(e => e.FirstName).HasColumnName("FirstName").HasMaxLength(200);
            this.Property(e => e.LastName).HasColumnName("LastName").HasMaxLength(200);
            this.Property(e => e.Email).HasColumnName("Email").HasMaxLength(200);
            this.Property(e => e.LastLocationID).HasColumnName("LastLocationID");
            this.Property(e => e.IsAdmin).HasColumnName("IsAdmin");
            this.Property(e => e.IsActive).HasColumnName("IsActive");
            this.Property(e => e.DateCreated).HasColumnName("DateCreated");
            this.Property(e => e.DateModified).HasColumnName("DateModified");
        }
        #endregion
    }
}
