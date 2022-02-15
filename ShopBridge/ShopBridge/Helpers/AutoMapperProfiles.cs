using AutoMapper;
using ShopBridge.DTOs;
using ShopBridge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<InventoryDto, Inventory>().ReverseMap();
        }
    }
}
