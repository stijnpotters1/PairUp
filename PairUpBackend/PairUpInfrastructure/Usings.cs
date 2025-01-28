global using PairUpCore.Models;
global using PairUpCore.Interfaces;
global using PairUpCore.DTO.Requests;
global using PairUpCore.DTO.Responses;
global using PairUpCore.Exceptions.Authentication;
global using PairUpCore.Exceptions.Role;
global using PairUpCore.Exceptions.User;

global using PairUpInfrastructure.Data;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;

global using System.Security.Claims;
global using System.IdentityModel.Tokens.Jwt;
global using Microsoft.IdentityModel.Tokens;
global using System.Text;