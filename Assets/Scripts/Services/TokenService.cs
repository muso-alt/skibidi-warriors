using System.Collections.Generic;
using System.Threading;

namespace Skibidi.Services
{
    public class TokenService
    {
        public readonly Dictionary<int, CancellationTokenSource> TokensByEntity 
            = new Dictionary<int, CancellationTokenSource>();

        public void DisposeByEntity(int entity)
        {
            if (TokensByEntity[entity] == null)
            {
                return;
            }
            
            TokensByEntity[entity].Cancel();
            TokensByEntity[entity].Dispose();
            TokensByEntity[entity] = null;
        }
    }
}