﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrgHome.Web.Areas.Admin.Models
{
    public class RoleViewModel
    {
        public int Row { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public int UsersCount { get; set; }

    }
}
