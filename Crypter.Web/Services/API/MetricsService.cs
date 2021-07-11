﻿using Crypter.Contracts.Responses;
using Crypter.Web.Models;
using System.Net;
using System.Threading.Tasks;

namespace Crypter.Web.Services.API
{
   public interface IMetricsService
   {
      Task<(HttpStatusCode HttpStatus, DiskMetricsResponse Response)> GetDiskMetricsAsync();
   }

   public class MetricsService : IMetricsService
   {
      private readonly string BaseMetricsUrl;
      private readonly IHttpService HttpService;

      public MetricsService(AppSettings appSettings, IHttpService httpService)
      {
         BaseMetricsUrl = $"{appSettings.ApiBaseUrl}/metrics";
         HttpService = httpService;
      }

      public async Task<(HttpStatusCode, DiskMetricsResponse)> GetDiskMetricsAsync()
      {
         var url = $"{BaseMetricsUrl}/disk";
         return await HttpService.Get<DiskMetricsResponse>(url);
      }
   }
}