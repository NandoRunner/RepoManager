using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceManager.Desktop.Model;
using Manatee.Trello.Rest;
using Manatee.Trello;

namespace SourceManager.Desktop.Business.Implementattions
{
    public class TrelloBoardImpl : ITrelloBoardBusiness
    {
        //private string _memberid = "5b7dfab28663063a70b30e6a";

        private readonly string _appKey = "43b7ac5e55b3a18b697b6dc25d16e9bd";

        private readonly string _userToken = "5dbfeaa0b9e600883dc77d500fe25dd96c749eac6cb13e0181b38d41457e8930";

        public TrelloBoardImpl()
        {
        }

        public TrelloBoard Create(TrelloBoard item)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TrelloBoard>> FindAll()
        {
            var ret = new List<TrelloBoard>();

            try
            {
                var auth = new TrelloAuthorization
                {
                    AppKey = _appKey,
                    UserToken = _userToken
                };

                var factory = new TrelloFactory();
    
                var me = await factory.Me().ConfigureAwait(true);


                foreach (var board in me.Boards)
                {
                    var t = new TrelloBoard
                    {
                        name = board.Name
                    };
                    ret.Add(t);
                }
            }
            catch (TrelloInteractionException e)
            {
                var msg = e.Message;
            }
            return ret;
        }

        public TrelloBoard FindByExactName(string name)
        {
            throw new NotImplementedException();
        }

        public TrelloBoard FindById(long id)
        {
            throw new NotImplementedException();
        }

        public List<TrelloBoard> FindByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
