using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace PATHEdit
{

    class PathHolder
    {
        private readonly PathType _PathType;

        public IList<String> Values { get; private set; }

        public enum PathType
        {
            User, System
        }

        public PathHolder(PathType pathType)
        {
            this._PathType = pathType;
            Reload();
        }

        public void Reload()
        {
            var target = (this._PathType == PathType.System) ? EnvironmentVariableTarget.Machine : EnvironmentVariableTarget.User;
            var path = Environment.GetEnvironmentVariable("PATH", target);

            var values = path.Split(';');
            this.Values = new List<String>(values);
        }

        public void Save()
        {
            var path = String.Join(";", this.Values);
            if (this._PathType == PathType.User)
            {
                Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.User);
            }
            else
            {
                EnvironmentPermission permissions = new EnvironmentPermission(EnvironmentPermissionAccess.Write, "PATH");
                permissions.Demand();
                Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Machine);

                //SetEnvironmentVariable("PATH", path);
            }
        }


        // Import the kernel32 dll.
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        // The declaration is similar to the SDK function
        private static extern bool SetEnvironmentVariable(string lpName, string lpValue);

    }
}
