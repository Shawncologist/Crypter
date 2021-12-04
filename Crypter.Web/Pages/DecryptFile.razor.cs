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
using Crypter.Web.Services;
using Crypter.Web.Services.API;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Crypter.Web.Pages
{
   public partial class DecryptFileBase : ComponentBase
   {
      [Inject]
      IJSRuntime JSRuntime { get; set; }

      [Inject]
      ILocalStorageService LocalStorage { get; set; }

      [Inject]
      protected ITransferApiService TransferService { get; set; }

      [Parameter]
      public Guid TransferId { get; set; }

      protected bool Loading;
      protected bool ItemFound;

      protected string FileName;
      protected string ContentType;
      protected int Size;
      protected string Created;
      protected string Expiration;

      protected Guid SenderId;
      protected string SenderUsername;
      protected string SenderAlias;
      protected string X25519PublicKey;

      protected Guid RecipientId;

      protected override async Task OnInitializedAsync()
      {
         Loading = true;
         await JSRuntime.InvokeVoidAsync("Crypter.SetPageTitle", "Crypter - Decrypt");
         await PrepareFilePreviewAsync(); 
         await base.OnInitializedAsync();
         Loading = false;
      }

      protected async Task PrepareFilePreviewAsync()
      {
         var filePreviewRequest = new GetTransferPreviewRequest(TransferId);
         var withAuth = LocalStorage.HasItem(StoredObjectType.UserSession);
         var (httpStatus, response) = await TransferService.DownloadFilePreviewAsync(filePreviewRequest, withAuth);

         ItemFound = httpStatus != HttpStatusCode.NotFound;
         if (ItemFound)
         {
            FileName = response.FileName;
            ContentType = response.ContentType;
            Created = response.CreationUTC.ToLocalTime().ToString();
            Expiration = response.ExpirationUTC.ToLocalTime().ToString();
            Size = response.Size;
            SenderId = response.SenderId;
            SenderUsername = response.SenderUsername;
            SenderAlias = response.SenderAlias;
            RecipientId = response.RecipientId;
            X25519PublicKey = response.X25519PublicKey;
         }
      }
   }
}
