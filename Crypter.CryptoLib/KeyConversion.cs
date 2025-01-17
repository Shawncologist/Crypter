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

using Crypter.Common.Primitives;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System.IO;

namespace Crypter.CryptoLib
{
   public static class KeyConversion
   {
      public static PEMString ConvertToPEM(this AsymmetricKeyParameter keyParams)
      {
         var stringWriter = new StringWriter();
         var pemWriter = new PemWriter(stringWriter);
         pemWriter.WriteObject(keyParams);
         pemWriter.Writer.Flush();
         return PEMString.From(stringWriter.ToString());
      }

      public static AsymmetricCipherKeyPair ConvertRSAPrivateKeyFromPEM(PEMString pemKey)
      {
         var stringReader = new StringReader(pemKey.Value);
         var pemReader = new PemReader(stringReader);
         return (AsymmetricCipherKeyPair)pemReader.ReadObject();
      }

      public static AsymmetricKeyParameter ConvertRSAPublicKeyFromPEM(PEMString pemKey)
      {
         var stringReader = new StringReader(pemKey.Value);
         var pemReader = new PemReader(stringReader);
         return (AsymmetricKeyParameter)pemReader.ReadObject();
      }

      public static X25519PrivateKeyParameters ConvertX25519PrivateKeyFromPEM(PEMString pemKey)
      {
         var stringReader = new StringReader(pemKey.Value);
         var pemReader = new PemReader(stringReader);
         return (X25519PrivateKeyParameters)pemReader.ReadObject();
      }

      public static X25519PublicKeyParameters ConvertX25519PublicKeyFromPEM(PEMString pemKey)
      {
         var stringReader = new StringReader(pemKey.Value);
         var pemReader = new PemReader(stringReader);
         return (X25519PublicKeyParameters)pemReader.ReadObject();
      }

      public static Ed25519PrivateKeyParameters ConvertEd25519PrivateKeyFromPEM(PEMString pemKey)
      {
         var stringReader = new StringReader(pemKey.Value);
         var pemReader = new PemReader(stringReader);
         return (Ed25519PrivateKeyParameters)pemReader.ReadObject();
      }

      public static Ed25519PublicKeyParameters ConvertEd25519PublicKeyFromPEM(PEMString pemKey)
      {
         var stringReader = new StringReader(pemKey.Value);
         var pemReader = new PemReader(stringReader);
         return (Ed25519PublicKeyParameters)pemReader.ReadObject();
      }
   }
}
