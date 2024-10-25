global using AutoMapper;
global using DinkToPdf;
global using DinkToPdf.Contracts;
global using KH.BuildingBlocks.Apis.Enums;
global using KH.BuildingBlocks.Apis.Extentions;
global using KH.BuildingBlocks.Apis.Helpers;
global using KH.BuildingBlocks.Apis.Responses;
global using KH.BuildingBlocks.Auth.User;
global using KH.BuildingBlocks.Excel.Contracts;
global using KH.BuildingBlocks.Excel.Services;
global using KH.BuildingBlocks.Infrastructure.Contracts;
global using KH.BuildingBlocks.Localizatoin.Enum;
global using KH.BuildingBlocks.Settings;
global using KH.Domain.Entities;
global using KH.Dto.Models.AuthenticationDto.Request;
global using KH.Dto.Models.AuthenticationDto.Response;
global using KH.Dto.Models.UserDto.Request;
global using KH.Dto.Models.UserDto.Response;
global using KH.Services.Users.Contracts;
global using KH.Services.Users.Implementation;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.IdentityModel.Tokens;
global using Quartz;
global using Quartz.Simpl;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Linq.Expressions;
global using System.Net;
global using System.Net.Mail;
global using System.Threading.Tasks;
global using KH.Services.Audits.Contracts;
global using KH.Services.Audits.Implementation;
global using KH.Services.Emails.Contracts;
global using KH.Services.Lookups.Departments.Contracts;
global using KH.Services.Lookups.Groups.Contracts;
global using KH.Services.Lookups.Permissions.Contracts;
global using KH.Services.Lookups.Roles.Contracts;
global using KH.Services.Media_s.Contracts;
global using KH.Services.Media_s.Implementation;
global using KH.Services.Sms.Contracts;
global using FluentValidation;
global using FluentValidation.AspNetCore;
global using KH.Dto.Models.UserDto.Validation;
global using KH.Services.BackgroundJobs.QuartzJobs;
global using KH.Services.BackgroundJobs.HangfireJobs.Contracts;
global using KH.Services.BackgroundJobs.HangfireJobs.Implementation;
global using Hangfire;
