using System;
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
        public Disk[] disks { get; set; }

        public Hardware()
        {
            readHardwareData();
        }

        private void readHardwareData()
        {
            ManagementClass management = new ManagementClass("Win32_Processor");
            ManagementObjectCollection managementobject = management.GetInstances();
            foreach (ManagementObject mngObject in managementobject)
            {
                numCores = mngObject.Properties["NumberOfCores"].Value.ToString();
                numLogProcs = mngObject.Properties["NumberOfLogicalProcessors"].Value.ToString();
                loadPercentage = mngObject.Properties["LoadPercentage"].Value.ToString() + "%";
            }

            ManagementClass management2 = new ManagementClass("Win32_OperatingSystem");
            ManagementObjectCollection managementobject2 = management2.GetInstances();
            foreach (ManagementObject mngObject in managementobject2)
            {
                totalMemorySize = fromKBToGB(mngObject.Properties["TotalVisibleMemorySize"].Value.ToString());
                freePhysMemory = fromKBToGB(mngObject.Properties["FreePhysicalMemory"].Value.ToString());
            }

            ManagementClass management3 = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection managementobject3 = management3.GetInstances();

            int i = 0;
            disks = new Disk[managementobject3.Count];
            foreach (ManagementObject mngObject in managementobject3)
            {
                disks[i++] = new Disk
                {
                    name = mngObject.Properties["Name"].Value.ToString(),
                    size = fromBytesToGB(mngObject.Properties["Size"].Value.ToString()),
                    freeSpace = fromBytesToGB(mngObject.Properties["FreeSpace"].Value.ToString())
                };
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
