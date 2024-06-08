using System.ComponentModel.DataAnnotations;

namespace CulinarioAPI.Models
{
    public class Friendship
    {
        [Key]
        public int FriendshipId { get; set; }

        public int UserId { get; set; }
        public int FriendId { get; set; }
        public string Status { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual UserProfile FriendUserProfile { get; set; }
    }
}
