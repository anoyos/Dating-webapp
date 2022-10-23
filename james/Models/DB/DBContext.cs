using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Models.DB
{
    public class DBContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<DDL> ddls { get; set; }

        public DbSet<UserLookingRelation> userLookingRelations { get; set; }
        public DbSet<UserPersonality> userPersonalities { get; set; }
        public DbSet<UserQuality> userQualities { get; set; }
        public DbSet<UserHobby> userHobbies { get; set; }
        public DbSet<Match> matches { get; set; }
        public DbSet<Album> albums { get; set; }
        public DbSet<AlbumImage> albumImages { get; set; }
        public DbSet<HiddenAlbum> hiddenAlbums { get; set; }
        public DbSet<UserRating> userRatings { get; set; }
        public DbSet<Diary> diaries { get; set; }
        public DbSet<ReportUser> reportUsers { get; set; }
        public DbSet<VerificationCode> verificationCodes { get; set; }
        public DbSet<FirebaseToken> firebaseTokens { get; set; }
        public DbSet<ChatThread> chatThreads { get; set; }
        public DbSet<Chat> chats { get; set; }
        public DbSet<Event> events { get; set; }
        public DbSet<CallLog> callLogs { get; set; }
        public DbSet<AppSetting> appSettings { get; set; }
        public DbSet<DiscountCoupon> discountCoupons { get; set; }
        public DbSet<Story> stories { get; set; }
        public DbSet<StoryPhoto> storyPhotos { get; set; }
        public DbSet<Blog> blogs { get; set; }
        public DbSet<Streaming> streamings { get; set; }
        public DbSet<StreamingMember> streamingMembers { get; set; }
        public DbSet<StreamingActivity> streamingActivities { get; set; }
        public DbSet<ErrorLog> errorlog { get; set; }
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var cascadeFKs = builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            base.OnModelCreating(builder);
        }
    }
}
