﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portfolio_Eastelson.Models
{
    public class ContactViewModel
    {
        public Employee Employee { get; set; }
        public List<Mission> Missions { get; set; }
    }
}