using System;
using System.Collections.Generic;
using System.Text;

namespace Fire_SMS_Console.App_Code
{
    class PhoneUser
    {
        public int ID { get; set; }
        public string email { get; set; }
        public string phonenumber { get; set; }
        public int regionid { get; set; }
        public string regionname { get; set; }
        public DateTime lastsmssent { get; set; }
    }
}
