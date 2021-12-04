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

using Crypter.Contracts.Requests;
using Crypter.Contracts.Responses;
using Crypter.Web.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Crypter.Web.Services.API
{
   public interface ITransferApiService
   {
      Task<(HttpStatusCode HttpStatus, TransferUploadResponse Response)> UploadMessageTransferAsync(MessageTransferRequest uploadRequest, Guid recipient, bool withAuthentication);
      Task<(HttpStatusCode HttpStatus, TransferUploadResponse Response)> UploadFileTransferAsync(FileTransferRequest uploadRequest, Guid recipient, bool withAuthentication);
      Task<(HttpStatusCode HttpStatus, MessagePreviewResponse Response)> DownloadMessagePreviewAsync(GetTransferPreviewRequest downloadRequest, bool withAuthentication);
      Task<(HttpStatusCode HttpStatus, GetTransferSignatureResponse Response)> DownloadMessageSignatureAsync(GetTransferSignatureRequest downloadRequest, bool withAuthentication);
      Task<(HttpStatusCode HttpStatus, GetTransferCiphertextResponse Response)> DownloadMessageCiphertextAsync(GetTransferCiphertextRequest downloadRequest, bool withAuthentication);
      Task<(HttpStatusCode HttpStatus, FilePreviewResponse Response)> DownloadFilePreviewAsync(GetTransferPreviewRequest downloadRequest, bool withAuthentication);
      Task<(HttpStatusCode HttpStatus, GetTransferSignatureResponse Response)> DownloadFileSignatureAsync(GetTransferSignatureRequest downloadRequest, bool withAuthentication);
      Task<(HttpStatusCode HttpStatus, GetTransferCiphertextResponse Response)> DownloadFileCiphertextAsync(GetTransferCiphertextRequest downloadRequest, bool withAuthentication);
   }

   public class TransferApiService : ITransferApiService
   {
      private readonly string BaseTransferUrl;
      private readonly IHttpService HttpService;

      public TransferApiService(AppSettings appSettings, IHttpService httpService)
      {
         BaseTransferUrl = $"{appSettings.ApiBaseUrl}/transfer";
         HttpService = httpService;
      }

      public async Task<(HttpStatusCode, TransferUploadResponse)> UploadMessageTransferAsync(MessageTransferRequest uploadRequest, Guid recipient, bool withAuthentication)
      {
         var url = recipient == Guid.Empty
            ? $"{BaseTransferUrl}/message"
            : $"{BaseTransferUrl}/message/{recipient}";
         return await HttpService.Post<TransferUploadResponse>(url, uploadRequest, withAuthentication);
      }

      public async Task<(HttpStatusCode, TransferUploadResponse)> UploadFileTransferAsync(FileTransferRequest uploadRequest, Guid recipient, bool withAuthentication)
      {
         var url = recipient == Guid.Empty
            ? $"{BaseTransferUrl}/file"
            : $"{BaseTransferUrl}/file/{recipient}";
         return await HttpService.Post<TransferUploadResponse>(url, uploadRequest, withAuthentication);
      }

      public async Task<(HttpStatusCode, MessagePreviewResponse)> DownloadMessagePreviewAsync(GetTransferPreviewRequest downloadRequest, bool withAuthentication)
      {
         var url = $"{BaseTransferUrl}/message/preview";
         return await HttpService.Post<MessagePreviewResponse>(url, downloadRequest, withAuthentication);
      }

      public async Task<(HttpStatusCode, GetTransferSignatureResponse)> DownloadMessageSignatureAsync(GetTransferSignatureRequest downloadRequest, bool withAuthentication)
      {
         var url = $"{BaseTransferUrl}/message/signature";
         return await HttpService.Post<GetTransferSignatureResponse>(url, downloadRequest, withAuthentication);
      }

      public async Task<(HttpStatusCode, GetTransferCiphertextResponse)> DownloadMessageCiphertextAsync(GetTransferCiphertextRequest downloadRequest, bool withAuthentication)
      {
         var url = $"{BaseTransferUrl}/message/ciphertext";
         return await HttpService.Post<GetTransferCiphertextResponse>(url, downloadRequest, withAuthentication);
      }

      public async Task<(HttpStatusCode, FilePreviewResponse)> DownloadFilePreviewAsync(GetTransferPreviewRequest downloadRequest, bool withAuthentication)
      {
         var url = $"{BaseTransferUrl}/file/preview";
         return await HttpService.Post<FilePreviewResponse>(url, downloadRequest, withAuthentication);
      }

      public async Task<(HttpStatusCode, GetTransferSignatureResponse)> DownloadFileSignatureAsync(GetTransferSignatureRequest downloadRequest, bool withAuthentication)
      {
         var url = $"{BaseTransferUrl}/file/signature";
         return await HttpService.Post<GetTransferSignatureResponse>(url, downloadRequest, withAuthentication);
      }

      public async Task<(HttpStatusCode, GetTransferCiphertextResponse)> DownloadFileCiphertextAsync(GetTransferCiphertextRequest downloadRequest, bool withAuthentication)
      {
         var url = $"{BaseTransferUrl}/file/ciphertext";
         return await HttpService.Post<GetTransferCiphertextResponse>(url, downloadRequest, withAuthentication);
      }
   }
}
