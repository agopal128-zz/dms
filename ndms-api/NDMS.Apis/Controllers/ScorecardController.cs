using NDMS.Apis.Models;
using NDMS.Business.Interfaces;
using NDMS.DomainModel.DTOs;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace NDMS.Apis.Controllers
{
    /// <summary>
    /// Web API Controller to deal with score card display
    /// </summary>
    [RoutePrefix("api/Scorecard")]
    public class ScorecardController : BaseController
    {
        #region Field(s)
        /// <summary>
        /// IScorecardManager reference
        /// </summary>
        private readonly IScorecardManager scorecardManager;

        /// <summary>
        /// IScorecardGraphManager reference
        /// </summary>
        private readonly IScorecardGraphManager scorecardGraphManager;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="scorecardManager">IScorecardManager reference</param>
        /// <param name="scorecardGraphManager">IScorecardGraphManager reference</param>
        public ScorecardController(IScorecardManager scorecardManager,
            IScorecardGraphManager scorecardGraphManager)
        {
            this.scorecardManager = scorecardManager;
            this.scorecardGraphManager = scorecardGraphManager;
        }
        #endregion

        #region Action Method(s)
        /// <summary>
        /// Api to retrieve details of individual scorecard kpi for a selected month and year
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">KPI Id</param>
        /// <param name="yearId">Identifier of the year</param>
        /// <param name="month">month</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("GetScorecardKPIData/{scorecardId}/{kpiId}/{yearId}/{month}")]
        [ResponseType(typeof(ScorecardKPIData))]
        public IHttpActionResult GetScorecardKPIData(int scorecardId, int kpiId, int yearId,
            int month)
        {
            ApiResponse<ScorecardKPIData> response = new ApiResponse<ScorecardKPIData>();
            response.Data = scorecardManager.GetScorecardKPIData(scorecardId, kpiId, yearId, month);
            return Ok(response);
        }

        /// <summary>
        /// Gets the scorecard kpi for secondary metric.
        /// </summary>
        /// <param name="kpiId">The kpi identifier.</param>
        /// <param name="targetId">The target identifier.</param>
        /// <param name="yearId">The year identifier.</param>
        /// <param name="month">The month.</param>
        /// <returns>Scorecard KPI data</returns>
        [Route("GetScorecardKPISecondaryMetricData/{kpiId}/{targetId}/{yearId}/{month}")]
        public IHttpActionResult GetScorecardKPISecondaryMetricData(int kpiId, int targetId, int yearId, int month)
        {
            ApiResponse<SecondaryMetricData> response = new ApiResponse<SecondaryMetricData>();
            response.Data = scorecardManager.GetScorecardKPISecondaryMetricData(kpiId, targetId, yearId, month);
            return Ok(response);
        }


        /// <summary>
        /// Api to retrieve the scorecard data for the current year/month depends on the
        /// tracking method selected for primary metric
        /// </summary>
        /// <param name="scorecardId">Identifier of the scorecard</param>
        /// <param name="yearId">Identifier of the year for which we need score card data</param>
        /// <param name="month">Identifier of the year for which we need score card data</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("GetScorecardData/{scorecardId}/{yearId}/{month}")]
        [ResponseType(typeof(ScorecardData))]
        public IHttpActionResult GetScorecardData(int scorecardId, int yearId, int month)
        {
            ApiResponse<ScorecardData> response = new ApiResponse<ScorecardData>();
            response.Data = scorecardManager.GetScorecardData(scorecardId, yearId, month);
            return Ok(response);
        }

        /// <summary>
        /// Api to retrieve fiscal month status of the primary metric of a KPI for an year
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">KPI Id</param>
        /// <param name="yearId">Identifier of the year</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("GetFiscalMonthStatusForKPI/{scorecardId}/{kpiId}/{yearId}")]
        [ResponseType(typeof(IEnumerable<FiscalMonthStatus>))]
        public IHttpActionResult GetFiscalMonthStatusForKPI(int scorecardId, int kpiId, int yearId)
        {
            ApiResponse<IEnumerable<FiscalMonthStatus>> response =
                                        new ApiResponse<IEnumerable<FiscalMonthStatus>>();
            response.Data = scorecardManager.GetFiscalMonthStatusForKPI(scorecardId,
                kpiId, yearId);
            return Ok(response);
        }

        /// <summary>
        /// Api to retrieve fiscal month status of a scorecard considering only primary metrics
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="yearId">Identifier of the year</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("GetFiscalMonthStatusForScorecard/{scorecardId}/{yearId}")]
        [ResponseType(typeof(IEnumerable<KpiFiscalMonthStatus>))]
        public IHttpActionResult GetFiscalMonthStatusForScorecard(int scorecardId, int yearId)
        {
            ApiResponse<IEnumerable<KpiFiscalMonthStatus>> response =
                                        new ApiResponse<IEnumerable<KpiFiscalMonthStatus>>();
            response.Data = scorecardManager.GetFiscalMonthStatusForScorecard(scorecardId,
                yearId);
            return Ok(response);
        }

        /// <summary>
        /// Api to get data to display graph for all kpi's for a given scorecard
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="yearId">Year Id</param>
        /// <param name="month">Month</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("GetScorecardGraphData/{scorecardId}/{yearId}/{month}")]
        [ResponseType(typeof(IEnumerable<KPIGraphData>))]
        public IHttpActionResult GetScorecardGraphData(int scorecardId, int yearId, int month)
        {
            ApiResponse<IEnumerable<KPIGraphData>> response =
                                   new ApiResponse<IEnumerable<KPIGraphData>>();
            response.Data = scorecardGraphManager.GetScorecardGraphData(scorecardId, yearId, month);
            return Ok(response);
        }

        /// <summary>
        /// Api to get data to display graph for a scorecard's selected kpi's primary metric
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">KPI Id</param>
        /// <param name="yearId">Year Id</param>
        /// <param name="month">Month</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("GetScorecardKPIGraphData/{scorecardId}/{kpiId}/{yearId}/{month}")]
        [ResponseType(typeof(KPIGraphData))]
        public IHttpActionResult GetScorecardKPIGraphData(int scorecardId, int kpiId,
                                                            int yearId, int month)
        {
            ApiResponse<KPIGraphData> response =
                                   new ApiResponse<KPIGraphData>();
            response.Data = scorecardGraphManager.GetScorecardKPIGraphData(scorecardId,
                kpiId, yearId, month);
            return Ok(response);
        }

        /// <summary>
        /// Gets the scorecard kpi graph data for selected metric.
        /// </summary>
        /// <param name="metricTargetId">The metric target identifier.</param>
        /// <param name="kpiId">The kpi identifier.</param>
        /// <param name="yearId">The year identifier.</param>
        /// <param name="month">The month.</param>
        /// <returns></returns>
        [Route("GetScorecardMetricKPIGraphData/{metricTargetId}/{kpiId}/{yearId}/{month}")]
        [ResponseType(typeof(KPIGraphData))]
        public IHttpActionResult GetScorecardMetricKPIGraphData(int metricTargetId, int kpiId, int yearId, int month)
        {
            ApiResponse<KPIGraphData> response =
                                  new ApiResponse<KPIGraphData>();
            response.Data = scorecardGraphManager.GetScorecardMetricKPIGraphData(metricTargetId, kpiId, yearId, month);
            return Ok(response);
        }

        [Route("GetBasicScorecardData/{scorecardId}/{yearId}/{month}")]
        [ResponseType(typeof(ScorecardData))]
        public IHttpActionResult GetBasicScorecardData(int scorecardId, int yearId, int month)
        {
            ApiResponse<ScorecardData> response = new ApiResponse<ScorecardData>();

            response.Data = scorecardManager.GetBasicScorecardData(scorecardId, yearId, month);

            return Ok(response);
        }

        
        #endregion
    }
}