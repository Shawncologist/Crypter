﻿/*
 * Copyright (C) 2022 Crypter File Transfer
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
 * Contact the current copyright holder to discuss commercial license options.
 */

using Crypter.Common.Monads;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Crypter.Web.Shared.Modal
{
   public partial class BasicModalBase : ComponentBase
   {
      protected string Subject { get; set; }

      protected string Message { get; set; }

      protected string PrimaryButtonText { get; set; }

      protected string SecondaryButtonText { get; set; }

      protected Maybe<EventCallback<bool>> ModalClosedCallback { get; set; }

      protected bool Show = false;
      protected string ModalDisplay = "none;";
      protected string ModalClass = "";

      public void Open(string subject, string message, string primaryButtonText, Maybe<string> secondaryButtonText, Maybe<EventCallback<bool>> modalClosedCallback)
      {
         Subject = subject;
         Message = message;
         PrimaryButtonText = primaryButtonText;
         SecondaryButtonText = secondaryButtonText.Match(
            () => string.Empty,
            x => x);
         ModalClosedCallback = modalClosedCallback;

         ModalDisplay = "block;";
         ModalClass = "Show";
         Show = true;
         StateHasChanged();
      }

      public async Task CloseAsync(bool modalClosedInTheAffirmative)
      {
         ModalDisplay = "none";
         ModalClass = "";
         Show = false;
         StateHasChanged();

         await ModalClosedCallback.IfSomeAsync(async x => await x.InvokeAsync(modalClosedInTheAffirmative));
      }
   }
}
