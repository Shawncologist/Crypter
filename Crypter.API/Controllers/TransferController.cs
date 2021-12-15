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

using Crypter.API.Controllers.Methods;
using Crypter.API.Services;
using Crypter.Contracts.Requests;
using Crypter.Core.Interfaces;
using Crypter.Core.Models;
using Crypter.CryptoLib.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Crypter.API.Controllers
{
   [Route("api/transfer")]
   public class TransferController : ControllerBase
   {
      private readonly UploadService UploadService;
      private readonly DownloadService DownloadService;

      public TransferController(IConfiguration configuration,
          IBaseTransferService<MessageTransfer> messageService,
          IBaseTransferService<FileTransfer> fileService,
          IUserService userService,
          IUserProfileService userProfileService,
          IEmailService emailService,
          IApiValidationService apiValidationService,
          ISimpleEncryptionService simpleEncryptionService,
          ISimpleHashService simpleHashService
         )
      {
         UploadService = new UploadService(configuration, messageService, fileService, emailService, apiValidationService, simpleEncryptionService, simpleHashService);
         DownloadService = new DownloadService(configuration, messageService, fileService, userService, userProfileService, simpleEncryptionService, simpleHashService);
      }

      [HttpPost("message")]
      public async Task<IActionResult> MessageTransferAsync([FromBody] MessageTransferRequest request, CancellationToken cancellationToken)
      {
         var senderId = ClaimsParser.ParseUserId(User);
         return await UploadService.ReceiveMessageTransferAsync(request, senderId, Guid.Empty, cancellationToken);
      }

      [HttpPost("message/{recipientId}")]
      public async Task<IActionResult> UserMessageTransferAsync([FromBody] MessageTransferRequest request, Guid recipientId, CancellationToken cancellationToken)
      {
         var senderId = ClaimsParser.ParseUserId(User);
         return await UploadService.ReceiveMessageTransferAsync(request, senderId, recipientId, cancellationToken);
      }

      [HttpPost("file")]
      public async Task<IActionResult> FileTransferAsync([FromBody] FileTransferRequest request, CancellationToken cancellationToken)
      {
         var senderId = ClaimsParser.ParseUserId(User);
         return await UploadService.ReceiveFileTransferAsync(request, senderId, Guid.Empty, cancellationToken);
      }

      [HttpPost("file/{recipientId}")]
      public async Task<IActionResult> UserFileTransferAsync([FromBody] FileTransferRequest request, Guid recipientId, CancellationToken cancellationToken)
      {
         var senderId = ClaimsParser.ParseUserId(User);
         return await UploadService.ReceiveFileTransferAsync(request, senderId, recipientId, cancellationToken);
      }

      [HttpPost("message/preview")]
      public async Task<IActionResult> GetMessagePreviewAsync([FromBody] GetTransferPreviewRequest request, CancellationToken cancellationToken)
      {
         var requestorId = ClaimsParser.ParseUserId(User);
         return await DownloadService.GetMessagePreviewAsync(request, requestorId, cancellationToken);
      }

      [HttpPost("file/preview")]
      public async Task<IActionResult> GetFilePreviewAsync([FromBody] GetTransferPreviewRequest request, CancellationToken cancellationToken)
      {
         var requestorId = ClaimsParser.ParseUserId(User);
         return await DownloadService.GetFilePreviewAsync(request, requestorId, cancellationToken);
      }

      [HttpPost("message/ciphertext")]
      public async Task<IActionResult> GetMessageCiphertextAsync([FromBody] GetTransferCiphertextRequest request, CancellationToken cancellationToken)
      {
         var requestorId = ClaimsParser.ParseUserId(User);
         return await DownloadService.GetMessageCiphertextAsync(request, requestorId, cancellationToken);
      }

      [HttpPost("file/ciphertext")]
      public async Task<IActionResult> GetFileCiphertext([FromBody] GetTransferCiphertextRequest request, CancellationToken cancellationToken)
      {
         var requestorId = ClaimsParser.ParseUserId(User);
         return await DownloadService.GetFileCiphertextAsync(request, requestorId, cancellationToken);
      }

      [HttpPost("message/signature")]
      public async Task<IActionResult> GetMessageSignatureAsync([FromBody] GetTransferSignatureRequest request, CancellationToken cancellationToken)
      {
         var requestorId = ClaimsParser.ParseUserId(User);
         return await DownloadService.GetMessageSignatureAsync(request, requestorId, cancellationToken);
      }

      [HttpPost("file/signature")]
      public async Task<IActionResult> GetFileSignatureAsync([FromBody] GetTransferSignatureRequest request, CancellationToken cancellationToken)
      {
         var requestorId = ClaimsParser.ParseUserId(User);
         return await DownloadService.GetFileSignatureAsync(request, requestorId, cancellationToken);
      }
   }
}