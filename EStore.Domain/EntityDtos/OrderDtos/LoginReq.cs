﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Domain.EntityDtos.OrderDtos
{
     public class LoginReq
     {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
