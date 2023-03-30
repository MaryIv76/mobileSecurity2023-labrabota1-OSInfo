using System;
using System.Collections.Generic;
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
        public List<String> updates { get; set; }

        public OS()
        {
            readOSVersion();
            readOSUpdates();
        }

        private void readOSVersion()
        {
            try
            {
                version = $"{OSVersion.GetOSVersion().Version.Major}." + $"{OSVersion.GetOSVersion().Version.Minor}." + $"{OSVersion.GetOSVersion().Version.Build}";
                type = OSVersion.GetOperatingSystem().ToString();
                bits = OSVersion.Is64BitOperatingSystem ? "64-Bit OS" : "32 - Bit OS";
            }
            catch(Exception ex)
            {
                Console.WriteLine("Can't read OS data: " + ex.Message);
            }
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

                updates = new List<String>();
                foreach (IUpdate update in sResult.Updates)
                {
                    updates.Add(update.Title);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't read OS updates data: " + ex.Message);
            }
        }
    }
}
