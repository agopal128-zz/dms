using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for Scorecard Teams entity
    /// </summary>
    public class ScorecardTeamConfig : BaseEntityTypeConfiguration<ScorecardTeam>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScorecardTeamConfig() : base()
        {
            this.ToTable("ScorecardTeams").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.ScorecardId).HasColumnName("ScorecardID");
            this.Property(e => e.UserId).HasColumnName("UserID");
            this.Property(e => e.IsActive).HasColumnName("IsActive");
            this.Property(e => e.AssignedOn).HasColumnName("AssignedOn");
            this.Property(e => e.DeactivatedOn).HasColumnName("DeactivatedOn");
            this.HasRequired<Scorecard>(e => e.Scorecard).WithMany(e => e.Teams).
                HasForeignKey(e => e.ScorecardId).WillCascadeOnDelete(true);
            this.HasRequired<User>(e => e.User).WithMany(e=>e.TeamScorecards).
                HasForeignKey(e => e.UserId).WillCascadeOnDelete(true);
        }
        #endregion
    }
}
