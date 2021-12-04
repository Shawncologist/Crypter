﻿/*
 * Copyright (C) 2021 Crypter File Transfer
 * 
 * This file is part of the Crypter file transfer project.
 * 
 * Crypter is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * The Crypter source code is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 * 
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * You can be released from the requirements of the aforementioned license
 * by purchasing a commercial license. Buying such a license is mandatory
 * as soon as you develop commercial activities involving the Crypter source
 * code without disclosing the source code of your own applications.
 * 
 * Contact the current copyright holder to discuss commerical license options.
 */

using Crypter.Contracts.Responses;
using Crypter.Core.Interfaces;
using Crypter.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Crypter.API.Controllers
{
   [Route("api/metrics")]
   public class MetricsController : ControllerBase
   {
      private readonly long AllocatedDiskSpace;
      private readonly IBaseTransferService<MessageTransfer> _messageService;
      private readonly IBaseTransferService<FileTransfer> _fileService;

      public MetricsController(IConfiguration configuration,
          IBaseTransferService<MessageTransfer> messageService,
          IBaseTransferService<FileTransfer> fileService
          )
      {
         AllocatedDiskSpace = long.Parse(configuration["EncryptedFileStore:AllocatedGB"]) * 1024 * 1024 * 1024;
         _messageService = messageService;
         _fileService = fileService;
      }

      [HttpGet("disk")]
      public async Task<IActionResult> GetDiskMetrics()
      {
         var sizeOfFileUploads = await _fileService.GetAggregateSizeAsync();
         var sizeOfMessageUploads = await _messageService.GetAggregateSizeAsync();
         var totalSizeOfUploads = sizeOfFileUploads + sizeOfMessageUploads;
         var isFull = totalSizeOfUploads + (10 * 1024 * 1024) >= AllocatedDiskSpace;

         var responseBody = new DiskMetricsResponse(isFull, AllocatedDiskSpace.ToString(), (AllocatedDiskSpace - totalSizeOfUploads).ToString());
         return new OkObjectResult(responseBody);
      }
   }
}
