using System;
using OSVersionExtension;
using WUApiLib;

namespace Lab1
{
    internal class OS
    {
        public String version { get; set; }
        public String type { get; set; }
        public String bits { get; set; }
        public String numUpdates { get; set; }
        public String[] updates { get; set; }

        public OS()
        {
            readOSVersion();
            readOSUpdates();
        }

        private void readOSVersion()
        {
            version = $"{OSVersion.GetOSVersion().Version.Major}." + $"{OSVersion.GetOSVersion().Version.Minor}." + $"{OSVersion.GetOSVersion().Version.Build}";
            type = OSVersion.GetOperatingSystem().ToString();
            bits = OSVersion.Is64BitOperatingSystem ? "64-Bit OS" : "32 - Bit OS";
        }

        void readOSUpdates()
        {
            UpdateSession uSession = new UpdateSession();
            IUpdateSearcher uSearcher = uSession.CreateUpdateSearcher();
            uSearcher.Online = false;
            try
            {
                ISearchResult sResult = uSearcher.Search("IsInstalled=1 And IsHidden=0");
                numUpdates = "Found " + sResult.Updates.Count + " updates";

                int i = 0;
                updates = new String[sResult.Updates.Count];
                foreach (IUpdate update in sResult.Updates)
                {
                    updates[i++] = update.Title;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong: " + ex.Message);
            }
        }
    }
}
