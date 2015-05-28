using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TubeStarMetro
{
    public class InternetGraph
    {
        public const int Multiplier = 4;
        public const int TotalUsers = 1000000 / Multiplier;
        public const int TotalSites = 1000 / Multiplier;

        private ConcurrentDictionary<int, InternetUser> _users { get; set; }
        private ConcurrentDictionary<int, InternetSite> _sites { get; set; }
        private ConcurrentDictionary<int, int?> _siteOwners { get; set; }

        public ConcurrentDictionary<int, HashSet<int>> Edges { get; private set; }

        public InternetGraph()
        {
            _users = new ConcurrentDictionary<int, InternetUser>();
            _sites = new ConcurrentDictionary<int, InternetSite>();
            Edges = new ConcurrentDictionary<int, HashSet<int>>();
            _siteOwners = new ConcurrentDictionary<int, int?>();

            //Create empty Websites
            for (int i = 0; i < InternetGraph.TotalSites; i++)
            {
                _siteOwners[RandomUserId] = null;
            }
        }

        public void ClearUsers()
        {
            _users.Clear();
        }

        public void SetUsers(HashSet<int> userIds)
        {
            foreach (var id in userIds)
            {
                if (!_users.ContainsKey(id))
                {
                    _users[id] = CreateUser(id);
                    CreateUserConnections(id);
                }
            }
        }

        private void CreateUserConnections(int id)
        {
            //Create new links
            var numConnectionsPerUser = 10 / InternetGraph.Multiplier + 1;
            for (int i = 0; i < numConnectionsPerUser; i++)
            {
                var userId = RandomUserId;
                if (userId != id)
                    AddEdge(id, userId);
            }
        }

        private int CreateSite(int ownerId)
        {
            InternetSite site = new InternetSite(_sites.Count, GetUser(ownerId));

            //Link subscribers - a site can have up to 5% of the internet subscribe to it
            var subscribers = RandomHelpers.RandomInt((int)(TotalUsers * (Player.Current.ChallengeMode ? 0.02 : 0.05)));
            for (int j = 0; j < subscribers; j++)
            {
                var user = RandomUser;
                if (CategoryHelpers.CheckInterest(user, site.Topic))
                {
                    AddEdge(site.Id, user.Id);
                }
            }

            _sites[site.Id] = site;
            return site.Id;
        }

        public void AddEdge(int start, int end)
        {
            if (!Edges.ContainsKey(start))
                Edges[start] = new HashSet<int>();

            Edges[start].Add(end);
        }

        public int? GetSiteFromOwner(int userId)
        {
            if (_siteOwners.ContainsKey(userId))
            {
                if (_siteOwners[userId] == null)
                {
                    _siteOwners[userId] = CreateSite(userId);
                }
                return _siteOwners[userId];
            }
            return null;
        }

        public InternetUser GetUser(int userId)
        {
            if (!_users.ContainsKey(userId))
            {
                _users[userId] = CreateUser(userId);
            }
            return _users[userId];
        }

        public InternetUser RandomUser
        {
            get
            {
                var randomUserId = RandomUserId;
                if (!_users.ContainsKey(randomUserId))
                {
                    _users[randomUserId] = CreateUser(randomUserId);
                }
                return _users[randomUserId];
            }
        }

        public int RandomUserId
        {
            get { return RandomHelpers.RandomInt(TotalUsers); }
        }

        private InternetUser CreateUser(int id)
        {
            var newUser = new InternetUser(id);
            CreateUserConnections(id);
            return newUser;
        }
    }
}