namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO to represent User Entity
    /// </summary>
    public class UserItem
    {
        /// <summary>
        /// Identifier of User
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Account Name of User
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// First Name of User
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name of User
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email Id of User
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Flag to represent whether user is Super Admin or Not
        /// </summary>
        public bool IsSuperAdmin { get; set; }
    }
}
