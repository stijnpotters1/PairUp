global using PairUpApi.Configuration.Role;
global using PairUpApi.Configuration.Services;
global using PairUpApi.Configuration;
global using PairUpApi.Service;
global using PairUpApi.Configuration.Seeder;

global using PairUpInfrastructure.Data;
global using PairUpInfrastructure.Repositories;

global using PairUpCore.Interfaces;
global using PairUpCore.Models;
global using PairUpCore.DTO;
global using PairUpCore.DTO.Requests;
global using PairUpCore.DTO.Responses;
global using PairUpCore.Exceptions.Authentication;

global using PairUpScraper;
global using PairUpScraper.Scrapers.BijzonderPlekjeScraper;

global using PairUpShared.Middleware;

global using System.Text;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.OpenApi.Models;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;

global using AutoMapper;
global using DotNetEnv;
global using AngleSharp.Text;