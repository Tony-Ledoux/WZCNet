using System;
using WZCNet.src.Application.DTOs;
using WZCNet.src.Domain.Entities;

namespace WZCNet.src.Application.Interfaces;

public interface ITokenService
{
    Task<string> CreateBearerToken(TokenClaimsDTO user);
}
