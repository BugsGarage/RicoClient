using RicoClient.Configs;
using RicoClient.Scripts.Network;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UniRx.Async;
using UnityEngine;

namespace RicoClient.Scripts.Cards
{
    public class CardsManager : IDisposable
    {
        private readonly NetworkManager _network;

        private readonly SQLiteConnection _db;

        public CardsManager(NetworkManager network, CardsConfig config)
        {
            _network = network;

            var databasePath = Path.Combine(Application.dataPath, config.CardLocalDBPath);
            _db = new SQLiteConnection(databasePath);
            DBSetup();
        }

        /// <summary>
        /// Updating local cards database from the cards server
        /// </summary>
        public async UniTask UpdateLocalCardsDB()
        {
            List<Card> cards = await _network.GetAllCards();
            await UniTask.Run(() => _db.InsertAll(cards));
        }

        public async UniTask<int> CardsCount()
        {
            return await UniTask.Run(() => _db.Table<Card>().Count());
        }

        public async UniTask<Card[]> GetCardsRange(int startId, int count)
        {
            int endId = startId + count;
            return await UniTask.Run(() => _db.Table<Card>().Where(c => c.CardId >= startId && c.CardId < endId).ToArray());
        }

        private async void DBSetup()
        {
            await UniTask.Run(() => _db.CreateTable<Card>());
        }

        #region IDisposable Support

        private bool _disposed = false; 

        protected virtual async void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    await UniTask.Run(() => _db.DropTable<Card>());
                    _db.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
