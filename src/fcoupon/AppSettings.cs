namespace fcoupon
{
	public class AppSettings
	{
		public DatabaseSettings Database { get; set; }

		public AppSettings() { }

		public class DatabaseSettings
		{
			public string Url { get; set; }
			public string Name { get; set; }
			public string Host { get; set; }
			public int Port { get; set; }
			public string Username { get; set; }
			public string Password { get; set; }
		}
	}
}