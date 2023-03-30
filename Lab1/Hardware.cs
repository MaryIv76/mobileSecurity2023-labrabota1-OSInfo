using System;
using System.Collections.Generic;
using System.Management;

namespace Lab1
{
    internal class Hardware
    {
        public String numCores { get; set; }
        public String numLogProcs { get; set; }
        public String loadPercentage { get; set; }
        public String totalMemorySize { get; set; }
        public String freePhysMemory { get; set; }
        public List<Disk> disks { get; set; }

        public Hardware()
        {
            readHardwareData();
        }

        private void readHardwareData()
        {
            try
            {
                ManagementClass management = new ManagementClass("Win32_Processor");
                ManagementObjectCollection managementobject = management.GetInstances();
                foreach (ManagementObject mngObject in managementobject)
                {
                    numCores = mngObject.Properties["NumberOfCores"].Value.ToString();
                    numLogProcs = mngObject.Properties["NumberOfLogicalProcessors"].Value.ToString();
                    loadPercentage = mngObject.Properties["LoadPercentage"].Value.ToString() + "%";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't read processor data: " + ex.Message);
            }

            try
            {
                ManagementClass management2 = new ManagementClass("Win32_OperatingSystem");
                ManagementObjectCollection managementobject2 = management2.GetInstances();
                foreach (ManagementObject mngObject in managementobject2)
                {
                    totalMemorySize = fromKBToGB(mngObject.Properties["TotalVisibleMemorySize"].Value.ToString());
                    freePhysMemory = fromKBToGB(mngObject.Properties["FreePhysicalMemory"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't read physical memory data: " + ex.Message);
            }

            try
            {
                ManagementClass management3 = new ManagementClass("Win32_LogicalDisk");
                ManagementObjectCollection managementobject3 = management3.GetInstances();

                disks = new List<Disk>();
                foreach (ManagementObject mngObject in managementobject3)
                {
                    try
                    {
                        disks.Add(new Disk
                        {
                            name = mngObject.Properties["Name"].Value.ToString(),
                            size = fromBytesToGB(mngObject.Properties["Size"].Value.ToString()),
                            freeSpace = fromBytesToGB(mngObject.Properties["FreeSpace"].Value.ToString())
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Can't read disk data: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't read logical disks data: " + ex.Message);
            }
        }

        private String fromBytesToGB(String bytes)
        {
            return $"{Math.Round(Convert.ToDouble(bytes) / 1024.0 / 1024.0 / 1024.0, 2)} GB";
        }

        private String fromKBToGB(String Kbytes)
        {
            return $"{Math.Round(Convert.ToDouble(Kbytes) / 1024.0 / 1024.0, 2)} GB";
        }
    }
}
