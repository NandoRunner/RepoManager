using SourceManager.Desktop.Business.Implementattions;
using SourceManager.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceManager.Desktop.Controllers
{
    public class TrelloController
    {
        private TrelloBoardImpl _board;

        public TrelloController()
        {
            _board = new TrelloBoardImpl();
        }


        public async Task<List<TrelloBoard>> GetBoards()
        {
            var ret = await _board.FindAll().ConfigureAwait(true);

            return ret;
        }


    }
}
