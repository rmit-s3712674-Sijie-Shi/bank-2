﻿using System;
using System.ComponentModel.DataAnnotations;

namespace assignment2.Models
{
    public class Login
    {
        public String LoginID { get; set; }
        public String PasswordHashed { get; set; }
        public String CustomerID { get; set; }

    }
}
