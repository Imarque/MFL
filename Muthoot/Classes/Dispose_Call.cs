using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Muthoot.Classes; // Assuming Utilities is here

namespace Muthoot
{
    [Serializable]
    public class Dispose_Call
    {
        private readonly Utilities log;
        private readonly string dispose_api_protocol;

        public delegate void DisposeAsync1(string campaignId, string crtObjectId, string userCrtObjectId,
                                           string customerId, string sessionId, string phone,
                                           string dispositionCode, string dispositionAttr,
                                           string selfCallback, string process);

        public Dispose_Call(IHttpContextAccessor httpContextAccessor,
                            IConfiguration configuration,
                            IWebHostEnvironment env)
        {
            log = new Utilities(httpContextAccessor, configuration, env);
            dispose_api_protocol = configuration["ApiSettings:dispose_api_protocol"];
        }

        public void CallDispose(string campaignId, string crtObjectId, string userCrtObjectId,
                                string customerId, string sessionId, string phone,
                                string dispositionCode, string dispositionAttr,
                                string selfCallback, string process)
        {
            log.WriteToFile("ICC_Dialer", "Dispose", $"Phone: {phone}, CallBackTime: {dispositionAttr}", "Status: Entered CallDispose");
            DisposeAsync1 AsyncDisp = new DisposeAsync1(this.TagToDialer);
            AsyncDisp.Invoke(campaignId, crtObjectId, userCrtObjectId, customerId, sessionId, phone, dispositionCode, dispositionAttr, selfCallback, process);
        }

        public void TagToDialer(string campaignId, string crtObjectId, string userCrtObjectId,
                                string customerId, string sessionId, string phone,
                                string dispositionCode, string dispositionAttr,
                                string selfCallback, string process)
        {
            log.WriteToFile("ICC_Dialer", "Dispose", $"Phone: {phone}, CallBackTime: {dispositionAttr}", "Status: Entered TagToDialer");
            try
            {
                using (WebClient client = new WebClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    log.WriteToFile("ICC_Dialer", "Dispose", $"Phone: {phone}, CallBackTime: {dispositionAttr}", "Status: Entered SecurityProtocolType");

                    string selfCallbackFlag = string.IsNullOrEmpty(dispositionAttr) ? "false" : "true";
                    string baseURL = $"http://{dispose_api_protocol}/dacx/dispose?" +
                        $"phone={phone}&campaignId={campaignId}&crtObjectId={crtObjectId}" +
                        $"&userCrtObjectId={userCrtObjectId}&customerId={customerId}" +
                        $"&dispositionCode={dispositionCode}&dispositionAttr=local-{dispositionAttr}" +
                        $"&sessionId={sessionId}&selfCallback={selfCallbackFlag}";

                    string jsonResponse = client.DownloadString(baseURL);
                    string concat_msg = $"campId:{campaignId}|phone:{phone}|crtObjectId:{crtObjectId}|dispoCode:{dispositionCode}|Response:{jsonResponse}";

                    log.WriteToFile("ICC_Dialer", "Dispose", $"Phone: {phone}, CallBackTime: {dispositionAttr}", "Status: " + jsonResponse);
                }
            }
            catch (WebException ex)
            {
                log.WriteToFile("ICC_Dialer", "Dispose", "Failure", ex.Message);
            }
        }
    }
}
