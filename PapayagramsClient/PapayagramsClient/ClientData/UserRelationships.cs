using PapayagramsClient.PapayagramsService;
using System.Collections.Generic;

namespace PapayagramsClient.ClientData
{
    public static class UserRelationships
    {
        public static Dictionary<string, int> FriendsList { get; set; } = new Dictionary<string, int>();
        public static Dictionary<string, int> BloquedUsersList { get; set; } = new Dictionary<string, int>();
        public static Dictionary<string, int> FriendRequestsList { get; set; } = new Dictionary<string, int>();

        public static void FillLists(FriendDC[] relationships)
        {
            FriendRequestsList.Clear();
            FriendsList.Clear();
            BloquedUsersList.Clear();

            foreach (FriendDC user in relationships)
            {
                switch (user.RelationState)
                {
                    case RelationStateDC.Friend:
                        FriendsList.Add(user.Username, 1);
                        break;

                    case RelationStateDC.Pending:
                        FriendRequestsList.Add(user.Username, 1);
                        break;

                    case RelationStateDC.Blocked:
                        BloquedUsersList.Add(user.Username, 1);
                        break;
                }
            }
        }
    }
}
