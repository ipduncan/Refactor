using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace AimHealth.Safari.BulkPosting.Bus
{
    public class Impersonation
    {
        //Windows token
        private IntPtr tokenHandle = new IntPtr(0);
        //Impersonated user
        private WindowsImpersonationContext impersonatedUser;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseHandle(IntPtr handle);

        public Impersonation()
            : this("AIM1", @"SAFCOM", "iwandays")
        {
        }

        private Impersonation(string domainName, string userName, string password)
        {
            //standard logon provider
            const int LOGON32_PROVIDER_DEFAULT = 0;
            //primary token
            const int LOGON32_LOGON_INTERACTIVE = 2;

            this.tokenHandle = IntPtr.Zero;

            //get a handle to an access token
            bool returnValue = LogonUser(userName
                , domainName, password, LOGON32_LOGON_INTERACTIVE
                , LOGON32_PROVIDER_DEFAULT, ref this.tokenHandle);

            if (false == returnValue)
            {
                //oops
                int ret = Marshal.GetLastWin32Error();
                throw new Exception("Impersonation Failed");
            }
        }

        public void Impersonate()
        {
            //create identity
            WindowsIdentity newId = new WindowsIdentity(this.tokenHandle);
            //start impersonating
            this.impersonatedUser = newId.Impersonate();
        }

        public void Revert()
        {
            //Stop impersonating
            if (this.impersonatedUser != null)
            {
                this.impersonatedUser.Undo();
            }

            //Release the token
            if (this.tokenHandle != IntPtr.Zero)
            {
                CloseHandle(this.tokenHandle);
            }
        }

    }
}
