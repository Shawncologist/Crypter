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

using Crypter.API.Attributes;
using Crypter.Common.Monads;
using Crypter.Contracts.Common;
using Crypter.Contracts.Features.Transfer;
using Crypter.Contracts.Features.Users;
using Crypter.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Crypter.API.Controllers
{
   [ApiController]
   [Route("api/user")]
   public class UserController : CrypterController
   {
      private readonly ITransferUploadService _transferUploadService;
      private readonly ITokenService _tokenService;
      private readonly IUserService _userService;

      public UserController(ITransferUploadService transferUploadService, ITokenService tokenService, IUserService userService)
      {
         _transferUploadService = transferUploadService;
         _tokenService = tokenService;
         _userService = userService;
      }

      [HttpGet("{username}/profile")]
      [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserProfileResponse))]
      [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
      [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
      public async Task<IActionResult> GetUserProfileAsync(string username, CancellationToken cancellationToken)
      {
         IActionResult MakeErrorResponse(GetUserProfileError error)
         {
            var errorResponse = new ErrorResponse(error);
#pragma warning disable CS8524
            return error switch
            {
               GetUserProfileError.NotFound => NotFound(errorResponse)
            };
#pragma warning restore CS8524
         }

         var userId = _tokenService.TryParseUserId(User);
         var result = await _userService.GetUserProfileAsync(userId, username, cancellationToken);
         return result.Match(
            () => MakeErrorResponse(GetUserProfileError.NotFound),
            Ok);
      }

      [HttpPost("{username}/file")]
      [MaybeAuthorize]
      [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UploadTransferResponse))]
      [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
      public async Task<IActionResult> UploadUserFileTransferAsync([FromBody] UploadFileTransferRequest request, string username, CancellationToken cancellationToken)
      {
         var uploadResult = await _tokenService.TryParseUserId(User)
            .MatchAsync(
            async () => await _transferUploadService.UploadUserFileAsync(Maybe<Guid>.None, username, request, cancellationToken),
            async x => await _transferUploadService.UploadUserFileAsync(x, username, request, cancellationToken));

         return uploadResult.Match(
            MakeErrorResponse,
            Ok,
            MakeErrorResponse(UploadTransferError.UnknownError));
      }

      [HttpPost("{username}/message")]
      [MaybeAuthorize]
      [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UploadTransferResponse))]
      [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
      public async Task<IActionResult> UploadUserMessageTransferAsync([FromBody] UploadMessageTransferRequest request, string username, CancellationToken cancellationToken)
      {
         var uploadResult = await _tokenService.TryParseUserId(User)
            .MatchAsync(
            async () => await _transferUploadService.UploadUserMessageAsync(Maybe<Guid>.None, username, request, cancellationToken),
            async x => await _transferUploadService.UploadUserMessageAsync(x, username, request, cancellationToken));

         return uploadResult.Match(
            MakeErrorResponse,
            Ok,
            MakeErrorResponse(UploadTransferError.UnknownError));
      }
   }
}
