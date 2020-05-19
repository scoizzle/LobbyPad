using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LobbyPad.Models;

using LiteDB;

namespace LobbyPad.Services
{
    public class LobbyPadService {
        LiteDatabase database;
        ILiteCollection<LobbyEntry> entries;

        public LobbyPadService(string dbFilePath) {
            database = new LiteDatabase(dbFilePath);
            entries = database.GetCollection<LobbyEntry>("entries");

            entries.EnsureIndex(entry => entry.PhoneNumber);
        }
        
        public IEnumerable<LobbyEntry> GetEntries() {
            return entries.Query().ToEnumerable();
        }

        public bool TryRegister(LobbyEntry entry) {
            try {
                entries.Insert(new BsonValue(entry.Id), entry);
                return true;
            }
            catch {
                return false;
            }
        }

        public bool TryUpdate(LobbyEntry entry) {
            try {
                return entries.Update(entry);
            }
            catch {
                return false;
            }
        }

        public bool TryComplete(Guid id) {
            try {
                var bsonId = new BsonValue(id);
                return entries.Delete(bsonId);
            }
            catch { }
            return false;
        }

        public bool TryGetById(Guid id, out LobbyEntry entry) {
            try {
                entry = entries.FindById(new BsonValue(id));
                return true;
            }
            catch {
                entry = default;
                return false;
            }
        }
    }
}