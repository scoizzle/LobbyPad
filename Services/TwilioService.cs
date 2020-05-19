using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LobbyPad.Models;

using Twilio;
using Twilio.Rest.Api.V2010.Account;


namespace LobbyPad.Services
{
    public class TwilioService {
        private readonly string twilioPhoneNumber;

        public TwilioService(string accountSid, string authToken, string phoneNumber) {
            TwilioClient.Init(accountSid, authToken);
            twilioPhoneNumber = phoneNumber;
        }

        public bool SendMessage(string phoneNumber, string message, out MessageResource result) {
            try {
            #if RELEASE
                result = MessageResource.Create(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(twilioPhoneNumber),
                    to: new Twilio.Types.PhoneNumber("+1" + phoneNumber)
                );
            #else
                result = default;
            #endif
                return true;
            }
            catch { }

            result = default;
            return false;
        }
    }
}