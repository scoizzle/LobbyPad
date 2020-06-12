using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LobbyPad.Models;

using LobbyPad.Services;


namespace LobbyPad.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        private readonly TwilioService _twilio;
        private readonly LobbyPadService _lobbyPad;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, TwilioService twilio, LobbyPadService lobbyPad)
        {
            _logger = logger;
            _twilio = twilio;
            _lobbyPad = lobbyPad;
        }

        public IActionResult Index()
        {
            ViewBag.LobbyEntries = _lobbyPad.GetEntries().OrderBy(entry => entry.CreationTime);

            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register(string guid, string name, string phoneNumber, int partySize, string specialRequests)
        {
            if (Guid.TryParse(guid, out var id) && !string.IsNullOrWhiteSpace(name) && name.Length != 0)
            {
                var entry = new LobbyEntry {
                    Id = id,
                    Name = name,
                    PhoneNumber = phoneNumber,
                    PartySize = partySize,
                    SpecialRequests = specialRequests?.Trim() ?? string.Empty,
                    CreationTime = DateTime.UtcNow,
                    Status = LobbyEntryStatus.Waiting
                };

                _lobbyPad.TryRegister(entry);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("Edit")]
        public IActionResult Edit(string guid)
        {
            if (Guid.TryParse(guid, out var id)) {
                if (_lobbyPad.TryGetById(id, out var entry)) {
                    ViewBag.Entry = entry;

                    return View();
                }
            }

            return RedirectToAction("Index");
        }
        
        [HttpPost("Edit")]
        public IActionResult Edit(string guid, string name, string phoneNumber, int partySize, string specialRequests) 
        {
            if (Guid.TryParse(guid, out var id)) {
                if (_lobbyPad.TryGetById(id, out var entry)) {
                    entry.Name = name;
                    entry.PhoneNumber = phoneNumber;
                    entry.PartySize = partySize;
                    entry.SpecialRequests = specialRequests;

                    _lobbyPad.TryUpdate(entry);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost("SendMessage")]
        public IActionResult SendMessage(string guid) 
        {
            if (Guid.TryParse(guid, out var id)) {
                if (_lobbyPad.TryGetById(id, out var entry)) {
                    _twilio.SendMessage(entry.PhoneNumber, "Your table is ready at Village Inn!", out _);

                    entry.Status = LobbyEntryStatus.Messaged;
                    entry.MessageSentTime = DateTime.Now;

                    _lobbyPad.TryUpdate(entry);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost("Complete")]
        public IActionResult Complete(string guid) 
        {
            if (Guid.TryParse(guid, out var id)) {
                _lobbyPad.TryComplete(id);
            }

            return RedirectToAction("Index");
        }

        [HttpPost("Cancel")]
        public IActionResult Cancel(string guid) 
        {
            if (Guid.TryParse(guid, out var id)) {
                _lobbyPad.TryComplete(id);
            }

            return RedirectToAction("Index");
        }
    }
}
