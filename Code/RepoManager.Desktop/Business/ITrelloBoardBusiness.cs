using SourceManager.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceManager.Desktop.Business
{
    public interface ITrelloBoardBusiness
    {
        TrelloBoard Create(TrelloBoard item);
        TrelloBoard FindById(long id);
        List<TrelloBoard> FindByName(string name);
        TrelloBoard FindByExactName(string name);
        Task<List<TrelloBoard>> FindAll();
    }
}
