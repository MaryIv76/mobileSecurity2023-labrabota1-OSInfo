using System;
using System.Management;

namespace Lab1
{
    internal class AntivirusData
    {
        public Antivirus[] antiviruses { get; set; }

        public AntivirusData()
        {
            ManagementObjectSearcher wmiData = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM AntivirusProduct");
            ManagementObjectCollection data = wmiData.Get();
            antiviruses = new Antivirus[data.Count];

            int i = 0;
            foreach (ManagementObject virusChecker in data)
            {
                Antivirus antivirus = new Antivirus(virusChecker);
                antiviruses.SetValue(antivirus, i++);
            }
        }
    }
}
