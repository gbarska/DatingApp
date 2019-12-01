
namespace DatingApp.API.Helpers
{
    public class LikersHeader
    {
        public bool Match { get; set; }
      public LikersHeader(bool match)
      {
          this.Match =match;
      }
    }
}