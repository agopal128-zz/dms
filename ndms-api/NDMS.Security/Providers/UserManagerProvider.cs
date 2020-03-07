using NDMS.DataAccess.Repositories;
using NDMS.DomainModel;

namespace NDMS.Security
{
    public class UserManagerProvider
    {
        #region Public Method(s)
        /// <summary>
        /// Returns the user manager requested
        /// </summary>
        /// <param name="authSvcIdentifier">Unique string to identify the auth service</param>
        /// <returns>Requested auth service</returns>
        public static IUserManager GetUserManager(string authSvcIdentifier)
        {
            var userRepo = new BaseRepository<User>();
            switch (authSvcIdentifier)
            {
                case "NOV":
                    {
                        return new NOVUserManager(userRepo);
                    }
                default:
                    {
                        return new NOVUserManager(userRepo);
                    }
            }
        }
        #endregion
    }
}
