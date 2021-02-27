using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    /*public class BackupTest : TestBase
    {
        [Fact]
        public async Task BackupRestoreDelete()
        {
            await SetupAsync("system-camel");

            var version = await Arango.GetVersionAsync();

            if (version.License != "enterprise")
                return;

            var backup = await Arango.Backup.CreateAsync(new ArangoBackupRequest
            {
                AllowInconsistent = false,
                Force = true,
                Label = "test",
                Timeout = 30
            });

            await Task.Delay(1000);
            await Arango.Backup.ListAsync();

            await Arango.Backup.RestoreAsync(backup.Id);

            await Task.Delay(1000);
            await Arango.Backup.DeleteAsync(backup.Id);
        }
    }*/
}