using System;
using System.Collections.Generic;
using System.Text;

namespace Contract
{
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string Author = "Author";
        public const string Reader = "Reader";

        public static readonly string[] AllRoles = { Admin, Author, Reader };
        public static readonly string[] SelfRegisterable = { Reader, Author };
    }
}
