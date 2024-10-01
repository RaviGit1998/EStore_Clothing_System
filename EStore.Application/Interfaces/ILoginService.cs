﻿using EStore.Domain.Entities;
using EStore.Domain.EntityDtos;
using EStore.Domain.EntityDtos.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Application.Interfaces
{
    public interface ILoginService
    {
        Task<User> AuthenticateUser(LoginReq loginDetails);
        Task<LoginRes> ProvideToken(LoginReq login);
        string GeneratePasswordResetToken(User user);
    }
}
