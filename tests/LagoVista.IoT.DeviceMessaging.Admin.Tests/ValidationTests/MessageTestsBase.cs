﻿using LagoVista.IoT.DeviceMessaging.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using LagoVista.Core;
using System.Threading.Tasks;
using LagoVista.Core.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LagoVista.Core.Models;

namespace LagoVista.IoT.DeviceMessaging.Admin.Tests.ValidationTests
{
    public class MessageTestsBase
    {
        protected DeviceMessageDefinition GetValidMessage(MessageContentTypes contenttype)
        {
            var msg = new DeviceMessageDefinition()
            {
                Name = "msg1234",
                MessageId = "msg1234",
                Key = "msg1234",
                MessageDirection = EntityHeader<MessageDirections>.Create(MessageDirections.Incoming),
                Id = Guid.NewGuid().ToId(),
                CreationDate = DateTime.Now.ToJSONString(),
                CreatedBy = new Core.Models.EntityHeader() { Id = Guid.NewGuid().ToId(), Text = "user name" }
            };

            msg.LastUpdatedDate = msg.CreationDate;
            msg.LastUpdatedBy = msg.CreatedBy;

            switch (contenttype)
            {
                case MessageContentTypes.Binary:
                    msg.ContentType = new Core.Models.EntityHeader<MessageContentTypes>() { Id = DeviceMessageDefinition.ContentType_Binary, Text = "Binary" };
                    msg.StringParsingStrategy = new Core.Models.EntityHeader<StringParsingStrategy>() { Id = DeviceMessageDefinition.StringParsingStrategy_NullTerminated, Text = "Null" };
                    msg.Endian = new Core.Models.EntityHeader<EndianTypes>() { Id = DeviceMessageDefinition.Endian_BigEndian, Text = "Little Endian" };
                    msg.BinaryParsingStrategy = new Core.Models.EntityHeader<BinaryParsingStrategy>() { Id = DeviceMessageDefinition.BinaryParsingStrategy_Absolute, Text = "AbsolutePosition" };
                    msg.StringLengthByteCount = 2;

                    break;
/*                case MessageContentTypes.Custom:
                    msg.ContentType = new Core.Models.EntityHeader<MessageContentTypes>() { Id = DeviceMessageDefinition.ContentType_Custom, Text = "Custom" };
                    msg.Script = "function() {do something, for now javascript validate}";
                    break;*/;
                case MessageContentTypes.Delimited:
                    msg.ContentType = new Core.Models.EntityHeader<MessageContentTypes>() { Id = DeviceMessageDefinition.ContentType_Delimited, Text = "Delimited" };
                    msg.Delimiter = ",";
                    msg.QuotedText = false;
                    break;
                case MessageContentTypes.JSON:
                    msg.ContentType = new Core.Models.EntityHeader<MessageContentTypes>()
                    {
                        Id = DeviceMessageDefinition.ContentType_Json,
                        Text = "JSON"
                    };
                    break;
                case MessageContentTypes.StringRegEx:
                    msg.ContentType = new Core.Models.EntityHeader<MessageContentTypes>() { Id = DeviceMessageDefinition.ContentType_StringRegEx, Text = "StringRegEx" };
                    msg.RegEx = "^parse_me_if_i_was_valid_reg_ex$";
                    break;
                case MessageContentTypes.StringPosition:
                    msg.ContentType = new Core.Models.EntityHeader<MessageContentTypes>() { Id = DeviceMessageDefinition.ContentType_StringPosition, Text = "StringPosition" };
                    break;
                case MessageContentTypes.XML:
                    msg.ContentType = new Core.Models.EntityHeader<MessageContentTypes>() { Id = DeviceMessageDefinition.ContentType_Xml, Text = "XML" };
                    break;
            }

            return msg;
        }

        protected void ShowErrors(ValidationResult result)
        {
            if (result.Errors.Any())
            {
                Console.WriteLine("Errors (Expected if test passed)");
                Console.WriteLine("======================================");
                foreach (var err in result.Errors)
                {
                    Console.WriteLine(err.Message);
                }
                Console.Write("");
            }
        }

        protected void ShowWarnings(ValidationResult result)
        {
            if (result.Warnings.Any())
            {
                Console.WriteLine("Warnings (Expected if test passed)");
                Console.WriteLine("======================================");
                foreach (var err in result.Warnings)
                {
                    Console.WriteLine(err.Message);
                }
                Console.Write("");
            }
        }

        protected void AssertValid(ValidationResult result)
        {
            ShowErrors(result);
            ShowWarnings(result);
            Assert.AreEqual(0, result.Warnings.Count);
            Assert.IsTrue(result.Successful);
        }

        protected void AssertInValid(ValidationResult result)
        {
            ShowErrors(result);
            ShowWarnings(result);
            Assert.IsFalse(result.Successful);
        }
    }
}
