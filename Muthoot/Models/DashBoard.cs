using DocumentFormat.OpenXml.InkML;
using Humanizer;

namespace Muthoot.Models
{
    public class DashBoard
    {
        public string DashboardKey { get; set; }
        public string MonthCode { get; set; }
        public DateTime Today { get; set; }
        public string status { get; set; }
        public string msg { get; set; }
        public int OverallBaseCount { get; set; }
        public int BaseDailed { get; set; }
        public int Connected { get; set; }
        public int NotConnected { get; set; }
        public int Pending { get; set; }
        public decimal ConnectedPercentage { get; set; }
        public decimal NotConnectedPercentage { get; set; }
        public decimal PendingPercentage { get; set; }
        public int Disbursal { get; set; }
        public decimal DisbursalPercentage { get; set; }
        public decimal BaseDailedPercentage { get; set; }

        public int FTDBaseDailed { get; set; }
        public int FTDConnected { get; set; }
        public int FTDNotConnected { get; set; }
        public int FTDPending { get; set; }
        public int FTDDisbursal { get; set; }
        public double FTDBaseDailedPercentage { get; set; }
        public double FTDConnectedPercentage { get; set; }
        public double FTDNotConnectedPercentage { get; set; }
        public double FTDPendingPercentage { get; set; }
        public double FTDDisbursalPercentage { get; set; }



        //-------------------------------------------------------New----------------------------------------------------------------

        public string FollowupHead { get; set; }
        public string FollowupKey { get; set; }
        public int TotalFollowups { get; set; }
        public int CompletedFollowups { get; set; }
        public int PendingFollowups { get; set; }










        //-------------------------------------------------------Old----------------------------------------------------------------
        //Callback Model

        public int TodayCallbackTagged { get; set; }
        public int CallbackCompleted { get; set; }
        public int CallbackPending { get; set; }

        //Stage1 Model

        public int TotalUserProfileQualified { get; set; }
        public int UserProfileQualifiedCompleted { get; set; }
        public int UserProfileQualifiedPending { get; set; }
        //Stage2 Model
        public int TotalApplicationFreshLoan { get; set; }
        public int ApplicationFreshLoanCompleted { get; set; }
        public int ApplicationFreshLoanPending { get; set; }
        //Stage3 Model
        public int TotalDetailsSubmitted { get; set; }
        public int DetailsSubmittedCompleted { get; set; }
        public int DetailsSubmittedPending { get; set; }
        //Stage4 Model
        public int TotalKYCSuccess { get; set; }
        public int KYCSuccessCompleted { get; set; }
        public int KYCSuccessPending { get; set; }
        //Stage5 Model
        public int TotalForwardedToCreditUR { get; set; }
        public int ForwardedToCreditURCompleted { get; set; }
        public int ForwardedToCreditURPending { get; set; }
        //Stage6 Model
        public int TotalLoanApproved { get; set; }
        public int LoanApprovedCompleted { get; set; }
        public int LoanApprovedPending { get; set; }
        //Stage7 Model
        public int TotalBankAdded { get; set; }
        public int BankAddedCompleted { get; set; }
        public int BankAddedPending { get; set; }
        //Stage8 Model
        public int TotalSignAgreement { get; set; }
        public int SignAgreementCompleted { get; set; }
        public int SignAgreementPending { get; set; }


    }
}
