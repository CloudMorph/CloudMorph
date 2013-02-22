using System.Collections.Generic;

namespace CloudAbstractions.Security
{
    public class CredentialsRepository : ICredentialsRepository
    {
        readonly Dictionary<string,ICredentials> _credentialsRepository = new Dictionary<string, ICredentials>();

        public ICredentials this[string name]
        {
            get { return Get(name); }
        }

        public ICredentials Get(string name)
        {
            ICredentials credentials;
            if (_credentialsRepository.TryGetValue(name, out credentials))
                return credentials;

            return null;
        }

        public void Add(string name, ICredentials credentials)
        {
            _credentialsRepository.Add(name, credentials);
        }
    }
}