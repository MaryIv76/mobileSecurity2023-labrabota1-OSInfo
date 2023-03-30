using System;
using System.Collections.Generic;
using System.Management;

namespace Lab1
{
    internal class AntivirusData
    {
        public List<Antivirus> antiviruses { get; set; }

        public AntivirusData()
        {
            try
            {
                ManagementObjectSearcher wmiData = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM AntivirusProduct");
                ManagementObjectCollection data = wmiData.Get();
                antiviruses = new List<Antivirus>();

                foreach (ManagementObject virusChecker in data)
                {
                    Antivirus antivirus = new Antivirus(virusChecker);
                    antiviruses.Add(antivirus);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Can't read antivirus data: " + ex.Message);
            }
        }
    }
}
