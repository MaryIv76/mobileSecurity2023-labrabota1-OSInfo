using System;
using System.Management;

namespace Lab1
{
    internal class Antivirus
    {
        public String displayName { get; set; }
        public String instanceGuid { get; set; }
        public String pathToSignedProductExe { get; set; }
        public String pathToSignedReportingExe { get; set; }
        public String productState { get; set; }

        public Antivirus(ManagementObject virusChecker)
        {
            readAntivirusData(virusChecker);
        }

        private void readAntivirusData(ManagementObject virusChecker)
        {
            try
            {
                displayName = virusChecker["displayName"].ToString();
                instanceGuid = virusChecker["instanceGuid"].ToString();
                pathToSignedProductExe = virusChecker["pathToSignedProductExe"].ToString();
                pathToSignedReportingExe = virusChecker["pathToSignedReportingExe"].ToString();
                productState = convertProductState(virusChecker["productState"].ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine("Can't read antivirus data: " + ex.Message);
            }
        }
        private String convertProductState(String state)
        {
            switch (state)
            {
                case "262144":
                case "393472":
                case "393216":
                    return "disabled and up to date";
                case "266240":
                case "397568":
                case "397312":
                    return "enabled and up to date";
                case "397584":
                    return "enabled and out of date";
                case "266256":
                    return "enabled";
                case "262160":
                    return "disabled";
                default:
                    return state;
            }
        }
    }
}

