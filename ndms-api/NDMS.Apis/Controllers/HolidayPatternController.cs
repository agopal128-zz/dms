using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using NDMS.Security;
using NDMS.Business.Interfaces;
using NDMS.DomainModel.DTOs;
using NDMS.Apis.Models;

namespace NDMS.Apis.Controllers
{
    /// <summary>
    /// API Controller to deal with Holiday patterns and holiday pattern definition
    /// </summary>
    [RoutePrefix("api/HolidayPattern")]
    [Authorize(Roles = NDMSSecurityConstants.AdminRole)]
    public class HolidayPatternController : BaseController
    {
        #region Field(s)
        /// <summary>
        /// HolidayPatternManager Reference
        /// </summary>
        private readonly IHolidayPatternManager holidayPatternManager;
        #endregion

        #region Constructor(s)
        public HolidayPatternController(IHolidayPatternManager holidayPatternManager)
        {
            this.holidayPatternManager = holidayPatternManager;
        }
        #endregion

        #region Action Method(s)

        /// <summary>
        /// Action method to retrieve all Holiday Patterns
        /// </summary>
        /// <returns>HolidayPatternItem List</returns>
        [Route("GetAllHolidayPatterns")]
        [ResponseType(typeof(IEnumerable<HolidayPatternItem>))]
        public IHttpActionResult GetAllHoldayPatterns()
        {
            ApiResponse<IEnumerable<HolidayPatternItem>> response = 
                              new ApiResponse<IEnumerable<HolidayPatternItem>>();
            response.Data = holidayPatternManager.GetAll();
            return Ok(response);

        }
        [Route("GetHolidayPattern/{id}")]
        public IHttpActionResult GetHolidayPattern([FromUri] int id)
        {
            ApiResponse<HolidayPatternItem> response =
                            new ApiResponse<HolidayPatternItem>();
            response.Data = holidayPatternManager.GetHolidayPattern(id);
            return Ok(response);
        }
        /// <summary>
        /// Action Method to Add or Update Holiday Pattern
        /// </summary>
        /// <param name="holidayPattern"></param>
        [HttpPost]
        [Route("AddOrUpdateHolidayPattern")]
        public IHttpActionResult PostHolidayPattern([FromBody] HolidayPatternItem holidayPattern)
        {
            CheckModelState();
            holidayPatternManager.AddOrUpdateHolidayPattern(holidayPattern, Username);
            return Ok();
        }
        /// <summary>
        /// Updates holidays associated with the holiday Pattern
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("UpdateHolidayPatternInfo")]
        public IHttpActionResult UpdateHolidayPatternInfo([FromBody] HolidayPatternInfoRequest request)
        {
            CheckModelState();
            holidayPatternManager.UpdateHolidayPatternMapping(request, Username);
            return Ok();
        }

        /// <summary>
        /// Copies once holiday Pattern ID to create new Pattern
        /// </summary>
        [Route("CopyHolidayPatternInfo/{holidayPatternId}")]
        public IHttpActionResult CopyHolidayPatternInfo([FromUri] int holidayPatternId)
        {

            holidayPatternManager.CopyHolidayPatternInfo(holidayPatternId, Username);
                return Ok();
        }

        #endregion

    }
}
