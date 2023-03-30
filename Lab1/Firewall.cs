using System;
using NetFwTypeLib;

namespace Lab1
{
    internal class Firewall
    {
        public String firewallState { get; set; }

        public Firewall()
        {
            readFirewallState();
        }

        private void readFirewallState()
        {
            try
            {
                Type tpNetFirewall = Type.GetTypeFromProgID("HNetCfg.FwMgr", false);
                INetFwMgr mgrInstance = (INetFwMgr)Activator.CreateInstance(tpNetFirewall);

                bool blnEnabled = mgrInstance.LocalPolicy.CurrentProfile.FirewallEnabled;

                mgrInstance = null;
                tpNetFirewall = null;

                if (blnEnabled)
                {
                    firewallState = "Enabled";
                }
                else
                {
                    firewallState = "Disabled";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't read firewall data: " + ex.Message);
            }
        }
    }
}
