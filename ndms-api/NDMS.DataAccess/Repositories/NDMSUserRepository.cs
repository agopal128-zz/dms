using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DataAccess.Repositories
{
    public class NDMSUserRepository : BaseRepository<User>, INDMSUserRepository
    {
        #region Constructor(s)
        /// <summary>
        /// Default constructor
        /// </summary>
        public NDMSUserRepository() : base(new NDMSDataContext())
        {
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="context">DBContext associated</param>
        public NDMSUserRepository(NDMSDataContext context) : base(context)
        {
        }
        #endregion

        /// <summary>
        /// Method to add AD users to NDMS Users if the user doesn't exists in table and return the
        /// corresponding identifier
        /// </summary>
        /// <param name="adUsers">list of account names of AD Users</param>
        /// <returns>list of User Identifiers</returns>
        public List<int> AddADUsersToNDMS(IEnumerable<string> adUsers)
        {
            List<int> ndmsUsers = new List<int>();
            foreach (string accountName in adUsers)
            {
                //check if user already exists in NDMS database 
                var userId = Context.Users.FirstOrDefault(x => x.AccountName == accountName)?.Id;
                if (userId == null)
                {
                    //get user details from AD User Database
                    var adUser = Context.ADUsers.FirstOrDefault(x => x.AccountName == accountName);
                    var currentDate = TimeZoneUtility.GetCurrentTimestamp();
                    //add user to NDMS database
                    var ndmsUser = new User()
                    {
                        AccountName = adUser.AccountName,
                        FirstName = adUser.FirstName,
                        LastName = adUser.LastName,
                        Email = adUser.Email,
                        LastLocationID = adUser.LastLocationID,
                        DateCreated = currentDate,
                        DateModified = currentDate,
                        IsActive = true
                    };
                    Context.Users.Add(ndmsUser);
                    Save();
                    ndmsUsers.Add(ndmsUser.Id);
                }
                else
                {
                    ndmsUsers.Add(userId.Value);
                }
            }
            return ndmsUsers;
        }
    }
}
