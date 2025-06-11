using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Muthoot.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Web;

namespace Muthoot.Helper
{
	public class General
	{
        private SqlHepler Helper { get; }

        public General(IConfiguration configuration)
        {
            Helper = new SqlHepler(configuration);
        }
        public DataTable MuthootAccoutControl(string Critiria, string EmpCode, string UserName, string password, string UserID, string DOJ,string Region,string AmeyoID,string IsActive,string CreatedBy,string UpdatedOn,string UpdatedBy)
        {
            DataTable MenuDate = new DataTable("MenuData");
            try
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("@Critiria", Critiria);
                Params.Add("@EmpCode", EmpCode);
                Params.Add("@UserName", UserName);
                Params.Add("@Password", password);
                Params.Add("@UserID", UserID);
                Params.Add("@DOJ", DOJ);
                Params.Add("@Region", Region);
                Params.Add("@AmeyoID", AmeyoID);
                Params.Add("@IsActive", IsActive);
                Params.Add("@CreatedBy", CreatedBy);
                Params.Add("@UpdatedOn", UpdatedOn);
                Params.Add("@UpdatedBy", UpdatedBy);

                MenuDate  = Helper.ReturnTableData("USPMuthootAccountController", Params);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MenuDate;
        }
        public DataTable GetUIElements(string Critiria,string Condition1,string Condition2,string Condition3, string Condition4)
        {
            DataTable MenuDate = new DataTable("MenuData");
            try
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("@Critiria", Critiria);
                Params.Add("@Condition1", Condition1);
                Params.Add("@Condition2", Condition2);
                Params.Add("@Condition3", Condition3);
                Params.Add("@Condition4", Condition4);

                MenuDate  = Helper.ReturnTableData("USPGetUiElements", Params);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MenuDate;
        }
        public DataTable InsertCustomerDetails(string Action,string baseID, string StageCode, string MonthCode,string Disposition,
                                               string SubDispositon, string FollowUpDate, string Remarks, string CreatedBy, string UpdatedBy)
        {
            DataTable MenuDate = new DataTable("MenuData");
            try
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("@Action", Action);
                Params.Add("@baseID", baseID);
                Params.Add("@StageCode", StageCode);
                Params.Add("@MonthCode", MonthCode);
                Params.Add("@Disposition", Disposition);
                Params.Add("@SubDispositon", SubDispositon);
                Params.Add("@FollowUpDate", FollowUpDate);
                Params.Add("@Remarks", Remarks);
                Params.Add("@CreatedBy", CreatedBy);
                Params.Add("@UpdatedBy", UpdatedBy);

                MenuDate  = Helper.ReturnTableData("USPInsertCustomerDeatils", Params);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MenuDate;
        }
        public DataTable Muthoot_InsertUsers(string Action,string EmpCode,string UserName,string Password,string UserID,string DOJ,string Privilege,string Regoin,string AmeyoID,string IsActive,string CreatedBy,string UpdatedBy)
        {
            DataTable MenuDate = new DataTable("MenuData");
            try
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("@Action", Action);
                Params.Add("@EmpCode", EmpCode);
                Params.Add("@UserName", UserName);
                Params.Add("@Password", Password);
                Params.Add("@UserID", UserID);
                Params.Add("@DOJ", DOJ);
                Params.Add("@Privilege", Privilege);
                Params.Add("@Regoin", Regoin);
                Params.Add("@AmeyoID", AmeyoID);
                Params.Add("@IsActive", IsActive);
                Params.Add("@CreatedBy", CreatedBy);
                Params.Add("@UpdatedBy", UpdatedBy);

                MenuDate  = Helper.ReturnTableData("USPInsertUsers", Params);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MenuDate;
        }
        public DataTable UserMasterControl(string Action, string Condition1, string Condition2, string Condition3)
        {
            DataTable MenuDate = new DataTable("MenuData");
            try
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("@Action", Action);
                Params.Add("@Condition1", Condition1);
                Params.Add("@Condition2", Condition2);
                Params.Add("@Condition3", Condition3);

                MenuDate  = Helper.ReturnTableData("USPUserMaster", Params);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MenuDate;
        }
        public DataTable GetCallingHistory(string MobileNUmber,string MonthCode)
        {
            DataTable MenuDate = new DataTable("MenuData");
            try
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("@MobileNumber", MobileNUmber);
                Params.Add("@MonthCode", MonthCode);

                MenuDate  = Helper.ReturnTableData("USPGetCallingHistory", Params);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MenuDate;
        }
        public DataTable Fetch_CustomerDetails(string SearchBy, string Value)
        {
            DataTable MenuDate = new DataTable("MenuData");
            try
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("@SearchByKey", SearchBy);
                Params.Add("@SearchByValue", Value);

                MenuDate  = Helper.ReturnTableData("USPFetchCustomerDetails", Params);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MenuDate;
        }
        public DataTable BaseUpload(string UserName, string EmpCode,string BaseType)
        {
            DataTable MenuDate = new DataTable("MenuData");
            try
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("@UserName", UserName);
                Params.Add("@EmpCode", EmpCode);
                Params.Add("@BaseType", BaseType);

                MenuDate  = Helper.ReturnTableData("USPBaseUpload", Params);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MenuDate;
        }
        public DataTable InputFileUpload(string UserName, string EmpCode, string BaseType)
        {
            DataTable MenuDate = new DataTable("MenuData");
            try
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("@UserName", UserName);
                Params.Add("@EmpCode", EmpCode);
                Params.Add("@BaseType", BaseType);

                MenuDate  = Helper.ReturnTableData("USPInputUpload", Params);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MenuDate;
        }
        public DataTable Get_AgentFollowup_Details(string Action,string startdate, string enddate, string UserId,string Followup)
        {
            DataTable MenuDate = new DataTable("MenuData");
            try
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("@Action", Action);
                Params.Add("@StartDate", startdate);
                Params.Add("@EndDate", enddate);
                Params.Add("@UserId", UserId);
                Params.Add("@Followup", Followup);

                MenuDate  = Helper.ReturnTableData("GetAgentFollowupDetails", Params);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MenuDate;
        }
        public DataTable Get_Reports(string ReportType, string startdate, string enddate)
        {
            DataTable MenuDate = new DataTable("MenuData");
            try
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("@ReportType", ReportType);
                Params.Add("@StartDate", startdate);
                Params.Add("@EndDate", enddate);

                MenuDate  = Helper.ReturnTableData("USPGetReports", Params);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MenuDate;
        }
        public DataTable Login_Check(string userId, string password, string ipAddress,string sessionid)
        {
            DataTable MenuDate = new DataTable("MenuData");
            try
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("@userId", userId);
                Params.Add("@password", password);
                Params.Add("@ipAddress", ipAddress);
                Params.Add("@sessionid", sessionid);

                MenuDate  = Helper.ReturnTableData("SP_Check_Login", Params);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MenuDate;
        }
        public DataSet DashBoard(string DashBoard,string MonthCode,string Today)
        {
            DataSet MenuDate = new DataSet("MenuData");
            try
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("@DashBoard", DashBoard);
                Params.Add("@MonthCode", MonthCode);
                Params.Add("@Today", Today);
                MenuDate  = Helper.ReturnDataSet("USPMuthootDashboard", Params);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MenuDate;
        }
    }
}