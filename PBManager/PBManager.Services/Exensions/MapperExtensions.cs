﻿using AutoMapper;

namespace PBManager.Services.Exensions
{
    public static class MapperExtensions
    {
        private static void IgnoreUnmappedProperties(TypeMap map, IMappingExpression expr)
        {
            foreach (var propName in map.GetUnmappedPropertyNames())
            {
                if (map.SourceType.GetProperty(propName) != null) expr.ForSourceMember(propName, opt => opt.Ignore());
                if (map.DestinationType.GetProperty(propName) != null) expr.ForMember(propName, opt => opt.Ignore());
            }
        }

        public static void IgnoreUnmapped(this IProfileExpression profile)
        {
            profile.ForAllMaps(IgnoreUnmappedProperties);
        }
    }
}