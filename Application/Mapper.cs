﻿using Application.UseCases.Accounts.Dtos;
using Application.UseCases.Companies.Dtos;
using Application.UseCases.Events.Dtos;
using AutoMapper;
using Domain;
using Infrastructure.Ef.DbEntities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Service.UseCases.Has.Dtos;

namespace Application;

public static class Mapper
{
    private static AutoMapper.Mapper _instance;

    public static AutoMapper.Mapper GetInstance()
    {
        return _instance ??= CreateMapper();
    }

    private static AutoMapper.Mapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            // Source, Destination
            // Account
            cfg.CreateMap<Account, DtoOutputAccount>();
            cfg.CreateMap<Account, DtoOutputAccountForCompanies>();
            cfg.CreateMap<DbAccount, DtoOutputAccount>();
            cfg.CreateMap<DbAccount, Account>();
            
            // Function
            cfg.CreateMap<Function, DtoOutputFunction>();
            cfg.CreateMap<DbFunction, DtoOutputFunction>();
            cfg.CreateMap<DbFunction, Function>();

            cfg.CreateMap<Companies, DtoOutputCompanies>();
            cfg.CreateMap<DbCompanies, DtoOutputCompanies>();
            cfg.CreateMap<DbCompanies, Companies>();

            cfg.CreateMap<Has, DtoOutputHas>();
            cfg.CreateMap<DbHas, DtoOutputHas>();
            cfg.CreateMap<DbHas, Has>();
            
            cfg.CreateMap<Events, DtoOutputEvents>();
            cfg.CreateMap<DbEvents, DtoOutputEvents>();
            cfg.CreateMap<DbEvents, Events>();
            
            cfg.CreateMap<EventTypes, DtoOutputEventTypes>();
            cfg.CreateMap<DbEventTypes, DtoOutputEventTypes>();
            cfg.CreateMap<DbEventTypes, EventTypes>();
        });
        return new AutoMapper.Mapper(config);
    }
}