using System;

namespace LobbyPad.Models
{
    public enum LobbyEntryStatus {
        Waiting,
        Messaged,
        Completed,
        Cancelled
    }

    public class LobbyEntry {
        public Guid Id { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime MessageSentTime { get; set; }
        
        public LobbyEntryStatus Status { get; set; }

        public string Name { get; set; }

        public int PartySize { get; set; }

        public string SpecialRequests { get; set; }

        public string PhoneNumber { get; set; }
    }
}