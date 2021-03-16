using Serilog.Sinks.Splunk.Plus;
using Serilog.Sinks.SplunkPlus.Tests.Support;
using Xunit;

namespace Serilog.Sinks.SplunkPlus.Tests.Durable
{
    public class BookmarkFileTests
    {
        [Fact]
        public void BookmarkPersistenceCanBeRoundTripped()
        {
            using (var tmp = new TempFolder())
            {
                var position = new FileSetPosition(1234, Some.String());

                var bookmark = new BookmarkFile(tmp.AllocateFilename("bookmark"));
                bookmark.WriteBookmark(position);

                var read = bookmark.TryReadBookmark();
                Assert.Equal(position.NextLineStart, read.NextLineStart);
                Assert.Equal(position.File, read.File);
            }
        }
    }
}
