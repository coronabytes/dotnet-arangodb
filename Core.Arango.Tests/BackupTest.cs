namespace Core.Arango.Tests
{
    /*public class BackupTest : TestBase
    {
        [Fact]
        public async ValueTask BackupRestoreDelete()
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