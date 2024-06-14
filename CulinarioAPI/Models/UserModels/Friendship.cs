using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinarioAPI.Models.UserModels
{
    public class Friendship
    {
        [Key]
        public int FriendshipId { get; set; }

        [ForeignKey("UserProfile")]
        public string Username { get; set; }

        [ForeignKey("FriendUserProfile")]
        public string FriendUsername { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual UserProfile FriendUserProfile { get; set; }
    }
}
