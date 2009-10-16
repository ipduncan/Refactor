using System;
using System.Security.Principal;
using System.Collections.Specialized;
using AimHealth.Safari.BulkPosting.Exceptions;
using System.Data;
using System.Configuration;
using System.Linq;

namespace AimHealth.Safari.BulkPosting.Bus
{
    public interface IUser
    {
        void IdentifyWindowsUser();
        int Key { get; set; }
        string Login { get; set; }
        bool IsAuthenticated { get; set; }
        bool IsAuthorizedPayerFeeBatchCreate { get; set; }
        StringCollection Roles { get; set; }
        bool AuthorizeAction(StringCollection allowedRoles);
}


  
    public class User : IUser
    {
 
        private WindowsIdentity _currentWindowsUser;
        private int _key = 0;
        private string _login = "";
        private bool _isAuthenticated;
        private bool _isAuthorizedPayerFeeBatchCreate;
        private StringCollection _roles;

 
        public User(int key, string login)
        {
            _key = key;
            _login = login;
        }

        public User()
        {
            GetCurrentWindowsUser();
        }

 
        public void IdentifyWindowsUser()
        {
            if (IsWindowsUserIdentified())
            {
                _login = _currentWindowsUser.Name;
                _key = 0;
                _isAuthenticated = _currentWindowsUser.IsAuthenticated;
            }
            else
            {
                GetCurrentWindowsUser();
                if (!IsWindowsUserIdentified())
                {
                    throw new UserInformationInvalidException("Windows is returning a null user");
                }
            }
        }

 

        public bool AuthorizeAction(StringCollection allowedRoles)
        {
            WindowsPrincipal currentPrincipal = new WindowsPrincipal(_currentWindowsUser);

            bool result = false;
            foreach (string allowedRole in allowedRoles)
            {
                if (currentPrincipal.IsInRole(allowedRole))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }


        private bool IsWindowsUserIdentified()
        {
            bool result;
            if (_currentWindowsUser != null)
                result = true;
            else
            {
                result = false;
            }
            return result;
        }

        private void GetCurrentWindowsUser()
        {
            _currentWindowsUser = WindowsIdentity.GetCurrent();
        }

 
        public int Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
            set { _isAuthenticated = value; }
        }

        public bool IsAuthorizedPayerFeeBatchCreate
        {
            get { return _isAuthorizedPayerFeeBatchCreate; }
            set { _isAuthorizedPayerFeeBatchCreate = value; }
        }

        public StringCollection Roles
        {
            get { return _roles; }
            set { _roles = value; }
        }




    }
}
