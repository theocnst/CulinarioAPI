using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinarioAPI.Models
{
    public class Friend
    {
        [Key]
        public int FriendId { get; set; }

        [ForeignKey("UserProfile")]
        public int UserProfileId { get; set; }

        [ForeignKey("FriendUserProfile")]
        public int FriendUserProfileId { get; set; }

        public string Status { get; set; }  // For example: Pending, Accepted, Rejected

        public virtual UserProfile UserProfile { get; set; }
        public virtual UserProfile FriendUserProfile { get; set; }
    }
}

